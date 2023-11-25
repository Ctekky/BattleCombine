using BattleCombine.Data;

namespace BattleCombine.Interfaces
{
    public interface ISaveLoad
    {
        void LoadData(GameDataNew gameDataNew);
        void SaveData(ref GameDataNew gameDataNew);
    }

}
