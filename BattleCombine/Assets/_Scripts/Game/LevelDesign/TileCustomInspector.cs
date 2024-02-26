using BattleCombine.Gameplay;
using UnityEditor;
using UnityEngine;

namespace BattleCombine.CustomInspectors
{
    [CustomEditor(typeof(Tile))]
    public class TileCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            Tile tileCustomInspector = (Tile)target;
            if (GUILayout.Button("Update tile"))
            {
                tileCustomInspector.UpdateTileInInspector();
            }
        }
    }
}