using System.Collections.Generic;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New TileType", menuName = "TileType")]
    public class TileType : ScriptableObject
    {
        public new string name;
        public CellType cellType;
        public Sprite spriteUp;
        public Sprite spriteDown;
        public List<TileModifierDictionary> modifierChances;
    }
}