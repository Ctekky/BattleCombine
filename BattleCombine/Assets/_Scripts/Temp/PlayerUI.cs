using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Temp
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Image shieldSprite;

        private bool _isShielded;

        private void SetUpStat(TMP_Text text, string value)
        {
            text.text = value;
        }

        public void SetUpAllStats(string attackValue, string healthValue, bool isShielded)
        {
            SetUpStat(attackText, attackValue);
            SetUpStat(healthText, healthValue);
            SetShield(isShielded);
            ChangeShieldSprite();
        }

        private void SetShield(bool value)
        {
            _isShielded = value;
            ChangeShieldSprite();
        }

        private void ChangeShieldSprite()
        {
            shieldSprite.enabled = _isShielded;
        }
    }
}