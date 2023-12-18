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

		[FormerlySerializedAs("_menuButton")]
		[Header("Buttons")]
		[SerializeField] private Button _optionsButton;
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _boostInBattleButton;
		[SerializeField] private Button _continueInBattleButton;

		[Header("Game Panels")]
		[SerializeField] private GameObject _boostPanel;
		[SerializeField] private PausePanel _pausePanel;
		[SerializeField] private SettingsPanel _settingsPanel;
		[SerializeField] private SoundHelper _soundHelper;
		[FormerlySerializedAs("_walletPanel")]
		[SerializeField] private UIWalletUpdate _uiWalletUpdate; //todo - add ScoreMechanics _wallet.AddScore(value)
		[SerializeField] private GameObject _curtain;
		[SerializeField] private ResultPanel _winPanel;
		[SerializeField] private ResultPanel _losePanel;

		[Header("Text panels")]
		[SerializeField] private TMP_Text _roundCountText;

		private Coroutine _sceneLoad;

		private bool _isBoostPanelActive = false;
		private bool _isOptionsPanelActive = false;
		private bool _isPausePanelActive = false;


		private void Awake()
		{
			_optionsButton.onClick.AddListener(OnOptionsButtonClick);
			_pauseButton.onClick.AddListener(OnPauseButtonClick);
			_boostInBattleButton.onClick.AddListener(OnBoostButtonClick);
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
			_isOptionsPanelActive = true;
			_isPausePanelActive = true;
			_isBoostPanelActive = true;

			OnPauseButtonClick();
			OnOptionsButtonClick();
			ClosePanel();
		}

		private void OnPauseButtonClick()
		{
			_soundHelper.PlayClickSound();
			_isPausePanelActive = !_isPausePanelActive;
			Debug.Log("Options Active = " + _isPausePanelActive);
			_pausePanel.gameObject.SetActive(_isPausePanelActive);
		}

		private void OnOptionsButtonClick()
		{
			_isOptionsPanelActive = !_isOptionsPanelActive;
			Debug.Log("Options Active = " + _isOptionsPanelActive);
			_settingsPanel.gameObject.SetActive(_isOptionsPanelActive);
		}

		private void OnBoostButtonClick()
		{
			Debug.Log("BoostButton Clicked!");
			ClosePanel();
		}

		private void OnContinueClick()
		{
			//todo - no boost select effects
			Debug.Log("ContinueButton Clicked!");
			ClosePanel();
		}

		private void OnOptionsButtonInPauseClick()
		{
			Debug.Log("Move from Pause to Options");
			OnPauseButtonClick();
			OnOptionsButtonClick();
		}

		private void OnSceneExit()
		{
			//todo - add functional to switch scenes
			_sceneLoad = StartCoroutine(OnSceneLoadRoutine(initialScene));
		}

		private void ClosePanel()
		{
			_soundHelper.PlayClickSound();
			_isBoostPanelActive = !_isBoostPanelActive;
			_boostPanel.SetActive(_isBoostPanelActive);
		}


		private IEnumerator OnSceneLoadRoutine(string sceneName)
		{
			//извращение, как оно есть 2... :D
			_curtain.SetActive(true);
			var panel = _curtain.GetComponent<Image>();
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
			_curtain.SetActive(false);
		}
	}
}