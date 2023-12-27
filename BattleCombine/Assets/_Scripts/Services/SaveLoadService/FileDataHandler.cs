using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
            BinaryFormatter bf = new BinaryFormatter();
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            FileStream fs = new FileStream(fullPath, FileMode.Create);
            bf.Serialize(fs, gameDataNew);
            fs.Close();
            /*
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
            */
        }

        public GameData Load()
        {
            Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
            var fullPath = Path.Combine(_dataDirPath, _dataFileName);
            GameData gd = null;
            if (File.Exists(fullPath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(fullPath, FileMode.Open);
                
                GameData gameData = bf.Deserialize(fs) as GameData;
                fs.Close();
                return gameData;
            }

            return gd;
            /*
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
            */
        }
        
    }
    
}

