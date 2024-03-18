using System;
using UnityEngine;

public class SpeedBallAnimationHelper : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string animationName;

    public void SetAnimationBool(bool flag)
    {
        animator.SetBool(animationName, flag);
    }
}
