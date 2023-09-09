using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseStats : MonoBehaviour
{

    private Player player;
    private StatsCollector stats_collector;

    private void Awake()
    {
        stats_collector = FindObjectOfType<StatsCollector>();
        player = FindObjectOfType<Player>(true);
    }
    public void Increase()
    {
        while (stats_collector.IsHasItem() && player.Move_speed_value == 0)
        {
            Tile tile = stats_collector.Get();
            if (StatsEnum.Attack == tile.StatsEnum)
            {
                player.AddAttack(tile.Tile_value);
            }

            else if (StatsEnum.Health == tile.StatsEnum)
            {
                player.ChangeHealth(tile.Tile_value);
            }

            else if (StatsEnum.Shield == tile.StatsEnum)
            {
                player.AddShield();
            }

        }
        if(player.Move_speed_value == 0)
        player.NextMove();


    }


}




