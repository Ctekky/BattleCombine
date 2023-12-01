using System;
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
        [SerializeField] private Image shieldSprite;
        [SerializeField] private GameObject avatarGameObject;
        [SerializeField] private GameObject speedArea;
        [SerializeField] private GameObject speedPrefab;
        [SerializeField] private List<GameObject> createdSpeedObjectList;
        private bool _isShielded;
        
        private void OnDisable()
        {
            DeleteAllSpeedObject();
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
            for (int i = 0; i < speed; i++)
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
    }
}