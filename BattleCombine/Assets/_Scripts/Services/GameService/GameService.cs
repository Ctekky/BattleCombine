using BattleCombine.Services.Other;
using UnityEngine;

namespace BattleCombine.Services
{
    public class GameService : MonoBehaviour
    {
        #region ArcadeGameStats

        [SerializeField] private int arcadeCurrentScore;
        [SerializeField] private int arcadeBestScore;

        public int ArcadeCurrentScore
        {
            get => arcadeCurrentScore;
            set => value = arcadeCurrentScore;
        }

        public int ArcadeBestScore
        {
            get => arcadeBestScore;
            set => value = arcadeBestScore;
        }

        #endregion


        [SerializeField] private ColorSettings currentColorSettings;

        public ColorSettings GetCurrentColorSettings()
        {
            return currentColorSettings;
        }
    }
}