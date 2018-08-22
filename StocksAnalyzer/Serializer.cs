using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StocksAnalyzer
{
    class Serializer
    {
        private readonly string _mCurrentFilePath;

        /// <summary>
        /// Инициализирует сериалайзер
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        public Serializer(string path)
        {
            _mCurrentFilePath = path;
        }

        /// <summary>
        /// Сериализует объект, указанный при инициализации объекта класса
        /// </summary>
        public void Serialize(object objectToSerialize)
        {
            using (var fs = new FileStream(_mCurrentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, objectToSerialize);
            }
        }

        /// <summary>
        /// Десериализует объект, указанный при инициализации объекта класса
        /// </summary>
        /// <returns>Полученный объект</returns>
        public object Deserialize()
        {
            using (var fs = new FileStream(_mCurrentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BinaryFormatter bf = new BinaryFormatter();
                var obj = bf.Deserialize(fs);
                return obj;
            }
        }
    }
}