using UnityEngine;
using UnityEngine.UI;

namespace BattleCombine.UI
{
    public class UISpriteImageStateChange : MonoBehaviour
    {
        [SerializeField] private Sprite spriteStateTrue;
        [SerializeField] private Sprite spriteStateFalse;
        [SerializeField] private Image image;

        public Sprite GetTrueStateSprite => spriteStateTrue;
        public Sprite GetFalseStateSprite => spriteStateFalse;
        public void SetupSprites(Sprite spriteTrue, Sprite spriteFalse)
        {
            spriteStateTrue = spriteTrue;
            spriteStateFalse = spriteFalse;
            EnableState();
        }

        private void Start()
        {
            image.sprite = spriteStateTrue;
        }

        public void EnableState()
        {
            image.sprite = spriteStateTrue;
        }

        public void DisableState()
        {
            image.sprite = spriteStateFalse;
        }
    }
}