using System;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class AnimationTriggerToEventRelay : MonoBehaviour
    {
        
        [SerializeField] private Animator _animator;
        
        public event Action<string> onRerollTrigger;
        public event Action onSpawnField;

        public void OnRerollAnimationTrigger(string animName)
        {
            onRerollTrigger?.Invoke(animName);
        }
        public void EndAnimationBoolTrigger(string animName)
        {
            _animator.SetBool(animName, false);
        }

        public void OnSpawnFieldAnimationTrigger()
        {
            onSpawnField?.Invoke();
        }
    }
}