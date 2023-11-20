using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class TemporaryUIHelper : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string battleScene = "DS_scene";

		[Header("Universal Buttons")]
		[SerializeField] private Button _menuButton;
		[FormerlySerializedAs("_infiniteGameButton")]
		[Header("Initial Buttons")]
		[SerializeField] private Button _ArcadeGameButton;
		[Header("Arcade Buttons")]
		[SerializeField] private Button _nextButton;
		[SerializeField] private Button _firstEnemyButton;
		[SerializeField] private Button _secondEnemyButton;
		[SerializeField] private Button _thirdEnemyButton;
		[SerializeField] private Button _reRollButton;
		[SerializeField] private Button _boostButton;
		[SerializeField] private Button _closeMatchPanelButton;
		[SerializeField] private Button _closeOptionsPanelButton;
		[SerializeField] private Button _startBattleButton;
		[SerializeField] private Button _boostContinueButton;

		[Header("Game Panels")]
		[SerializeField] private GameObject _optionPanel;
		[SerializeField] private GameObject _boostPanel;
		[SerializeField] private GameObject _matchPanel;
		[SerializeField] private GameObject _arcadePanel;
		[SerializeField] private GameObject _initialPanel;
		[SerializeField] private GameObject _walletPanel;
		[SerializeField] private GameObject _curtain;
		[Header("Toggles")]
		[SerializeField] private Toggle[] _enemyToggle;

		[Header("Hero Avatars")]
		[SerializeField] private Image _playerAvatar;
		[SerializeField] private Image _enemyFirstAvatar;
		[SerializeField] private Image _enemySecondAvatar;
		[SerializeField] private Image _enemyThirdAvatar;
		[SerializeField] private Image _playerAvatarMatchPanel;
		[SerializeField] private Image _enemyChosenAvatar;
		[Header("Text panels (Init+Arcade)")]
		[SerializeField] private TMP_Text _coinCountText;
		[SerializeField] private TMP_Text _diamondCountText;
		[Header("Text panels(Arcade)")]
		[SerializeField] private TMP_Text _reRollPriceText;
		[SerializeField] private TMP_Text _playerLevelArcadeText;
		[SerializeField] private TMP_Text _playerLevelMatchPanelText;
		[SerializeField] private TMP_Text _scoreCountText;
		[SerializeField] private TMP_Text _bestScoreCountText;
		[Header("Stat Values (Arcade)")]
		[SerializeField] private TMP_Text _playerExpText;

		[SerializeField] private TMP_Text _playerHealthText;
		[SerializeField] private TMP_Text _playerAttackText;

		[SerializeField] private TMP_Text _firstEnemyHealthText;
		[SerializeField] private TMP_Text _firstEnemyAttackText;
		[SerializeField] private TMP_Text _secondEnemyHealthText;
		[SerializeField] private TMP_Text _secondEnemyAttackText;
		[SerializeField] private TMP_Text _thirdEnemyHealthText;
		[SerializeField] private TMP_Text _thirdEnemyAttackText;

		[SerializeField] private TMP_Text _inMatchEnemyHealthText;
		[SerializeField] private TMP_Text _inMatchEnemyAttackText;
		[SerializeField] private TMP_Text _inMatchPlayerHealthText;
		[SerializeField] private TMP_Text _inMatchPlayerAttackText;

		[Header("Toggle Sprites")]
		[SerializeField] private Sprite _toggleOff;
		[SerializeField] private Sprite _toggleOn;

		private Coroutine _sceneLoad;

		private bool _isMatchPanelActive = false;
		private bool _isOptionsPanelActive = false;
		private bool _isPausePanelActive = false;
		private bool _isBoostPanelActive = false;

		private void Awake()
		{
			_menuButton.onClick.AddListener(OnOptionsButtonClick);
			_ArcadeGameButton.onClick.AddListener(OnArcadeButtonClick);
			_nextButton.onClick.AddListener(OnNextButtonClick);
			_firstEnemyButton.onClick.AddListener(() => CheckToggleGroup(0));
			_secondEnemyButton.onClick.AddListener(() => CheckToggleGroup(1));
			_thirdEnemyButton.onClick.AddListener(() => CheckToggleGroup(2));
			_reRollButton.onClick.AddListener(OnReRollButtonClick);
			_boostButton.onClick.AddListener(OnBoostButtonClick);
			_closeMatchPanelButton.onClick.AddListener(OnCloseButtonClick);
			_closeOptionsPanelButton.onClick.AddListener(OnCloseButtonClick);
			_startBattleButton.onClick.AddListener(OnBattleButtonClick);
			_boostContinueButton.onClick.AddListener(OnCloseButtonClick);

			DontDestroyOnLoad(this);
		}

		private void OnCloseButtonClick()
		{
			_isMatchPanelActive = true;
			_isOptionsPanelActive = true;
			_isPausePanelActive = true;
			_isBoostPanelActive = true;
			
			OnOptionsButtonClick();
			OnNextButtonClick();
			OnBoostButtonClick();
		}

		private void OnOptionsButtonClick()
		{
			_isOptionsPanelActive = !_isOptionsPanelActive;
			Debug.Log("Options Active = " + _isOptionsPanelActive);
			_optionPanel.SetActive(_isOptionsPanelActive);
		}
		
		private void OnArcadeButtonClick()
		{
			//todo - add functional to switch scenes
			Debug.Log("Arcade button click!");
			_sceneLoad = StartCoroutine(OnSceneLoadRoutine(arcadeScene));
		}

		private void OnNextButtonClick()
		{
			_isMatchPanelActive = !_isMatchPanelActive;
			Debug.Log("Match panel Active = " + _isMatchPanelActive);
			_matchPanel.SetActive(_isMatchPanelActive);
		}

		private void OnReRollButtonClick()
		{
			Debug.Log("ReRollButton Clicked!");
		}

		private void OnBoostButtonClick()
		{
			_isBoostPanelActive = !_isBoostPanelActive;
			Debug.Log("Boost panel Active = " + _isBoostPanelActive);
			_boostPanel.SetActive(_isBoostPanelActive);
		}

		private void OnBattleButtonClick()
		{
			Debug.Log("BattleButton Clicked!");
			//todo - Scene switch to GameLoopScene
			StartCoroutine(OnSceneLoadRoutine(battleScene));

		}

		private void SwitchToArcadeUI()
		{
			Debug.Log("Scene loaded = " + arcadeScene);
			_initialPanel.SetActive(false);
			_arcadePanel.SetActive(true);
		}

		private void SwitchToBattleUI()
		{
			Debug.Log("Scene loaded = " + battleScene);
			_arcadePanel.SetActive(false);
			_walletPanel.SetActive(false);
		}

		private IEnumerator OnSceneLoadRoutine(string sceneName)
		{
			//извращение, как оно есть... :D
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

			SwitchScene(sceneName);

			while (panel.color.a > 0f)
			{
				var newColor = panel.color;
				newColor.a -= changeRate;
				panel.color = newColor;
				yield return new WaitForEndOfFrame();
			}

			panel.raycastTarget = false;

			StopCoroutine(_sceneLoad);
			_curtain.SetActive(false);
			if(SceneManager.GetActiveScene().name == battleScene)
				Destroy(this.gameObject);
		}

		private void SwitchScene(string sceneName)
		{
			SceneManager.LoadScene(sceneName);

			switch (SceneManager.GetActiveScene().name)
			{
				case initialScene:
					SwitchToArcadeUI();
					break;
				case arcadeScene:
					SwitchToBattleUI();
					break;
				default:
					Debug.Log("No scene available");
					break;
			}
		}

		private void CheckToggleGroup(int number)
		{
			foreach (var enemy in _enemyToggle)
			{
				enemy.isOn = false;
				enemy.GetComponent<Image>().sprite = _toggleOff;
			}

			_enemyToggle[number].isOn = true;
			_enemyToggle[number].GetComponent<Image>().sprite = _toggleOn;
			Debug.Log("Enemy " + number + " is Selected!");
		}

	}
}