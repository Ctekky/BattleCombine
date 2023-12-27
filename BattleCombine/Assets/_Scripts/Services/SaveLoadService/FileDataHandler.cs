using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

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
                /*
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    //ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    //Formatting = Formatting.None
                };
                var mSavedGameFileContent = JsonConvert.SerializeObject(gameDataNew, settings);
                using FileStream streamFile = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                using StreamWriter sr = new StreamWriter(streamFile);
                sr.Write(mSavedGameFileContent);
                sr.Flush();
                sr.Close();
                streamFile.Close();
                */
                
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

        public GameData Load()
        {
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData gd = null;
            if (!File.Exists(fullPath))
            {
                return gd;
            }
            try
            {
                /*
                using FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                using StreamReader sr = new StreamReader(fileStream);
                var mSavedGameFileContent = sr.ReadToEnd();
                sr.Close();
                fileStream.Close();
                
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All,
                    //ObjectCreationHandling = ObjectCreationHandling.Replace
                };

                gd = JsonConvert.DeserializeObject<GameData>(mSavedGameFileContent, settings);
                */
                
                using FileStream fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                BinaryFormatter bf = new BinaryFormatter();
                gd = (GameData)bf.Deserialize(fileStream);
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

