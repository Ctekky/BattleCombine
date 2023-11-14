namespace BattleCombine.Data
{
    public class GameData
    {
        public string PlayerID;
        public string PlayerName;
        public int Exp;
        public int Gold;
        public int CurrentScore;
        public int MaxScore;
        public SerializableDictionary<string, float> AudioSettings;

        public GameData()
        {
            PlayerID = "testPlayerID";
            PlayerName = "Kachachar";
            Exp = 0;
            Gold = 0;
            CurrentScore = 0;
            MaxScore = 0;
            AudioSettings = new SerializableDictionary<string, float>();
        }
    }
}