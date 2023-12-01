using BattleCombine.Data;

namespace BattleCombine.Interfaces
{
    public interface ISaveLoad
    {
        void LoadData(GameData gameData, bool newGameBattle, bool firstStart);
        void SaveData(ref GameData gameData, bool newGameBattle, bool firstStart);
    }

}
