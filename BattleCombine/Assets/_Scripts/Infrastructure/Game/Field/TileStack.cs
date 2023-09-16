using BattleCombine.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Scripts
{
    public class TileStack : MonoBehaviour
    {
        [SerializeField] private int _speedPlayer;
        [SerializeField] private List<GameObject> _nextMoveTiles;

        private Stack<GameObject> _tileStack;

        public int SpeedPlayer { get => _speedPlayer; }
        public Stack<GameObject> TilesStack
        {
            get => _tileStack;
            set => value = _tileStack;
        }
        public List<GameObject> NextMoveTiles
        {
            get => _nextMoveTiles;
            set => value = _nextMoveTiles;
        }

        private void Start()
        {
            _tileStack = new Stack<GameObject>();
            _nextMoveTiles = new List<GameObject>();
            //_speedPalyer = 4; //TODO передаём сюда скорость игрока
        }

        private void OnApplicationQuit()
        {
            _tileStack.Clear();
            _nextMoveTiles.Clear();
        }
    }
}

