using System.Collections.Generic;
using BattleCombine.Data;
using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New BattleTable", menuName = "Battle table")]
    public class SOBattleDataTable : ScriptableObject
    {
        public new string name;
        public CellType cellType;
        public Sprite spriteUp;
        public Sprite spriteDown;
        public List<TileModifierDictionary> modifierChances;
    }
}