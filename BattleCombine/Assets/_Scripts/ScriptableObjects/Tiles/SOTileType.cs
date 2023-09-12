using BattleCombine.Enums;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New TileType", menuName = "TileType")]
    public class TileType : ScriptableObject
    {
        public new string name;
        public CellType cellType;
        public Sprite sprite;
    }
}