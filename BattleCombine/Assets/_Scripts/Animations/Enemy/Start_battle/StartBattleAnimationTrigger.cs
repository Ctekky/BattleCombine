using BattleCombine.Gameplay;
using BattleCombine.UI;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class StartBattleAnimationTrigger : MonoBehaviour
    {
        [SerializeField] private UISpriteImageStateChange spriteChangeHelper;
        [SerializeField] private Player player;

        public void ChangeAvatarSprite()
        {
            player.GetUi().DisableAnimator();
            player.ReloadAvatar();
        }
    }
    
}
