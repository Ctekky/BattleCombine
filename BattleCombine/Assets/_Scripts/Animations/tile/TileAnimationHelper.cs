using System;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class TileAnimationHelper : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string animationName;
        
        public Action onTileAnimationTrigger;
        
        public void TileAnimationTriggerEvent()
        {
            onTileAnimationTrigger?.Invoke();
        }

        public void SetAnimationBool(bool flag)
        {
            animator.SetBool(animationName, flag);
        }
    }
}