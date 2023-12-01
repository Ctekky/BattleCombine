public class PlayerAccountData
{
    private string playerID;
    private string playerName;
    private int exp;
    private int gold;
    private int currentScore;
    private int maxScore;

    public string PlayerID
    {
        get => playerID;
        set => playerID = value;
    }
    public string PlayerName
    {
        get => playerName;
        set => playerName = value;
    }
    public int Exp
    {
        get => exp;
        set => exp = value;
    }
    public int Gold
    {
        get => gold;
        set => gold = value;
    }
    public int CurrentScore
    {
        get => currentScore;
        set => currentScore = value;
    }
    public int MaxScore
    {
        get => maxScore;
        set => maxScore = value;
    }
    /*public PlayerAccountData(string playerID, string playerName, int exp, int gold, int currentScore, int maxScore)
    {
        PlayerID = playerID;
        PlayerName = playerName;
        Exp = exp;
        Gold = gold;
        CurrentScore = currentScore;
        MaxScore = maxScore;
    }
    public PlayerAccountData()
    {
        PlayerID = "0000";
        PlayerName = "TestName";
        Exp = 200;
        Gold = 100;
        CurrentScore = 0;
        MaxScore = 0;
    }*/
}
