using System;
using BattleCombine.Interfaces;
using UnityEngine;


public class NextTurnButton : MonoBehaviour, ITouchable
{
    [SerializeField] private bool isTouchable;
    [SerializeField] private SpriteRenderer spriteColor;
    public Action onButtonPressed;

    public bool IsTouchable
    {
        get => isTouchable;
        set => isTouchable = value;
    }

    private void Start()
    {
        spriteColor.color = Color.red;
    }

    private void Update()
    {
        CheckColor();
    }

    private void CheckColor()
    {
        spriteColor.color = isTouchable ? Color.green : Color.red;
    }

    public void Touch()
    {
        if (!isTouchable) return;
        onButtonPressed?.Invoke();
        Debug.Log("Button pressed");
        isTouchable = false;
    }
}