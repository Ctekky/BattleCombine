using BattleCombine.Gameplay;
using UnityEditor;
using UnityEngine;

namespace BattleCombine.CustomInspectors
{
    [CustomEditor(typeof(LevelDesignCreateField))]
    public class LevelDesignCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelDesignCreateField levelDesign = (LevelDesignCreateField)target;
            if (GUILayout.Button("Generate field"))
            {
                levelDesign.GenerateDefaultField();
            }
            if(GUILayout.Button("Delete field"))
            {
                levelDesign.DeleteField();
            }
        }
        
    }
}
