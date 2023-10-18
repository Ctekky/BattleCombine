using BattleCombine.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class TileStack : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] private NextTurnButton nextTurnButton;
        [SerializeField] private int speedPlayer;
        [SerializeField] private IDPlayer player;
        [SerializeField] private List<GameObject> nextMoveTiles = new List<GameObject>();

        [SerializeField] private List<GameObject> tileListPlayer1;
        [SerializeField] private List<GameObject> tileListPlayer2;

        private Stack<GameObject> _tileStackPlayer1 = new Stack<GameObject>();
        private Stack<GameObject> _tileStackPlayer2 = new Stack<GameObject>();

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
        public List<GameObject> TilesListPlayer1
        {
            get => tileListPlayer1;
            set => tileListPlayer1 = value;
        }
        public List<GameObject> TilesListPlayer2
        {
            get => tileListPlayer2;
            set => tileListPlayer2 = value;
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

        public GameManager GetGameManager { get => gameManager; }
     
        public NextTurnButton GetNextTurnButton { get => nextTurnButton; }
        private void Start()
        {
            player = IDPlayer.Player1;
            gameManager = FindObjectOfType<GameManager>();
            nextTurnButton = FindObjectOfType<NextTurnButton>();
            //speedPlayer = 4; //TODO: get player speed

        }
        private void OnDisable()
        {
            _tileStackPlayer1.Clear();
            nextMoveTiles.Clear();
        }
    }
}

