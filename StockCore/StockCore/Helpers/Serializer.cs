using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace StocksAnalyzer
{
	public class Serializer
	{
		private readonly string m_mCurrentFilePath;

		/// <summary>
		/// Инициализирует сериалайзер
		/// </summary>
		/// <param name="path">Путь к файлу</param>
		public Serializer(string path)
		{
			m_mCurrentFilePath = path;
		}

		/// <summary>
		/// Сериализует объект, указанный при инициализации объекта класса
		/// </summary>
		public void Serialize(object objectToSerialize)
		{
			using (var fs = new FileStream(m_mCurrentFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
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
			using (var fs = new FileStream(m_mCurrentFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				BinaryFormatter bf = new BinaryFormatter();
				try
				{
					return bf.Deserialize(fs);
				}
				catch (SerializationException ser)
				{
					Logger.Log.Fatal(ser);
				}
				catch (ArgumentException ser)
				{
					Logger.Log.Fatal(ser);
				}
			}
			RenameFileIfError(m_mCurrentFilePath);
			return null;
		}

		private void RenameFileIfError(string oldFileName)
		{
			var i = 1;
			while (File.Exists($"{oldFileName}_{i}"))
				i++;
			File.Move(oldFileName, $"{oldFileName}_{i}");
		}
	}
}