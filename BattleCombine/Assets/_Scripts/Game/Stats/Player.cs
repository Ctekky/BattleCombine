using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class Player : Character
    {
        [SerializeField] private PlayerUI playerUIScript;
        [SerializeField] private string playerName;
        public string GetPlayerName => playerName;

        protected override void Start()
        {
            base.Start();
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString());
        }

        public void UpdateStats()
        {
            playerUIScript.SetUpAllStats(AttackValue.ToString(), HealthValue.ToString());
        }
    }
}