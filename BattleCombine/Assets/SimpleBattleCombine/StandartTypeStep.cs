using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartTypeStep : Step
{
    int playerCurrentStep;
    public int PlayerCurrentStep => playerCurrentStep;
    public override bool MoveIsPassed()
    {
        playerCurrentStep++;
        if ((playerCurrentStep % (stepsInTurn * 2)) != 0) return false; 
        return true;
    }
}
