using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class SettingsPanel : MonoBehaviour
    {
        [SerializeField] private Button _close;
        [SerializeField] private Button _connectFacebook;
        [SerializeField] private Button _connectGoogle;

        public Button GetSettingsCloseButton => _close;

        private void Start()
        {
            _connectFacebook.onClick.AddListener(ConnectToFacebook);
            _connectGoogle.onClick.AddListener(ConnectToGoogle);
        }

        private void ConnectToFacebook()
        {
            Debug.Log(_connectFacebook + " = connect! (test)");
        }

        private void ConnectToGoogle()
        {
            Debug.Log(_connectGoogle + " = connect! (test)");
        }
    }
}
