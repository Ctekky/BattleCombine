using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class TileStack : MonoBehaviour
    {
        [SerializeField] private int speedPlayer;
        [SerializeField] private List<GameObject> nextMoveTiles;

        private Stack<GameObject> _tileStack;

        public int SpeedPlayer { get => speedPlayer; }
        public Stack<GameObject> TilesStack
        {
            get => _tileStack;
            set => value = _tileStack;
        }
        public List<GameObject> NextMoveTiles
        {
            get => nextMoveTiles;
            set => value = nextMoveTiles;
        }

        private void Start()
        {
            _tileStack = new Stack<GameObject>();
            nextMoveTiles = new List<GameObject>();
            //speedPlayer = 4; //TODO: get player speed
        }

        private void OnApplicationQuit()
        {
            _tileStack.Clear();
            nextMoveTiles.Clear();
        }
    }
}

