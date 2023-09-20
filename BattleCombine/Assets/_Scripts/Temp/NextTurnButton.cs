using System;
using System.Collections.Generic;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using UnityEngine;


public class NextTurnButton : MonoBehaviour, ITouchable
{
    [SerializeField] private bool isTouchable;
    [SerializeField] private SpriteRenderer spriteColor;
    [SerializeField] private TileStack tileStack;
    public Action onButtonPressed;

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
        onButtonPressed?.Invoke();
        Debug.Log("Button pressed");

        switch (tileStack.IDPlayer)
        {
            case IDPlayer.Player1:
                ConfirmSelectedTiles(tileStack.TilesStackPlayer1, tileStack.NextMoveTiles);
                break;
            case IDPlayer.Player2:
                ConfirmSelectedTiles(tileStack.TilesStackPlayer2, tileStack.NextMoveTiles);
                break;
        }

        isTouchable = false;
    }

    public void ConfirmSelectedTiles(Stack<GameObject> stack, List<GameObject> list) //Confirm tile for add to characteristics
    {
        foreach(GameObject tileGameObjectStack in stack)
        {
            Tile tile = tileGameObjectStack.GetComponent<Tile>();
            tile.StateMachine.ChangeState(tile.FinalChoiceState);
        }

        foreach (GameObject tileGameObjectList in list)
        {
            Tile tile = tileGameObjectList.GetComponent<Tile>();
            tile.StateMachine.ChangeState(tile.EnabledState);
        }

        tileStack.NextMoveTiles.Clear();

        PassingTheTurnToTheNextPayer();
    }

    public void PassingTheTurnToTheNextPayer()
    {
        if (tileStack.IDPlayer == IDPlayer.Player1)
        {
            tileStack.IDPlayer = IDPlayer.Player2;
            Debug.Log(tileStack.IDPlayer.ToString() +" moves");
        }
        else if (tileStack.IDPlayer == IDPlayer.Player2)
        {
            tileStack.IDPlayer = IDPlayer.Player1;
            Debug.Log(tileStack.IDPlayer.ToString() + " moves");
        }
        else
        {
            Debug.Log("Player is not defined");
        }
    }
}
