using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class FieldScaler
    {
        public void ScaleMainField(GameObject mainField, float edgeOffset)
        {
            //test scaler (not complete)
            var mainCamera = Camera.main;
            var halfHeight = mainCamera.orthographic ? mainCamera.orthographicSize : 0;
            var halfWidth = mainCamera.aspect * halfHeight;

            var objectWidth = mainField.GetComponent<Renderer>().bounds.size.x;
            var objectHeight = mainField.GetComponent<Renderer>().bounds.size.y;

            var scaleX = (halfWidth * 1.7f - edgeOffset * 1.7f) / objectWidth;
            var scaleZ = (halfHeight * 1f - edgeOffset * 1f) / objectHeight;

            mainField.transform.localScale = new Vector3(scaleX, 1, scaleZ);
        }
    }
}