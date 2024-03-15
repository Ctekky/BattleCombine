using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.CharacterChoose
{
    public class SwipeAvatarProvider : MonoBehaviour
    {
        [SerializeField] private List<Sprite> _avatars;
        [SerializeField] private List<Image> _avatarCards;

        private const string AvatarsPath = "Images/UI/Portraits/NewChars";
        
        private List<Transform> avatarTransform = new List<Transform>();

        private void Awake()
        {
            LoadAvatars();
            GetTransforms();
        }

        public void AvatarMoveLeft()
        {
            _avatarCards[1].sprite = _avatars.First();
            Debug.Log("ВЛЕВА");
            
            AvatarListUpdate(true);
        }
        public void AvatarMoveRight()
        {
            _avatarCards[1].sprite = _avatars.Last();
            Debug.Log("ВПРАВА");
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

        private void GetTransforms()
        {
            foreach (var card in _avatarCards)
            {
                avatarTransform.Add(card.gameObject.transform);
            }
        }
    }
}