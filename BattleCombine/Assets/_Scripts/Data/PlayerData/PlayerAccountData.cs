public class PlayerAccountData
{
    private string playerID;
    private string playerName;
    private int exp;
    private int gold;
    private int diamond;
    private int currentScore;
    private int maxScore;
    private int playerAvatarID;

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

    public int Diamond
    {
        get => diamond;
        set => diamond = value;
    }

    public int PlayerAvatarID
    {
        get => playerAvatarID;
        set => playerAvatarID = value;
    }
}