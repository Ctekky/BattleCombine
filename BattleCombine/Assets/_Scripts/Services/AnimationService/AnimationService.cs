using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimationService
{
    [SerializeField] private Image img;
    [SerializeField] private float time = 0.1f;
    public AnimationNames AnimationNames { get; }

    public AnimationService()
    {
        AnimationNames = new AnimationNames();
    }

    public void PlayAnim(string nameAnim, float timeAnim, Animator animator)
    {
        animator.SetBool(nameAnim, true);
        animator.speed = timeAnim;
        //AnimationClip animations = new AnimationClip();

    }

    
    private IEnumerator FadeIn()
    {
        float i = 0;
        while (i <= 0)
        {
            img.color = new Color(0.0f, 0.0f, 0.0f, i);
            i += 0.2f;
            yield return new WaitForSeconds(time / 5);
        }

    }
    private IEnumerator FadeOut()
    {
        float i = 1;
        while (i >= 0)
        {
            img.color = new Color(0.0f, 0.0f, 0.0f, i);
            i -= 0.2f;
            yield return new WaitForSeconds(time / 5);
        }
    }

   
}




