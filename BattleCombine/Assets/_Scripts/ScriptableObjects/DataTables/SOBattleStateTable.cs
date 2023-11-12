using System.Collections.Generic;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New BattleTable", menuName = "BattleTable")]
    public class BattleDataTable : ScriptableObject
    {
        public new string name;
        public CellType cellType;
        public Sprite spriteUp;
        public Sprite spriteDown;
        public List<TileModifierDictionary> modifierChances;
    }
}
