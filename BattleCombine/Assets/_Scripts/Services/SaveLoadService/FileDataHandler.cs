using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace BattleCombine.Data
{
    public class FileDataHandler
    {
        private string _dataDirPath = "";
        private string _dataFileName = "";
        private bool _encryptData = false;
        private string _codeWord = "kachachar";

        public FileDataHandler(string dataDirPath, string dataFileName, bool encryptData)
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
            if(File.Exists(fullPath)) File.Delete(fullPath);
        }

        public void Save(GameData gameDataNew)
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting = Formatting.None
                };

                File.WriteAllText(fullPath, JsonConvert.SerializeObject(gameDataNew, settings));
            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to save data to file {fullPath} \n {e}");
            }
        }

        public GameData Load()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData loadData = null;
            if (!File.Exists(fullPath))
            {
                Debug.Log($"There is no save file {fullPath}");
                return null;
            }
            try
            {
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };

                loadData = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(fullPath), settings);
            }
            catch (Exception e)
            {
                Debug.Log($"Error on trying to load data from file {fullPath} \n {e}");
            }
            return loadData;
        }
        
    }
    
}

