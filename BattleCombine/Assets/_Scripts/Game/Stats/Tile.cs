using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private StatsCollector stats_collector;
    [SerializeField] private StatsEnum stats_enum;
    [SerializeField] private int tile_value;
    private Player player;

    public int Tile_value => tile_value;
    public StatsEnum StatsEnum => stats_enum;


    private void Start()
    {
        stats_collector = FindObjectOfType<StatsCollector>(true);
        player = FindObjectOfType<Player>(true);
    }
    public void AddStats()
    {
       if( player.CheckMove() == false) return;
        stats_collector.Add(this);
        player.MakeMove(1);
    }
}
