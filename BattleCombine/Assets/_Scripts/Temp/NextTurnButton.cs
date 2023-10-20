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
    [SerializeField] private List<GameObject> tilesForNextMovePlayer1;
    [SerializeField] private List<GameObject> tilesForNextMovePlayer2;

    public Action onButtonPressed;

    public bool IsTouchable
    {
        get => isTouchable;
        set => isTouchable = value;
    }
    public List<GameObject> GetTilesForNextMovePlayer1 { get => tilesForNextMovePlayer1; }
    public List<GameObject> GetTilesForNextMovePlayer2 { get => tilesForNextMovePlayer2; }

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
        
        switch (tileStack.IDPlayer)
        {
            case IDPlayer.Player1:
                ConfirmSelectedTiles(tileStack.TilesStackPlayer1, tileStack.TilesListPlayer1, tileStack.NextMoveTiles, tilesForNextMovePlayer1, tilesForNextMovePlayer2);
                SetFlagOnTilePlayer2(tilesForNextMovePlayer2);
                tileStack.NextMoveTiles.AddRange(tilesForNextMovePlayer2);
                break;
            case IDPlayer.Player2:
                ConfirmSelectedTiles(tileStack.TilesStackPlayer2, tileStack.TilesListPlayer2, tileStack.NextMoveTiles, tilesForNextMovePlayer2, tilesForNextMovePlayer1);
                SetFlagOnTilePlayer1(tilesForNextMovePlayer1);
                tileStack.NextMoveTiles.AddRange(tilesForNextMovePlayer1);
                break;
        }
        isTouchable = false;
        
        Debug.Log("Button pressed");
    }
    public void ConfirmSelectedTiles(Stack<GameObject> stackChosenTile, List<GameObject> listChosenTile, List<GameObject> list, List<GameObject> tileNextMove, List<GameObject> listNextMoveOpponent) //Confirm tile for add to characteristics
    {
        foreach (GameObject tileGameObjectStack in stackChosenTile) //touch
        {
            Tile tile = tileGameObjectStack.GetComponent<Tile>();
            tile.StateMachine.ChangeState(tile.FinalChoiceState);
        }

        foreach (GameObject tileGameObjectStack in listChosenTile) //move
        {
            Tile tile = tileGameObjectStack.GetComponent<Tile>();
            tile.StateMachine.ChangeState(tile.FinalChoiceState);
        }

        foreach (GameObject tileGameObjectList in list)
        {
            Tile tile = tileGameObjectList.GetComponent<Tile>();
            tile.StateMachine.ChangeState(tile.EnabledState);
        }
        
        foreach(GameObject tileGameObjectListOpponent in listNextMoveOpponent)
        {
            Tile tile = tileGameObjectListOpponent.GetComponent<Tile>();
            if (tile.GetTileState == TileState.DisabledState)
            {
                continue;
            }
            else
            {
                tile.StateMachine.ChangeState(tile.AvailableForSelectionState);
            }
        }

        onButtonPressed?.Invoke();
        stackChosenTile.Clear();
        listChosenTile.Clear();
        tileNextMove.Clear();
        tileNextMove.AddRange(tileStack.NextMoveTiles);
        tileStack.NextMoveTiles.Clear();

        PassingTheTurnToTheNextPlayer();
    }

    public void PassingTheTurnToTheNextPlayer()
    {
        if (tileStack.IDPlayer == IDPlayer.Player1)
        {
            tileStack.IDPlayer = IDPlayer.Player2;
            Debug.Log(tileStack.IDPlayer.ToString() + " moves");
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
    public void SetFlagOnTilePlayer1(List<GameObject> tileList)
    {

        foreach(GameObject tileGameObject in tileList)
        {
            Tile tile = tileGameObject.GetComponent<Tile>();
            tile.SetAlignTileToPlayer1(flag: true);
        }
    }
    public void SetFlagOnTilePlayer2(List<GameObject> tileList)
    {

        foreach (GameObject tileGameObject in tileList)
        {
            Tile tile = tileGameObject.GetComponent<Tile>();
            tile.SetAlignTileToPlayer2(flag: true);
        }
    }
}