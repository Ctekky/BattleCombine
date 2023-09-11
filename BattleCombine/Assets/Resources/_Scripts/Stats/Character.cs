using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [SerializeField] private Stats stats;
    public int Move_speed_value;


    public bool Shielded => stats.Shielded;
    public int Attack_value => stats.Attack_value;
    public int Health_value => stats.Health_value;





    protected virtual void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        NextMove();
        stats.Shielded = false;
        stats.Attack_value = stats.Attack_value_default;
        stats.Health_value = stats.Health_value_default;
    }

    public void ChangeHealth(int add_health)
    {
        int tmp = stats.Health_value + add_health;
     
        if (tmp > stats.Health_value_default)
        {
            stats.Health_value = stats.Health_value_default;
        }
        else if (tmp <= 0)
        {
            stats.Health_value = 0;
        }
        else
        {
            stats.Health_value = tmp;
        }

    }

  
    public void AddAttack(int add_attack)
    {
        
        int tmp = stats.Attack_value + add_attack;

        if (tmp > stats.Attack_value_default)
        {
            stats.Attack_value = stats.Attack_value_default;
        }
        else if (tmp <= 0)
        {
            stats.Attack_value = 0;
        }
        else
        {
            stats.Attack_value = tmp;
        }

    }

    public void AddShield()
    {
        stats.Shielded = true;
    }

    public void MakeMove(int move)
    {
        Move_speed_value -= move;

    }

    public bool CheckMove()
    {
        if (Move_speed_value == 0) return false;
        return true;
    }


    public void NextMove()
    {
        Move_speed_value = stats.Move_speed_value_default;
    }

    
}
