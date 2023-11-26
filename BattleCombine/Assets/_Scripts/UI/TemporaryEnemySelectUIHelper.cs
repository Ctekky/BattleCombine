using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class TemporaryEnemySelectUIHelper : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string battleScene = "ArcadeGameLoop";

		[Header("Buttons")]
		[SerializeField] private Button _menuButton;
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
		[SerializeField] private GameObject _walletPanel;

		[Header("Hero Avatars")]
		[SerializeField] private Image _playerAvatar;
		[SerializeField] private Image _enemyFirstAvatar;
		[SerializeField] private Image _enemySecondAvatar;
		[SerializeField] private Image _enemyThirdAvatar;
		[SerializeField] private Image _playerAvatarMatchPanel;
		[SerializeField] private Image _enemyChosenAvatar;
		[Header("Text panels")]
		[SerializeField] private TMP_Text _coinCountText;
		[SerializeField] private TMP_Text _diamondCountText;
		[SerializeField] private TMP_Text _reRollPriceText;
		[SerializeField] private TMP_Text _playerLevelArcadeText;
		[SerializeField] private TMP_Text _playerLevelMatchPanelText;
		[SerializeField] private TMP_Text _scoreCountText;
		[SerializeField] private TMP_Text _bestScoreCountText;
		[Header("Stat Values")]
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

		[Header("Sliders")]
		[SerializeField] private Slider _sfxVolumeSlider;
		[SerializeField] private Slider _musicVolumeSlider;
		[SerializeField] private Slider _playerExpSlider;

		private Coroutine _sceneLoad;
		private Curtain _curtain;

		private bool _isMatchPanelActive = false;
		private bool _isOptionsPanelActive = false;
		private bool _isPausePanelActive = false;
		private bool _isBoostPanelActive = false;

		private void Awake()
		{
			_menuButton.onClick.AddListener(OnOptionsButtonClick);
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

			_curtain = FindObjectOfType<Curtain>();
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
			_curtain.MoveToAnotherScene(battleScene);
		}

		private void CheckToggleGroup(int number)
		{
			Debug.Log("Enemy " + number + " is Selected!");
		}
	}
}