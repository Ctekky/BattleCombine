using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fight : MonoBehaviour
{
    private Player player;
    private Enemy enemy;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        enemy = FindObjectOfType<Enemy>();
    }

   
    public void Fighting()
    {   if (gameOver) return;
        PlayerTakeDamage();
        if (gameOver) return;
        EnemyTakeDamage();
    }

    private bool gameOver;
    private void PlayerTakeDamage()
    {
        if (player.Shielded)
        {
            player.ChangeHealth(-(enemy.Attack_value / 2));

        }
        else if (player.Shielded == false)
        {
            player.ChangeHealth(-(enemy.Attack_value));
        }

        if (player.Health_value <= 0)
        {
            print("игрок проиграл");
            gameOver = true;
            player.StartGame();
        }

    }

    private void EnemyTakeDamage()
    {
        if (enemy.Shielded)
        {
            enemy.ChangeHealth(-(player.Attack_value / 2));

        }
        else if (enemy.Shielded == false)
        {
            enemy.ChangeHealth(-(player.Attack_value));
        }

        if (enemy.Health_value <= 0)
        {
            print("враг проиграл");
            gameOver = true;
            enemy.StartGame();
        }

    }

}
