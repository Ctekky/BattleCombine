using System;
using _Scripts.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
    public class UiHelper : MonoBehaviour
    {
        private const string initialScene = "Initial";
        private const string enemySelectScene = "EnemySelectionScene";
        private const string gameLoopScene = "ArcadeGameLoop";

        public event Action BattleButtonClickEvent;
        public event Action RerollButtonClickEvent; 
        public event Action<string> BattleSceneExitEvent;

        [Header("Buttons")] [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _boostButton;
        [SerializeField] private Button _continueBoostButton;
        [SerializeField] private Button _continueWinButton;
        [SerializeField] private Button _leaveLoseButton;

        [Header("Buttons(OnlySelectEnemyScene")] [SerializeField]
        private Button _nextButton;

        [SerializeField] private Button _reRollButton;
        [SerializeField] private Button _closeMatchPanelButton;
        [SerializeField] private Button _startBattleButton;

        [Header("Game Panels")] [SerializeField]
        private BoostPanel _boostMenu;

        [SerializeField] private PauseMenu _pauseMenu;
        [SerializeField] private SettingsMenu _settingsMenu;
        [SerializeField] private SoundHelper _soundHelper;
        [SerializeField] private UIWalletUpdate _uiWalletUpdate;
        [SerializeField] private Result _win;
        [SerializeField] private Result _lose;
        [SerializeField] private LevelUpMenu _levelUpMenu;
        [SerializeField] private MatchPanel _matchPanel;
        [SerializeField] private Curtain _curtain;

        [Header("Text panels")] [SerializeField]
        private TMP_Text _roundCountText;

        [Header("Texts values")] [SerializeField]
        private TMP_Text _reRollPriceText;

        [SerializeField] private TMP_Text _levelMatchPanelText;

        [Header("Help parameters")] [SerializeField]
        private Sprite enableSprite;

        [SerializeField] private Sprite disableSprite;
        [SerializeField] private bool isNextButtonEnable;
        [SerializeField] private bool isRerollButtonEnable;

        [Inject]
        private AudioService audioService;
        
        private Scene _currentScene;

        private bool _isMatchPanelActive = false;
        public Action<int> onStatChange;
        public Action onLoseClick;
        public Action onWinClick;
        public Action OnEndBattle;

        private void OnEnable()
        {
            _currentScene = SceneManager.GetActiveScene();
            BattleSceneExitEvent += OnSceneExit;
            if (_levelUpMenu != null) _levelUpMenu.StatChangeEvent += LevelUpStat;
            if (_nextButton != null) _nextButton.image.sprite = disableSprite;
            if (_pauseMenu != null) _pauseMenu.OnLeaveGameButtonPress += EndBattle;
            isNextButtonEnable = false;
            isRerollButtonEnable = true;
            AddListeners();
        }

        private void EndBattle()
        {
            OnEndBattle?.Invoke();
        }

        private void LevelUpStat(int key)
        {
            onStatChange?.Invoke(key);
            _levelUpMenu.gameObject.SetActive(false);
        }

        public void NextButtonActivate()
        {
            isNextButtonEnable = true;
            _nextButton.image.sprite = enableSprite;
        }

        public void NextButtonDeactivate()
        {
            isNextButtonEnable = false;
            _nextButton.image.sprite = disableSprite;
        }

        public void ShowMatchResult(bool isWin, int score = 0, int bestScore = 0, int diamonds = 0, int coins = 0,
            int exp = 0, int attack = 0)
        {
            var result = isWin ? _win : _lose;

            result.SetScore(score, bestScore);
            result.SetRewardText(coins, diamonds, exp, attack);

            result.gameObject.SetActive(true);
            WalletUpdate();
        }

        public void ShowLevelUp(int level = 0, int attack = 0, int health = 0, int speed = 0)
        {
            _levelUpMenu.SetLevelUpText(level, attack, health, speed);

            _levelUpMenu.gameObject.SetActive(true);
        }

        public void ExitBattleScene(string value = enemySelectScene)
        {
            BattleSceneExitEvent?.Invoke(value);
        }

        private void AddListeners()
        {
            _settingsButton.onClick.AddListener(() => OnOptionsButtonClick(_settingsMenu.isActiveAndEnabled));
            _boostButton.onClick.AddListener((() => OnBoostButtonClick(_boostMenu.isActiveAndEnabled)));
            _continueBoostButton.onClick.AddListener(OnContinueClick);
            _settingsMenu.GetSettingsCloseButton.onClick.AddListener(OnCloseButtonClick);
            if (_leaveLoseButton != null) _leaveLoseButton.onClick.AddListener(OnLoseLeaveButtonClick);
            if (_continueWinButton != null) _continueWinButton.onClick.AddListener(OnWinContinueClick);

            switch (_currentScene.name)
            {
                case gameLoopScene:
                    _pauseButton.onClick.AddListener(() => OnPauseButtonClick(_pauseMenu.isActiveAndEnabled));
                    _pauseMenu.GetCloseButton.onClick.AddListener(OnCloseButtonClick);
                    _pauseMenu.GetMenuButton.onClick.AddListener(OnOptionsButtonInPauseClick);
                    _pauseMenu.GetPauseContinueButton.onClick.AddListener(OnCloseButtonClick);
                    break;
                case enemySelectScene:
                    _nextButton.onClick.AddListener(() => OnNextButtonClick(_matchPanel.isActiveAndEnabled));
                    _reRollButton.onClick.AddListener(OnReRollButtonClick);
                    _closeMatchPanelButton.onClick.AddListener(OnCloseButtonClick);
                    _startBattleButton.onClick.AddListener(OnBattleButtonClick);
                    break;
            }
        }

        private void OnLoseLeaveButtonClick()
        {
            onLoseClick?.Invoke();
        }

        private void OnWinContinueClick()
        {
            onWinClick?.Invoke();
        }

        public void OnCloseButtonClick()
        {
            OnPauseButtonClick(true);
            OnOptionsButtonClick(true);
            OnBoostButtonClick(true);
            OnMatchCloseButtonClick(true);
        }

        private void OnMatchCloseButtonClick(bool active)
        {
            if (_currentScene.name == enemySelectScene)
                _matchPanel.gameObject.SetActive(!active);
        }

        private void OnPauseButtonClick(bool active)
        {
            if (_currentScene.name == gameLoopScene)
                _pauseMenu.gameObject.SetActive(!active);
        }

        private void OnContinueClick()
        {
            //todo - no boost select effects
            Debug.Log("ContinueButton Clicked!");
            OnCloseButtonClick();
        }

        private void OnBoostButtonClick(bool active) =>
            _boostMenu.gameObject.SetActive(!active);

        private void OnOptionsButtonClick(bool active) =>
            _settingsMenu.gameObject.SetActive(!active);

        private void OnOptionsButtonInPauseClick()
        {
            OnPauseButtonClick(_pauseMenu.isActiveAndEnabled);
            OnOptionsButtonClick(_settingsMenu.isActiveAndEnabled);
        }

        private void OnNextButtonClick(bool active)
        {
            if (!isNextButtonEnable) return;
            _matchPanel.gameObject.SetActive(!active);
        }

        private void OnReRollButtonClick()
        {
            if (!isRerollButtonEnable) return;
            isRerollButtonEnable = false;
            NextButtonDeactivate();
            RerollButtonClickEvent?.Invoke();
        }

        public void ChangeRerollButtonState(bool state)
        {
            isRerollButtonEnable = state;
        }
        
        private void OnBattleButtonClick()
        {
            BattleButtonClickEvent?.Invoke();
            _curtain.MoveToAnotherScene(gameLoopScene);
        }

        private void WalletUpdate() =>
            _uiWalletUpdate.UpdateWallet();

        private void OnSceneExit(string value) =>
            _curtain.MoveToAnotherScene(value);

        private void OnDisable()
        {
            _settingsButton.onClick.RemoveListener(() => OnOptionsButtonClick(_settingsMenu.isActiveAndEnabled));
            _boostButton.onClick.RemoveListener((() => OnBoostButtonClick(_boostMenu.isActiveAndEnabled)));
            _continueBoostButton.onClick.RemoveListener(OnContinueClick);
            _settingsMenu.GetSettingsCloseButton.onClick.RemoveListener(OnCloseButtonClick);
            if (_leaveLoseButton != null) _leaveLoseButton.onClick.RemoveListener(OnLoseLeaveButtonClick);
            if (_continueWinButton != null) _continueWinButton.onClick.RemoveListener(OnWinContinueClick);
            if (_pauseMenu != null) _pauseMenu.OnLeaveGameButtonPress -= EndBattle;
            if (_levelUpMenu != null) _levelUpMenu.StatChangeEvent -= LevelUpStat;
            switch (_currentScene.name)
            {
                case gameLoopScene:
                    _pauseButton.onClick.RemoveListener(() => OnPauseButtonClick(_pauseMenu.isActiveAndEnabled));
                    _pauseMenu.GetCloseButton.onClick.RemoveListener(OnCloseButtonClick);
                    _pauseMenu.GetMenuButton.onClick.RemoveListener(OnOptionsButtonInPauseClick);
                    _pauseMenu.GetPauseContinueButton.onClick.RemoveListener(OnCloseButtonClick);
                    break;
                case enemySelectScene:
                    _nextButton.onClick.RemoveListener(() => OnNextButtonClick(_matchPanel.isActiveAndEnabled));
                    _reRollButton.onClick.RemoveListener(OnReRollButtonClick);
                    _closeMatchPanelButton.onClick.RemoveListener(OnCloseButtonClick);
                    _startBattleButton.onClick.RemoveListener(OnBattleButtonClick);
                    break;
            }
        }
    }
}