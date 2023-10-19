using BattleCombine.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceMoves 
{
    private Player currentPlayer;
    private Player nextPlayer;

    public Player CurrentPlayer => currentPlayer;
    public Player NextPlayer => nextPlayer;

    public SequenceMoves(Player currentPlayer, Player nextPlayer)
    {
        this.currentPlayer = currentPlayer;
        this.nextPlayer = nextPlayer;
    }
    public void Next()
    {
        Player tmp = currentPlayer;
        currentPlayer = nextPlayer;
        nextPlayer = tmp;
    }
}
