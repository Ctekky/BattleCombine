using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class TemporaryInitialUIHelper : MonoBehaviour
	{
		private const string initialScene = "Initial";
		private const string arcadeScene = "EnemySelectionScene";
		private const string battleScene = "ArcadeGameLoop";

		[SerializeField] private Curtain _curtain;

		[Header("Buttons")]
		[SerializeField] private Button _menuButton;
		[SerializeField] private Button _ArcadeGameButton;
		[SerializeField] private Button _closeOptionsPanelButton;

		[Header("Game Panels")]
		[SerializeField] private GameObject _optionPanel;

		private bool _isOptionsPanelActive = false;

		private void Awake()
		{
			_menuButton.onClick.AddListener(OnOptionsButtonClick);
			_ArcadeGameButton.onClick.AddListener(OnArcadeButtonClick);
			_closeOptionsPanelButton.onClick.AddListener(OnCloseButtonClick);
		}

		private void OnCloseButtonClick()
		{
			_isOptionsPanelActive = true;
			
			OnOptionsButtonClick();
		}

		private void OnOptionsButtonClick()
		{
			_isOptionsPanelActive = !_isOptionsPanelActive;
			Debug.Log("Options Active = " + _isOptionsPanelActive);
			_optionPanel.SetActive(_isOptionsPanelActive);
		}
		
		private void OnArcadeButtonClick()
		{
			Debug.Log("Arcade button click!");
			_curtain.MoveToAnotherScene(arcadeScene);
		}

	}
}