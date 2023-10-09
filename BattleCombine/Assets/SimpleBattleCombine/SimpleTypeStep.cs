using BattleCombine.Gameplay;


public class SimpleTypeStep : Step
{
    int countPlayer;
    int currentStep;

    private void Awake()
    {
        countPlayer = FindObjectsOfType<Player>().Length;
        currentStep = countPlayer;
    }
   
    public override bool MoveIsPassed()
    {
       // currentStep--;
      //  if (currentStep > 0) return false;
        return true;
    }
        
     
 
}
