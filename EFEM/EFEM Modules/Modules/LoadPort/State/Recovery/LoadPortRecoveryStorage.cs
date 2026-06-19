using System;
using System.IO;
using Newtonsoft.Json;

namespace EFEM.Modules.LoadPort.Recovery
{
    public static class LoadPortRecoveryStorage
    {
        public static void Save(string filePath, LoadPortRecoveryData data)
        {
            if (data == null)
                return;

            data.SavedAtUtc = DateTime.UtcNow;

            var json = JsonConvert.SerializeObject(
                data,
                Formatting.Indented);

            var dirPath = Path.GetDirectoryName(filePath);
            if (false == Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            File.WriteAllText(filePath, json);
        }

        public static LoadPortRecoveryData Load(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var json = File.ReadAllText(filePath);

            return JsonConvert.DeserializeObject<LoadPortRecoveryData>(json);
        }
    }
}