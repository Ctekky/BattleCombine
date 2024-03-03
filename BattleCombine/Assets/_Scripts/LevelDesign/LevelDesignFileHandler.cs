using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace BattleCombine.Data
{
    public class LevelDesignFileHandler
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        private bool _encryptData = false;
        private string _codeWord = "kachachar";

        public LevelDesignFileHandler(string dataDirPath, string dataFileName, bool encryptData)
        {
            _dataDirPath = dataDirPath;
            _dataFileName = dataFileName;
            _encryptData = encryptData;
        }

        private string EncryptDecrypt(string data)
        {
            var modifiedData = "";
            for (var i = 0; i < data.Length; i++)
            {
                modifiedData += (char)(data[i] ^ _codeWord[i % _codeWord.Length]);
            }

            return modifiedData;
        }

        public void Delete()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            if (File.Exists(fullPath)) File.Delete(fullPath);
        }

        public void Save(LevelDesignFieldData gameDataNew)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                using FileStream streamFile = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(streamFile, gameDataNew);
                streamFile.Flush();
                streamFile.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to save data to file {fullPath} \n {e}");
            }
        }

        public LevelDesignFieldData Load()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            LevelDesignFieldData gd = null;
            if (!File.Exists(fullPath))
            {
                return gd;
            }

            try
            {
                using FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                gd = (LevelDesignFieldData)bf.Deserialize(fileStream);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to load data from file {fullPath} \n {e}");
            }

            return gd;
        }
    }
}