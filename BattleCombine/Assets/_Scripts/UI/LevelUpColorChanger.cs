using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
    public class LevelUpColorChanger : MonoBehaviour
    {
        [SerializeField] private Image _statImage;

        private void Start()
        {
            _statImage.color = Color.white;
        }

        public void EnableState()
        {
            var color =
                _statImage.color = Color.green;
        }

        public void DisableState()
        {
            _statImage.color = Color.white;
        }
    }
}
