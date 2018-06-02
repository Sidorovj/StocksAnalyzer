using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace StocksAnalyzer
{
    class Serializer
    {
        private object ObjectToSerialize = null;
        private string CurrentFilePath = "";
        private FileStream FS;

        /// <summary>
        /// Инициализирует сериалайзер
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <param name="obj">Объект, который надо сериализовать</param>
        public Serializer(string path, object obj)
        {
            ObjectToSerialize = obj;
            CurrentFilePath = path;
        }

        /// <summary>
        /// Сериализует объект, указанный при инициализации объекта класса
        /// </summary>
        public void Serialize()
        {
            FS = new FileStream(CurrentFilePath,
                FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(FS, ObjectToSerialize);
            Close();
        }

        /// <summary>
        /// Десериализует объект, указанный при инициализации объекта класса
        /// </summary>
        /// <returns>Полученный объект</returns>
        public object Deserialize()
        {
            object _obj;
            FS = new FileStream(CurrentFilePath,
                FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bf = new BinaryFormatter();
            _obj = bf.Deserialize(FS);
            Close();
            return _obj;
        }

        private void Close()
        {
            FS.Close();
            ObjectToSerialize = null;
            CurrentFilePath = "";
        }

    }
}
