using System.Collections;
using _Scripts.Audio;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class UiHelperBattleScene : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string gameLoopScene = "ArcadeGameLoop";

		[Header("Buttons")]
		[SerializeField] private Button _settingsButton;
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _boostInBattleButton;
		[SerializeField] private Button _continueInBattleButton;

		[Header("Game Panels")]
		[SerializeField] private BoostPanel _boostPanel;
		[SerializeField] private PausePanel _pausePanel;
		[SerializeField] private SettingsPanel _settingsPanel;
		[SerializeField] private SoundHelper _soundHelper;
		[SerializeField] private UIWalletUpdate _uiWalletUpdate;
		[SerializeField] private Curtain _curtain;
		[SerializeField] private ResultPanel _winPanel;
		[SerializeField] private ResultPanel _losePanel;

		[Header("Text panels")]
		[SerializeField] private TMP_Text _roundCountText;

		private Coroutine _sceneLoad;

		private void Awake()
		{
			_settingsButton.onClick.AddListener(()=> OnOptionsButtonClick(_settingsPanel.isActiveAndEnabled));
			_pauseButton.onClick.AddListener(()=>OnPauseButtonClick(_pausePanel.isActiveAndEnabled));
			_boostInBattleButton.onClick.AddListener((() => OnBoostButtonClick(_boostPanel.isActiveAndEnabled)));
			_continueInBattleButton.onClick.AddListener(OnContinueClick);
			_settingsPanel.GetSettingsCloseButton.onClick.AddListener(OnCloseButtonClick);
			_pausePanel.GetCloseButton.onClick.AddListener(OnCloseButtonClick);
			_pausePanel.GetMenuButton.onClick.AddListener(OnOptionsButtonInPauseClick);
			_pausePanel.GetPauseContinueButton.onClick.AddListener(OnCloseButtonClick);
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
		}

		private void OnPauseButtonClick(bool active)
		{
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
			_settingsButton.onClick.RemoveListener(()=> OnOptionsButtonClick(_settingsPanel.isActiveAndEnabled));
			_pauseButton.onClick.RemoveListener(()=>OnPauseButtonClick(_pausePanel.isActiveAndEnabled));
			_boostInBattleButton.onClick.RemoveListener((() => OnBoostButtonClick(_boostPanel.isActiveAndEnabled)));
			_continueInBattleButton.onClick.RemoveListener(OnContinueClick);
			_settingsPanel.GetSettingsCloseButton.onClick.RemoveListener(OnCloseButtonClick);
			_pausePanel.GetCloseButton.onClick.RemoveListener(OnCloseButtonClick);
			_pausePanel.GetMenuButton.onClick.RemoveListener(OnOptionsButtonInPauseClick);
			_pausePanel.GetPauseContinueButton.onClick.RemoveListener(OnCloseButtonClick);
		}
	}
}