using BattleCombine.Services;
using UnityEditor;
using UnityEngine;

namespace BattleCombine.CustomInspectors
{
    [CustomEditor(typeof(LevelDesignSaveField))]
    public class SaveLevelCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            LevelDesignSaveField saveField = (LevelDesignSaveField)target;
            if (GUILayout.Button("Save field file"))
            {
                saveField.SaveFieldData();
            }

            if (GUILayout.Button("Load field file"))
            {
                saveField.LoadFieldData();
            }
        }
    }
}