using System;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class TileTextAnimationHelper : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        //TODO: change for one animation db
        [SerializeField] private string animationName;
        
        public Action onAnimationTrigger;
        public void AnimationTriggerEvent()
        {
            onAnimationTrigger?.Invoke();
        }

        public void SetAnimationBool(bool flag)
        {
            animator.SetBool(animationName, flag);
        }
    }
    
}
