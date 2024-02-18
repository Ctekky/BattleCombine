using System;
using _Scripts.Audio;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public class UIHelperInitialScene : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string battleScene = "ArcadeGameLoop";

		[SerializeField] private Curtain _curtain;

		[Header("Buttons")]
		[SerializeField] private Button _menuButton;
		[SerializeField] private Button _arcadeGameButton;
		[SerializeField] private Button _closeOptionsPanelButton;

		[FormerlySerializedAs("_optionPanel")]
		[Header("Game Panels")]
		[SerializeField] private GameObject _settingsPanel;
		[SerializeField] private SoundHelper _soundHelper;

		private bool isOptionsPanelActive = false;

		[Inject] private PlayerAccount _playerAccount;

		private void Awake()
		{
			_menuButton.onClick.AddListener(OnOptionsButtonClick);
			_arcadeGameButton.onClick.AddListener(OnArcadeButtonClick);
			_closeOptionsPanelButton.onClick.AddListener(OnCloseButtonClick);
		}

		private void OnCloseButtonClick()
		{
			isOptionsPanelActive = true;
			
			OnOptionsButtonClick();
		}

		private void OnOptionsButtonClick()
		{
			isOptionsPanelActive = !isOptionsPanelActive;
			Debug.Log("Options Active = " + isOptionsPanelActive);
			_settingsPanel.SetActive(isOptionsPanelActive);
			
			_soundHelper.PlayClickSound();
		}
		
		private void OnArcadeButtonClick()
		{
			Debug.Log("Arcade button click!");
			_curtain.MoveToAnotherScene(arcadeScene);
		}
	}
}