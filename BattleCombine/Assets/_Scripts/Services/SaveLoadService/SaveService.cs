using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Data;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using UnityEngine;
using Zenject;


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

        [Inject] private PlayerAccount _playerAccount;
        

        public bool NewGameBattle
        {
            get => newGameBattle;
            set => newGameBattle = value;
        }

        public void Initialization()
        {
            _fileName = "data.txt";
            encryptData = false;
            _dataHandler = new FileDataHandler(Application.persistentDataPath, _fileName, encryptData);
            _saveInterfacesInScripts = FindAllSaveAndLoadInterfaces();
        }
        
        public void AddScriptToList(ISaveLoad script)
        {
            _saveInterfacesInScripts.Add(script);
        }

        public void NewGame()
        {
            _gameData = new GameData
            {
                PlayerAccountData =
                {
                    //TODO - need to connect with all player base and generate correct unique player ID
                    PlayerID = "playerID",
                    //TODO - create new player name nick generation
                    PlayerName = "Noname new player",
                    CurrentScore = 0,
                    Exp = 0,
                    MaxScore = 0,
                    Gold = 0,
                    Diamond = 0,
                    PlayerAvatarID = 1
                },
                ArcadePlayerLevel = 1
            };
            var playerBattleStateData = new BattleStatsData
            {
                Name = "Player",
                Shield = false,
                DamageDefault = 3,
                DamageModifier = 0,
                CurrentDamage = 3,
                SpeedDefault = 3,
                SpeedModifier = 0,
                CurrentSpeed = 3,
                CurrentHealth = 25,
                HealthDefault = 25,
                HealthModifier = 0
            };
            _gameData.BattleStatsData.Add(playerBattleStateData);
            var playerInfo = _gameData.PlayerAccountData;
            _playerAccount.CreatePlayerAccount(playerInfo.PlayerID, playerInfo.PlayerName, playerInfo.Exp,
                playerInfo.Gold, playerInfo.Diamond, playerInfo.CurrentScore, playerInfo.MaxScore,
                playerInfo.PlayerAvatarID, playerBattleStateData);
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

            _saveInterfacesInScripts = FindAllSaveAndLoadInterfaces();
            foreach (var loadScript in _saveInterfacesInScripts)
            {
                loadScript.LoadData(_gameData, NewGameBattle, firstStart);
                firstStart = false;
            }
        }

        public void SaveGame()
        {
            _saveInterfacesInScripts = FindAllSaveAndLoadInterfaces();
            _gameData.BattleStatsData.Clear();
            foreach (var saveScript in _saveInterfacesInScripts)
            {
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
            /*
            
            var saveInterfacesInScripts = FindObjectsOfType<MonoBehaviour>().OfType<ISaveLoad>();
            //var saveBattleStats = FindObjectsOfType<Character>().OfType<ISaveLoad>();
            //
            saveInterfacesInScripts.AddRange(saveBattleStats);
            saveInterfacesInScripts.AddRange(saveOthers);
            return new List<ISaveLoad>(saveInterfacesInScripts);
            */
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