using System;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using UnityEngine;


public class NextTurnButton : MonoBehaviour, ITouchable
{
    [SerializeField] private bool isTouchable;
    [SerializeField] private SpriteRenderer spriteColor;
    [SerializeField] private TileStack tileStack;

    public Action onButtonPressed;

    public bool IsTouchable
    {
        get => isTouchable;
        set => isTouchable = value;
    }

    private void Start()
    {
        spriteColor.color = Color.red;
        tileStack = FindObjectOfType<TileStack>();
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
        tileStack.ConfirmTiles();
        onButtonPressed?.Invoke();
        isTouchable = false;
        Debug.Log("Button pressed");
    }
}