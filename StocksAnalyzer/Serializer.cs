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
        private object CurrentType = null;
        private string CurrentFile = "";
        private FileStream FS;
        //s - Имя файла. t - объект, который надо сериализировать.
        public Serializer(string s, object t)
        {
            CurrentType = t;
            CurrentFile = s;
        }

        public void Serialize()
        {
            FS = new FileStream(CurrentFile,
                FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(FS, CurrentType);
            FS.Close();
        }

        public object Deserialize()
        {
            object a1;
            FS = new FileStream(CurrentFile,
                FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter bf = new BinaryFormatter();
            a1 = bf.Deserialize(FS);
            FS.Close();
            return a1;
        }

        public void Close()
        {
            CurrentType = null;
            CurrentFile = "";
        }

    }
}
