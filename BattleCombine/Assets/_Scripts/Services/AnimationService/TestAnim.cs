using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TestAnim : MonoBehaviour
{
    private Animator animator;
    private AnimationService animationService;
    [Inject]
    public void GetAnimationService(AnimationService animationService)
    {

        this.animationService = animationService;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        animationService.PlayAnim(animationService.AnimationNames.Name, 1, animator);
    }
}
