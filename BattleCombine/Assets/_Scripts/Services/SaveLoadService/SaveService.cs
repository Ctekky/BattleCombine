using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using UnityEngine;

namespace BattleCombine.Services
{
    public class SaveManager : MonoBehaviour
    {
        //private GameData _gameData;
        private GameDataNew _gameDataNew;
        private PlayerAccount _playerAccount;
        private BattleStats _battleStats;
        private FileDataHandler _dataHandler;
        private string _fileName;
        private List<ISaveLoad> _saveInterfacesInScripts;
        [SerializeField] private bool encryptData;
        public string sceneName;
        public bool firstSave; //TODO

        private void Awake()
        {
            _fileName = "data.txt";
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
            _gameDataNew = new GameDataNew();
        }

        public bool CheckForSavedData()
        {
            _gameDataNew = _dataHandler.Load();
            return _gameDataNew != null;
        }

        public void LoadGame()
        {
            _gameDataNew = _dataHandler.Load();
            if (_gameDataNew == null)
            {
                firstSave = true;
                Debug.Log("No save data found!");
                NewGame();
                return;
            }

            foreach (var loadScript in _saveInterfacesInScripts)
            {
                loadScript.LoadData(_gameDataNew);
                firstSave = false;
            }
        }
        public void SaveGame()
        {
            foreach (var saveScript in _saveInterfacesInScripts)
            {
                if(firstSave == true)
                {
                    _gameDataNew.battleStats.Add(new BattleStats());
                }
                
                saveScript.SaveData(ref _gameDataNew);
            }
            _dataHandler.Save(_gameDataNew);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISaveLoad> FindAllSaveAndLoadInterfaces()
        {
            var  saveInterfacesInScripts = FindObjectsOfType<Character>().OfType<ISaveLoad>(); //TODO
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