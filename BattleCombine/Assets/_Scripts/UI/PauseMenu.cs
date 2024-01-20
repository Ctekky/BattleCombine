using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class PauseMenu : MonoBehaviour
	{
		[SerializeField] private Button _menuButton;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _continueButton;
		[SerializeField] private Button _leaveGameButton;

		public Button GetCloseButton => _closeButton;
		public Button GetMenuButton => _menuButton;

		public Button GetPauseContinueButton => _continueButton;

		public Action OnLeaveGameButtonPress;

		private void Start()
		{
			_leaveGameButton.onClick.AddListener(LeaveGame);
		}

		private void LeaveGame()
		{
			OnLeaveGameButtonPress?.Invoke();
		}
	}
}