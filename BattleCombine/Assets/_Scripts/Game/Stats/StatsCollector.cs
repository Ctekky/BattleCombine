using System.Collections.Generic;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class StatsCollector : MonoBehaviour
    {
        private readonly Queue<Tile> _cells = new Queue<Tile>();

        public void Add(Tile cell)
        {
            if (_cells.Contains(cell))
                _cells.Dequeue();
            else
            {
                _cells.Enqueue(cell);
            }
        }

        public Tile Get()
        {
            return _cells.Dequeue();
        }

        public void RemoveFromQueue()
        {
            _cells.Dequeue();
        }

        public bool IsHasItem()
        {
            if (_cells.Count == 0) return false;
            return true;
        }
    }
}