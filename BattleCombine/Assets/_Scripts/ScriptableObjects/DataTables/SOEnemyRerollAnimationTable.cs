using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New RerollAnimationTable", menuName = "Animations table")]
    public class SOEnemyRerollAnimationTable : ScriptableObject
    {
        public List<string> rerollAnimationName;
    }
    
}
