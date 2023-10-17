using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Step : MonoBehaviour
{
    protected int currentStepInTurn;
    protected int stepsInTurn;
    public abstract bool MoveIsPassed();
    public void GetVariables(int currentStepInTurn, int stepsInTurn)
    {
        this.currentStepInTurn = currentStepInTurn;
        this.stepsInTurn = stepsInTurn;
    }
}