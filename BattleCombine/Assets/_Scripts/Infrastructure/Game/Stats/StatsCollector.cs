using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsCollector : MonoBehaviour
{
    Queue<Tile> cells = new Queue<Tile>();

    public void Add(Tile cell)
    {

        cells.Enqueue(cell);
      
    }

    public Tile Get()
    {
        
        return cells.Dequeue();
    }

    public bool IsHasItem()
    {
        if (cells.Count == 0) return false;
        return true;
  
    }
}
