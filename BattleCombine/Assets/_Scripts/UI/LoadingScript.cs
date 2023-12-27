using System;
using System.Collections;
using System.Collections.Generic;
using BattleCombine.Data;
using BattleCombine.Interfaces;
using BattleCombine.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI
{
    public class LoadingScript : MonoBehaviour, ISaveLoad
    {
        [SerializeField] private TMP_Text _playerNameText;
        [SerializeField] private TMP_Text _loadingProgressText;
        [SerializeField] private Curtain _curtain;

        [Header("Loading progress bars")] [SerializeField]
        private List<GameObject> _barImages;

        private float loadProgress = 0;

        [Inject] private InitialSceneService _initialSceneService;

        public string SetPlayerName
        {
            get => _playerNameText.text;
            set => _playerNameText.text = value;
        }

        public float SetLoadProgress
        {
            get => loadProgress;
            set
            {
                if (loadProgress == value)
                    return;

                loadProgress = value;
                ChangeProgressText();
            }
        }

        private void Start()
        {
            foreach (var item in _barImages)
            {
                item.SetActive(false);
            }
            _initialSceneService.Initialize();
            StartCoroutine(CountRoutine());
        }

        private void ChangeProgressText()
        {
            _loadingProgressText.text = loadProgress + "%";
            UpdateLoadingBar();
        }

        private void UpdateLoadingBar()
        {
            var value = Convert.ToInt32(loadProgress / 10);
            if (value < 10)
                _barImages[value].SetActive(true);
        }

        //todo - test coroutine
        private IEnumerator CountRoutine()
        {
            var count = 0;
            while (loadProgress < 100)
            {
                yield return new WaitForSeconds(.01f);
                count++;
                SetLoadProgress = count;
            }

            _curtain.gameObject.SetActive(true);
            Destroy(gameObject);
            _initialSceneService.LoadBattleScene();
        }

        public void LoadData(GameData gameData, bool newGameBattle, bool firstStart)
        {
            _playerNameText.text = "Hello, " + gameData.PlayerAccountData.PlayerName;
        }

        public void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart)
        {
        }
    }
}