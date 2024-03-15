using UnityEngine;

namespace BattleCombine.Services
{
    public class AnimationService : MonoBehaviour
    {
        public void PlayAnimByTime(string nameAnim, float timeAnim, Animator animator)
        {
            animator.SetBool(nameAnim, true);
            animator.speed = timeAnim;
        }

        public void PlayAnim(string nameAnim, Animator animator)
        {
            animator.SetBool(nameAnim, true);
        }

        public void StopAnim(string nameAnim, Animator animator)
        {
            animator.SetBool(nameAnim, false);
        }
    }
}