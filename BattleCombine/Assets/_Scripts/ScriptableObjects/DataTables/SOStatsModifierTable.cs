using System.Collections.Generic;
using BattleCombine.Data;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Stats modifier table", menuName = "Stats modifier table")]
    public class SOStatsModifierTable : ScriptableObject
    {
        public float healthBaseModifier;
        public float healthMultiplier;
        public float attackBaseModifier;
        public float attackMultiplier;
        public List<SpeedStatModifierStruct> speedStatModifier;
        
    }
}