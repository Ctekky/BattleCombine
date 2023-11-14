using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using UnityEngine;

namespace BattleCombine.Services
{
    public class SaveManager : MonoBehaviour
    {
        private GameData _gameData;
        private FileDataHandler _dataHandler;
        private string _fileName;
        private List<ISaveLoad> _saveInterfacesInScripts;
        [SerializeField] private bool encryptData;
        public string sceneName;

        private void Awake()
        {
            _fileName = "data.test";
            encryptData = false;
        }

        private void Start()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, encryptData);
            _saveInterfacesInScripts = FindAllSaveAndLoadInterfaces();
            LoadGame();
        }

        public void AddScriptToList(ISaveLoad script)
        {
            _saveInterfacesInScripts.Add(script);
        }

        private void NewGame()
        {
            _gameData = new GameData
            {
                PlayerName = "New Player"
            };
        }

        public bool CheckForSavedData()
        {
            _gameData = _dataHandler.Load();
            return _gameData != null;
        }

        public void LoadGame()
        {
            _gameData = _dataHandler.Load();
            if (_gameData == null)
            {
                Debug.Log("No save data found!");
                NewGame();
            }

            foreach (var loadScript in _saveInterfacesInScripts)
            {
                loadScript.LoadData(_gameData);
            }
        }

        public void SaveGame()
        {
            foreach (var saveScript in _saveInterfacesInScripts)
            {
                saveScript.SaveData(ref _gameData);
            }

            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISaveLoad> FindAllSaveAndLoadInterfaces()
        {
            var saveInterfacesInScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
            return new List<ISaveLoad>(saveInterfacesInScripts);
        }

        [ContextMenu("Delete Save Data")]
        public void DeleteSavedData()
        {
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, encryptData);
            _dataHandler.Delete();
        }
    }
}