using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using Define.DefineConstant;
using System.Runtime.InteropServices;

namespace FrameOfSystem3.Functional.SerializableObject
{
    public enum SerializableType
    {
        XML,
        Binary,
    }

    [Serializable]
    public abstract class ObjectSerializer
    {
        public ObjectSerializer()
        {

        }

        public SerializableType SerializableType
        {
            get
            {
                return _serializableType;
            }
        }
        
        readonly SerializableType _serializableType;

        public static bool Load<TObject>(string fullFilename, ref TObject deserializedObject) where TObject : ObjectSerializer
        {
            if (false == DeserializeFromFile(deserializedObject.SerializableType, fullFilename, ref deserializedObject))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool Save<TObject>(string fullFilename, TObject serializingObject) where TObject : ObjectSerializer
        {
            return SerializeToFile(serializingObject.SerializableType, fullFilename, serializingObject);
        }


        protected static string GetFileExtensionBySerializableType(SerializableType type)
        {
            switch (type)
            {
                case SerializableType.Binary:
                    return ".bin";
                case SerializableType.XML:
                default:
                    return ".xml";
            }
        }

        static bool SerializeToFile<T>(SerializableType storageType, string fullFilename, T targetObject)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Path.GetDirectoryName(fullFilename));
                if (dirInfo.Exists == false)
                {
                    dirInfo.Create();
                }

                using (FileStream fs = new FileStream(fullFilename, FileMode.Create, FileAccess.Write))
                {
                    switch (storageType)
                    {
                        case SerializableType.Binary:
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            binaryFormatter.Serialize(fs, targetObject);
                            break;
                        case SerializableType.XML:
                        default:
                            XmlSerializer serializer = new XmlSerializer(targetObject.GetType());
                            serializer.Serialize(fs, targetObject);
                            break;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        static bool DeserializeFromFile<T>(SerializableType storageType, string fullFilename, ref T targetObject) where T : class
        {
            bool success = false;
            try
            {
                if (false == File.Exists(fullFilename))
                {
                    return success;
                }

                using (FileStream fs = new FileStream(fullFilename, FileMode.Open, FileAccess.Read))
                {
                    switch (storageType)
                    {
                        case SerializableType.Binary:
                            BinaryFormatter binaryFormatter = new BinaryFormatter();
                            targetObject = binaryFormatter.Deserialize(fs) as T;
                            break;

                        case SerializableType.XML:
                        default:
                            XmlSerializer serializer = new XmlSerializer(typeof(T));
                            targetObject = serializer.Deserialize(fs) as T;
                            break;
                    }

                    success = true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                success = false;
            }

            return success;
        }

        public static T DeepCopySerializableObject<T>(T source)
        {
            if (false == typeof(T).IsSerializable)
            {
                return source;
            }

            try
            {
                using (var ms = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(ms, source);
                    ms.Position = 0;
                    return (T)formatter.Deserialize(ms);
                }
            }
            catch(Exception)
            {
                return source;
            }
        }

        static bool DeepCopySerializableObjectToMemoryStream<T>(T source, ref MemoryStream memoryStream)
        {
            if (false == typeof(T).IsSerializable)
            {
                return false;
            }

            try
            {
                if (memoryStream == null)
                {
                    memoryStream = new MemoryStream();
                }
                var formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, source);
                memoryStream.Position = 0;
            }
            catch
            {
                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
                return false;
            }
            return true;
        }

        static bool GetSerializableObjectFromMemoryStream<T>(MemoryStream memoryStream, ref T serializableObject)
        {
            if (false == typeof(T).IsSerializable || memoryStream == null)
            {
                return false;
            }

            try
            {
                var formatter = new BinaryFormatter();
                serializableObject = (T)formatter.Deserialize(memoryStream);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
