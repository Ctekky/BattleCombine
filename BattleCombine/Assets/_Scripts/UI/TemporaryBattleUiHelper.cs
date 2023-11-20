using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class TemporaryBattleUiHelper : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string gameLoopScene = "DS_scene";

		[FormerlySerializedAs("_menuButton")]
		[Header("Buttons")]
		[SerializeField] private Button _optionsButton;
		[SerializeField] private Button _pauseButton;
		[SerializeField] private Button _boostInBattleButton;
		[SerializeField] private Button _continueInBattleButton;
		[SerializeField] private Button _closeOptionsPanelButton;
		[SerializeField] private Button _closePausePanelButton;
		[SerializeField] private Button _optionsInPauseButton;

		[Header("Boosters")]
		[SerializeField] private Button _lHeartBooster;
		[SerializeField] private Button _heartBooster;
		[SerializeField] private Button _shieldBooster;
		[SerializeField] private Button _speedBooster;

		[Header("Game Panels")]
		[SerializeField] private GameObject _boostPanel;
		[SerializeField] private GameObject _optionPanel;
		[SerializeField] private GameObject _pausePanel;
		[SerializeField] private GameObject _curtain;

		[Header("Hero Avatars")]
		[SerializeField] private Image _playerInBattleAvatar;
		[SerializeField] private Image _enemyInBattleAvatar;

		[Header("Text panels")]
		[SerializeField] private TMP_Text _scoreCountText;
		[SerializeField] private TMP_Text _bestScoreCountText;
		[SerializeField] private TMP_Text _roundCountText;
		[SerializeField] private TMP_Text _playerLevelText;

		[Header("Stat Values (Battle)")]
		[SerializeField] private TMP_Text _inBattleEnemyHealthText;
		[SerializeField] private TMP_Text _inBattleEnemyAttackText;
		//[SerializeField] private TMP_Text _inBattleEnemySpeedText;
		[SerializeField] private TMP_Text _inBattlePlayerHealthText;
		[SerializeField] private TMP_Text _inBattlePlayerAttackText;
		//[SerializeField] private TMP_Text _inBattlePlayerSpeedText;

		[Header("Boost Toggle")]
		[SerializeField] private Toggle[] _boostToggles;
		[Header("Toggle Sprites")]
		[SerializeField] private Sprite _toggleOff;
		[SerializeField] private Sprite _toggleOn;

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
			_closeOptionsPanelButton.onClick.AddListener(OnCloseButtonClick);
			_closePausePanelButton.onClick.AddListener(OnCloseButtonClick);
			_optionsInPauseButton.onClick.AddListener(OnOptionsButtonInPauseClick);

			_lHeartBooster.onClick.AddListener(() => CheckToggleGroup(0));
			_heartBooster.onClick.AddListener(() => CheckToggleGroup(1));
			_shieldBooster.onClick.AddListener(() => CheckToggleGroup(2));
			_speedBooster.onClick.AddListener(() => CheckToggleGroup(3));
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
			_isPausePanelActive = !_isPausePanelActive;
			Debug.Log("Options Active = " + _isPausePanelActive);
			_pausePanel.SetActive(_isPausePanelActive);
		}

		private void OnOptionsButtonClick()
		{
			_isOptionsPanelActive = !_isOptionsPanelActive;
			Debug.Log("Options Active = " + _isOptionsPanelActive);
			_optionPanel.SetActive(_isOptionsPanelActive);
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

		private void CheckToggleGroup(int number)
		{
			foreach (var boost in _boostToggles)
			{
				boost.isOn = false;
				boost.GetComponent<Image>().sprite = _toggleOff;
			}

			_boostToggles[number].isOn = true;
			_boostToggles[number].GetComponent<Image>().sprite = _toggleOn;
			Debug.Log("Boost " + number + " is Selected!");
		}
	}
}