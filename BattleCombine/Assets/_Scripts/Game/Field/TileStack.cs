using BattleCombine.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class TileStack : MonoBehaviour
    {
        [SerializeField] private int speedPlayer;
        [SerializeField] private List<GameObject> nextMoveTiles;
        [SerializeField] private IDPlayer player;

        private Stack<GameObject> _tileStackPlayer1;
        private Stack<GameObject> _tileStackPlayer2;

        public int SpeedPlayer { get => speedPlayer; }
        public Stack<GameObject> TilesStackPlayer1
        {
            get => _tileStackPlayer1;
            set => value = _tileStackPlayer1;
        }
        public Stack<GameObject> TilesStackPlayer2
        {
            get => _tileStackPlayer2;
            set => value = _tileStackPlayer2;
        }
        public List<GameObject> NextMoveTiles
        {
            get => nextMoveTiles;
            set => value = nextMoveTiles;
        }
        public IDPlayer IDPlayer
        {
            get => player;
            set => player = value;
        }
        private void Start()
        {
            player = IDPlayer.Player1;
            _tileStackPlayer1 = new Stack<GameObject>();
            _tileStackPlayer2 = new Stack<GameObject>();
            nextMoveTiles = new List<GameObject>();
            //speedPlayer = 4; //TODO: get player speed

        }
        private void OnDisable()
        {
            _tileStackPlayer1.Clear();
            nextMoveTiles.Clear();
        }
    }
}

