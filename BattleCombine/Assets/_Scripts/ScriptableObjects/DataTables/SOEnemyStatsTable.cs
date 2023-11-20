using System.Collections.Generic;
using BattleCombine.Data;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New EnemyStatsTable", menuName = "Stats table")]
    public class SOEnemyStatsTable : ScriptableObject
    {
        public List<EnemyStatsStruct> enemyStatsStruct;
    }
    
}
