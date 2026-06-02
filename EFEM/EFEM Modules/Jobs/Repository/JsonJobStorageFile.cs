using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace EFEM.Jobs.Repository
{
    internal static class JsonJobStorageFile
    {
        public const string OrderFileName = "Order.json";

        private static readonly JsonSerializerSettings _jsonSettings =
            new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ObjectCreationHandling = ObjectCreationHandling.Replace,
                Converters = new List<JsonConverter>
                {
                    new ReadOnlyMaterialInfoJsonConverter()
                }
            };

        public static string EntityPath(string dir, string key)
        {
            return Path.Combine(dir, ToSafeFileName(key) + ".json");
        }

        public static string OrderPath(string dir)
        {
            return Path.Combine(dir, OrderFileName);
        }

        public static T Load<T>(string path)
        {
            string json = File.ReadAllText(path, Encoding.UTF8);

            return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
        }

        public static void SaveAtomic<T>(string path, T value)
        {
            string dir = Path.GetDirectoryName(path);

            if (!string.IsNullOrWhiteSpace(dir))
                Directory.CreateDirectory(dir);

            string tmp = path + "." + Guid.NewGuid().ToString("N") + ".tmp";
            string bak = path + ".bak";

            bool tmpCreated = false;

            try
            {
                string json = JsonConvert.SerializeObject(value, _jsonSettings);

                var utf8NoBom = new UTF8Encoding(false);

                using (var fs = new FileStream(
                    tmp,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    8192,
                    false))
                using (var writer = new StreamWriter(fs, utf8NoBom))
                {
                    tmpCreated = true;

                    writer.Write(json);
                    writer.Flush();

                    try
                    {
                        fs.Flush(true);
                    }
                    catch
                    {
                        fs.Flush();
                    }
                }

                if (File.Exists(path))
                {
                    try
                    {
                        File.Replace(tmp, path, bak);
                    }
                    catch
                    {
                        SafeMoveReplace(tmp, path);
                    }
                }
                else
                {
                    File.Move(tmp, path);
                }

                tmpCreated = false;
            }
            finally
            {
                if (tmpCreated)
                {
                    try
                    {
                        if (File.Exists(tmp))
                            File.Delete(tmp);
                    }
                    catch
                    {
                    }
                }
            }
        }

        private static string ToSafeFileName(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key is required.", nameof(key));

            return Uri.EscapeDataString(key);
        }

        private static void SafeMoveReplace(string src, string dst)
        {
            try
            {
                if (File.Exists(dst))
                    File.Delete(dst);

                File.Move(src, dst);
            }
            catch
            {
                try
                {
                    File.Copy(src, dst, true);
                    File.Delete(src);
                }
                catch
                {
                }
            }
        }

        public static T LoadOrBackup<T>(string path)
        {
            try
            {
                return Load<T>(path);
            }
            catch (Exception activeException)
            {
                string bakPath = path + ".bak";

                if (!File.Exists(bakPath))
                    throw;

                try
                {
                    return Load<T>(bakPath);
                }
                catch (Exception backupException)
                {
                    throw new InvalidDataException(
                        "JSON storage load failed. Active and backup files are both invalid. Path="
                        + path
                        + ", ActiveError="
                        + activeException.Message
                        + ", BackupError="
                        + backupException.Message,
                        backupException);
                }
            }
        }

        private sealed class ReadOnlyMaterialInfoJsonConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(IReadOnlyDictionary<string, IReadOnlyList<int>>);
            }

            public override object ReadJson(
                JsonReader reader,
                Type objectType,
                object existingValue,
                JsonSerializer serializer)
            {
                if (reader.TokenType == JsonToken.Null)
                    return null;

                JObject obj = JObject.Load(reader);

                var result = new Dictionary<string, IReadOnlyList<int>>(StringComparer.Ordinal);

                foreach (var property in obj.Properties())
                {
                    var values = property.Value.ToObject<List<int>>(serializer);

                    if (values == null)
                        values = new List<int>();

                    result[property.Name] = values;
                }

                return result;
            }

            public override void WriteJson(
                JsonWriter writer,
                object value,
                JsonSerializer serializer)
            {
                var materialInfo = value as IReadOnlyDictionary<string, IReadOnlyList<int>>;

                if (materialInfo == null)
                {
                    writer.WriteNull();
                    return;
                }

                writer.WriteStartObject();

                foreach (var item in materialInfo)
                {
                    writer.WritePropertyName(item.Key);
                    serializer.Serialize(writer, item.Value);
                }

                writer.WriteEndObject();
            }
        }
    }
}