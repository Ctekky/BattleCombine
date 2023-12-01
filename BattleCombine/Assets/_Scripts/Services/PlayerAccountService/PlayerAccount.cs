namespace BattleCombine.Data
{
    public class PlayerAccount
    {
        public string PlayerID;
        public string PlayerName;
        public int Exp;
        public int Gold;
        public int CurrentScore;
        public int MaxScore;

        public PlayerAccount(string playerID, string playerName, int exp, int gold, int currentScore, int maxScore)
        {
            PlayerID = playerID;
            PlayerName = playerName;
            Exp = exp;
            Gold = gold;
            CurrentScore = currentScore;
            MaxScore = maxScore;
        }

        public PlayerAccount()
        {
            PlayerID = "0000";
            PlayerName = "TestName";
            Exp = 0;
            Gold = 0;
            CurrentScore = 0;
            MaxScore = 0;
        }
    }
    
}
