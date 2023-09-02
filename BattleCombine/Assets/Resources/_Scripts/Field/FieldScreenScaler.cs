using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldScreenScaler : MonoBehaviour
{
    [SerializeField] private GameObject field;
    [Header("Scale or not on start")]
    [SerializeField] private bool makeScale;
    
    private void Start()
    {
        //test scaler (not complete)
        if(!makeScale) return;
        
        var padding = .5f;

        var mainCamera = Camera.main;
        var halfHeight = mainCamera.orthographic ? mainCamera.orthographicSize : 0;
        var halfWidth = mainCamera.aspect * halfHeight;

        var objectWidth = field.GetComponent<Renderer>().bounds.size.x;
        var objectHeight = field.GetComponent<Renderer>().bounds.size.y;

        var scaleX = (halfWidth * 2f - padding * 2) / objectWidth;
        var scaleY = (halfHeight * 2f - padding * 2) / objectHeight;

        field.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}