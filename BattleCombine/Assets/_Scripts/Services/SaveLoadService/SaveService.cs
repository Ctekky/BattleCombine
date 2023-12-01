using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

namespace BattleCombine.Services
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private bool encryptData;
        [SerializeField] private bool firstStart;
        [SerializeField] private bool newGameBattle;

        private GameData _gameData;
        private FileDataHandler _dataHandler;
        private string _fileName;
        private List<ISaveLoad> _saveInterfacesInScripts;
        

        public bool NewGameBattle { get => newGameBattle; set => newGameBattle = value; }

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
            _gameData = new GameData();
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
                firstStart = true;
                Debug.Log("No save data found!");
                NewGame();
                return;
            }

            foreach (var loadScript in _saveInterfacesInScripts)
            {
                loadScript.LoadData(_gameData, NewGameBattle, firstStart);
                firstStart = false;
            }
        }
        public void SaveGame()
        {
            foreach (var saveScript in _saveInterfacesInScripts)
            {
                if (firstStart == true & saveScript is Player)
                {
                    _gameData.battleStatsData.Add(new BattleStatsData());
                }
                
                saveScript.SaveData(ref _gameData, NewGameBattle, firstStart);
            }
            _dataHandler.Save(_gameData);
        }

        private void OnApplicationQuit()
        {
            SaveGame();
        }

        private List<ISaveLoad> FindAllSaveAndLoadInterfaces() //TODO
        {
            List<ISaveLoad> saveInterfacesInScripts = new List<ISaveLoad>();

            var saveBattleStats = FindObjectsOfType<Character>().OfType<ISaveLoad>();
            var saveOthers = FindObjectsOfType<PlayerAccount>().OfType<ISaveLoad>();
            saveInterfacesInScripts.AddRange(saveBattleStats);
            saveInterfacesInScripts.AddRange(saveOthers);
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