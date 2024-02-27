using System;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class TileTextAnimationHelper : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string animationName;
        
        public Action onTileTextAnimationTrigger;
        public void TileTextAnimationTriggerEvent()
        {
            onTileTextAnimationTrigger?.Invoke();
        }

        public void SetAnimationBool(bool flag)
        {
            animator.SetBool(animationName, flag);
        }
    }
    
}