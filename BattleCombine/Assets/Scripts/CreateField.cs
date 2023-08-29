using UnityEngine;

namespace Scripts
{
    public class CreateField : MonoBehaviour
    {
        [SerializeField] private GameObject tile;
        [SerializeField] private Transform field;
        [SerializeField, Range(4, 21)] private int fieldSize;

        private void Start()
        {
            Debug.Log("Start");
            Create();
        }

        private void Create()
        {
            var newTile = new FieldCreateFactory(tile);
            Debug.Log(newTile);
            
            for (var i = 0; i < fieldSize; i++)
            {
                Debug.Log("Create " + i);
                var gameObject = newTile.Create(field);
            }
        }
    }
}