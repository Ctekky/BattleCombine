using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class PausePanel : MonoBehaviour
	{
		[SerializeField] private Button _menuButton;
		[SerializeField] private Button _closeButton;
		[SerializeField] private Button _continueButton;
		[SerializeField] private Button _leaveGameButton;

		public Button GetCloseButton => _closeButton;
		public Button GetMenuButton => _menuButton;

		public Button GetPauseContinueButton => _continueButton;

		private void Start()
		{
			_leaveGameButton.onClick.AddListener(LeaveGame);
		}

		private void LeaveGame()
		{
			Debug.Log("Game Exit");
			Application.Quit();
#if UNITY_EDITOR
			EditorApplication.isPlaying = false;
#endif
		}
	}
}