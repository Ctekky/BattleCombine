using BattleCombine.Data;

namespace BattleCombine.Interfaces
{
    public interface ISaveLoad
    {
        void LoadData(GameData gameData);
        void SaveData(ref GameData gameData);
    }

}
