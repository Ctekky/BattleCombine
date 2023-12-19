using System.Collections;
using System;
using _Scripts.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class UiHelper : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string enemySelectScene = "EnemySelectionScene";
		private const string gameLoopScene = "ArcadeGameLoop";

		public event Action BattleButtonClickEvent;

		[Header("Buttons")]
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _boostButton;
		[SerializeField] private Button _continueBoostButton;
		[Header("Buttons(OnlySelectEnemyScene")]
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _firstEnemyButton;
		[SerializeField] private Button _secondEnemyButton;
		[SerializeField] private Button _thirdEnemyButton;
		[SerializeField] private Button _reRollButton;
		[SerializeField] private Button _closeMatchPanelButton;
		[SerializeField] private Button _startBattleButton;

		[Header("Game Panels")]
		[SerializeField] private BoostPanel _boostPanel;
		[SerializeField] private PausePanel _pausePanel;
		[SerializeField] private SettingsPanel _settingsPanel;
		[SerializeField] private SoundHelper _soundHelper;
		[SerializeField] private UIWalletUpdate _uiWalletUpdate;
		[SerializeField] private ResultPanel _winPanel;
		[SerializeField] private ResultPanel _losePanel;
		[SerializeField] private MatchPanel _matchPanel;
		[SerializeField] private Curtain _curtain;

		[Header("Text panels")]
		[SerializeField] private TMP_Text _roundCountText;

		[Header("Texts values")]
		[SerializeField] private TMP_Text _reRollPriceText;
		[SerializeField] private TMP_Text _levelMatchPanelText;

		private Coroutine _sceneLoad;
		private Scene _currentScene;

		private bool _isMatchPanelActive = false;

		private void Awake()
		{
			_currentScene = SceneManager.GetActiveScene();

			AddListeners();
		}

		private void AddListeners()
		{
			_settingsButton.onClick.AddListener(() => OnOptionsButtonClick(_settingsPanel.isActiveAndEnabled));
			_boostButton.onClick.AddListener((() => OnBoostButtonClick(_boostPanel.isActiveAndEnabled)));
			_continueBoostButton.onClick.AddListener(OnContinueClick);
			_settingsPanel.GetSettingsCloseButton.onClick.AddListener(OnCloseButtonClick);

			switch (_currentScene.name)
			{
				case gameLoopScene:
					_pauseButton.onClick.AddListener(() => OnPauseButtonClick(_pausePanel.isActiveAndEnabled));
					_pausePanel.GetCloseButton.onClick.AddListener(OnCloseButtonClick);
					_pausePanel.GetMenuButton.onClick.AddListener(OnOptionsButtonInPauseClick);
					_pausePanel.GetPauseContinueButton.onClick.AddListener(OnCloseButtonClick);
					break;
				case enemySelectScene:
					_firstEnemyButton.onClick.AddListener(() => CheckToggleGroup(0));
					_secondEnemyButton.onClick.AddListener(() => CheckToggleGroup(1));
					_thirdEnemyButton.onClick.AddListener(() => CheckToggleGroup(2));
					_nextButton.onClick.AddListener(() => OnNextButtonClick(_matchPanel.isActiveAndEnabled));
					_reRollButton.onClick.AddListener(OnReRollButtonClick);
					_closeMatchPanelButton.onClick.AddListener(OnCloseButtonClick);
					_startBattleButton.onClick.AddListener(OnBattleButtonClick);
					break;
			}
		}

		//todo - rework
		public void ShowMatchResult(bool isWin, int score = 0, int bestScore = 0, int diamonds = 0, int coins = 0, int exp = 0, int attack = 0)
		{
			var resultPanel = isWin ? _winPanel : _losePanel;

			resultPanel.SetScore(score, bestScore);
			resultPanel.SetRewardText(coins, diamonds, exp, attack);

			resultPanel.gameObject.SetActive(true);
			WalletUpdate();
		}

		private void WalletUpdate()
		{
			_uiWalletUpdate.UpdateWallet();
		}

		private void OnCloseButtonClick()
		{
			_soundHelper.PlayClickSound();
			OnPauseButtonClick(true);
			OnOptionsButtonClick(true);
			OnBoostButtonClick(true);
			OnMatchCloseButtonClick(true);
		}

		private void OnMatchCloseButtonClick(bool active)
		{
			if(_currentScene.name == enemySelectScene)
				_matchPanel.gameObject.SetActive(!active);
		}

		private void OnPauseButtonClick(bool active)
		{
			if(_currentScene.name == gameLoopScene)
				_pausePanel.gameObject.SetActive(!active);
		}

		private void OnOptionsButtonClick(bool active)
		{
			_settingsPanel.gameObject.SetActive(!active);
		}

		private void OnBoostButtonClick(bool active)
		{
			_boostPanel.gameObject.SetActive(!active);
		}

		private void OnContinueClick()
		{
			//todo - no boost select effects
			Debug.Log("ContinueButton Clicked!");
			OnCloseButtonClick();
		}

		private void OnOptionsButtonInPauseClick()
		{
			OnPauseButtonClick(_pausePanel.isActiveAndEnabled);
			OnOptionsButtonClick(_settingsPanel.isActiveAndEnabled);
		}

		private void OnSceneExit()
		{
			//todo - add functional to switch scenes
			_sceneLoad = StartCoroutine(OnSceneLoadRoutine(initialScene));
		}

		private void OnNextButtonClick(bool active)
		{
			Debug.Log("Match panel Active = " + _isMatchPanelActive);
			_matchPanel.gameObject.SetActive(!active);
		}

		private void OnReRollButtonClick()
		{
			Debug.Log("ReRollButton Clicked!");
		}

		private void OnBattleButtonClick()
		{
			BattleButtonClickEvent?.Invoke();
			_curtain.MoveToAnotherScene(gameLoopScene);
		}

		private void CheckToggleGroup(int number)
		{
			Debug.Log("Enemy " + number + " is Selected!");
		}

		private IEnumerator OnSceneLoadRoutine(string sceneName)
		{
			//извращение, как оно есть 2... :D
			_curtain.gameObject.SetActive(true);
			var panel = _curtain.GetCurtainImage;
			var changeRate = 0.01f;

			panel.raycastTarget = true;

			panel.color = new Color(panel.color.r, panel.color.g, panel.color.b, 0);
			while (panel.color.a < 1.0f)
			{
				var newColor = panel.color;
				newColor.a += changeRate;
				panel.color = newColor;
				yield return new WaitForEndOfFrame();
			}

			panel.raycastTarget = false;

			StopCoroutine(_sceneLoad);
			SceneManager.LoadScene(initialScene);
			_curtain.gameObject.SetActive(false);
		}

		private void OnDisable()
		{
			_settingsButton.onClick.RemoveListener(() => OnOptionsButtonClick(_settingsPanel.isActiveAndEnabled));
			_boostButton.onClick.RemoveListener((() => OnBoostButtonClick(_boostPanel.isActiveAndEnabled)));
			_continueBoostButton.onClick.RemoveListener(OnContinueClick);
			_settingsPanel.GetSettingsCloseButton.onClick.AddListener(OnCloseButtonClick);

			switch (_currentScene.name)
			{
				case gameLoopScene:
					_pauseButton.onClick.RemoveListener(() => OnPauseButtonClick(_pausePanel.isActiveAndEnabled));
					_pausePanel.GetCloseButton.onClick.RemoveListener(OnCloseButtonClick);
					_pausePanel.GetMenuButton.onClick.RemoveListener(OnOptionsButtonInPauseClick);
					_pausePanel.GetPauseContinueButton.onClick.RemoveListener(OnCloseButtonClick);
					break;
				case enemySelectScene:
					_firstEnemyButton.onClick.RemoveListener(() => CheckToggleGroup(0));
					_secondEnemyButton.onClick.RemoveListener(() => CheckToggleGroup(1));
					_thirdEnemyButton.onClick.RemoveListener(() => CheckToggleGroup(2));
					_nextButton.onClick.RemoveListener(() => OnNextButtonClick(_matchPanel.isActiveAndEnabled));
					_reRollButton.onClick.RemoveListener(OnReRollButtonClick);
					_closeMatchPanelButton.onClick.RemoveListener(OnCloseButtonClick);
					_startBattleButton.onClick.RemoveListener(OnBattleButtonClick);
					break;
			}
		}
	}
}