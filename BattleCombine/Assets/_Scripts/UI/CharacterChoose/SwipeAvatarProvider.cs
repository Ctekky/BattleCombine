using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.CharacterChoose
{
    public class SwipeAvatarProvider : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _avatars;
        [SerializeField] private List<Image> _avatarCards;
        [SerializeField] private TMP_Text _playerName;

        private const string AvatarsPath = "Images/UI/Portraits/NewChars";
        
        private void Awake()
        {
            LoadAvatars();
        }

        //todo - Continue button link needed
        //or Player Account Set Avatar
        public Sprite GetChosenAvatar() =>
            _avatars.First();
        public string GetPlayerName() =>
            _playerName.text;

        public void AvatarMoveLeft()
        {
            _avatarCards[1].sprite = _avatars.First();
            
            AvatarListUpdate(true);
        }
        public void AvatarMoveRight()
        {
            _avatarCards[1].sprite = _avatars.Last();
            
            AvatarListUpdate(false);
        }

        private void AvatarListUpdate(bool swipeLeft)
        {
            if(swipeLeft)
            {
                var temp = _avatars.First();
                
                _avatars.RemoveAt(0);
                _avatars.Add(temp);
            }
            else
            {
                var temp = _avatars.Last();
                
                _avatars.RemoveAt(_avatars.Count -1);
                _avatars.Insert(0, temp);
            }
            
            AssignAvatars();
        }
        
        private void LoadAvatars()
        {
            var sprites = Resources.LoadAll<Sprite>(AvatarsPath);

            foreach (var sprite in sprites)
            {
                _avatars.Add(sprite);
            }

            AssignAvatars();
        }

        private void AssignAvatars()
        {
            _avatarCards[0].sprite = _avatars.Last();
            _avatarCards[1].sprite = _avatars.First();
            _avatarCards[2].sprite = _avatars[1];
        }
    }
}