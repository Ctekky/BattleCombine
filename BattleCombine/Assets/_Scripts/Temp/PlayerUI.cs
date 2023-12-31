using System.Collections.Generic;
using BattleCombine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Temp
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private TMP_Text playerLevelText;
        [SerializeField] private Slider playerLevelSlider;
        [SerializeField] private Image shieldSprite;
        [SerializeField] private GameObject avatarGameObject;
        [SerializeField] private GameObject speedArea;
        [SerializeField] private GameObject speedPrefab;
        [SerializeField] private List<GameObject> createdSpeedObjectList;

        [Header("BoostTimer")]
        [SerializeField] private Image _heartImg;
        [SerializeField] private Image _swordsImg;
        [SerializeField] private Image _heartsBackImg;
        [SerializeField] private Image _swordsBackImg;
        [SerializeField] private List<Sprite> _cooldownSprites;
        [SerializeField] private GameObject _healthCooldownObj;
        [SerializeField] private GameObject _attackCooldownObj;
        [SerializeField] private TMP_Text _healthTimerText;
        [SerializeField] private TMP_Text _attackTimerText;

        private bool _isShielded;
        private bool _isPlayerBoostCooldownOn;

        public void UpdateLevelSlider(float value)
        {
            playerLevelSlider.value = value;
        }

        public void UpdatePlayerLevel(int value)
        {
            playerLevelText.text = value.ToString();
        }
        
        public void ChangeImageInCooldown(int value)
        {
            _isPlayerBoostCooldownOn = !_isPlayerBoostCooldownOn;
            
            _healthCooldownObj.SetActive(_isPlayerBoostCooldownOn);
            _attackCooldownObj.SetActive(_isPlayerBoostCooldownOn);
            
            if(_isPlayerBoostCooldownOn)
            {
                _heartImg.sprite = _cooldownSprites[1];
                _heartsBackImg.sprite = _cooldownSprites[5];
                _healthTimerText.text = value + "h";

                _swordsImg.sprite = _cooldownSprites[3];
                _swordsBackImg.sprite = _cooldownSprites[5];
                _attackTimerText.text = value + "h";
            }
            else
            {
                _heartImg.sprite = _cooldownSprites[0];
                _swordsImg.sprite = _cooldownSprites[2];
                _heartsBackImg.sprite = _cooldownSprites[4];
                _swordsBackImg.sprite = _cooldownSprites[4];
            }
        }

        private void SetupStat(TMP_Text text, string value)
        {
            text.text = value;
        }

        public void SetUpAllStats(string attackValue, string healthValue, bool isShielded)
        {
            SetupStat(attackText, attackValue);
            SetupStat(healthText, healthValue);
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

        public void SetupAvatar(Sprite enableState, Sprite disableState)
        {
            var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
            changeSpriteScript.SetupSprites(enableState, disableState);
        }

        public Sprite GetSprite(bool state)
        {
            var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
            var sprite = state ? changeSpriteScript.GetTrueStateSprite : changeSpriteScript.GetFalseStateSprite;
            return sprite;
        }

        public void ChangeAvatarState(bool state)
        {
            var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
            if (state)
                changeSpriteScript.EnableState();
            else
                changeSpriteScript.DisableState();
        }

        public void SetupSpeed(int speed)
        {
            DeleteAllSpeedObject();
            for (var i = 0; i < speed; i++)
            {
                var speedObject = Instantiate(speedPrefab, speedArea.transform);
                createdSpeedObjectList.Add(speedObject);
            }
        }

        private void DeleteAllSpeedObject()
        {
            foreach (var speedObject in createdSpeedObjectList)
            {
                Destroy(speedObject);
            }

            createdSpeedObjectList.Clear();
        }

        private void OnDisable()
        {
            DeleteAllSpeedObject();
        }
    }
}