using System;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class AnimationTriggerToEventRelay : MonoBehaviour
    {
        public Action<string> onRerollTrigger;

        public void OnRerollAnimationTrigger(string animName)
        {
            onRerollTrigger?.Invoke(animName);
        }
        
    }
}