namespace BattleCombine.Data
{
    public class GameData
    {
        public string playerID;
        public string playerName;
        public int exp;
        public int gold;
        public int score;
        public SerializableDictionary<string, float> audioVolume;

        public GameData()
        {
            playerID = "testPlayerID";
            playerName = "Kachachar";
            exp = 0;
            gold = 0;
            score = 0;
            audioVolume = new SerializableDictionary<string, float>();
        }
    }
}