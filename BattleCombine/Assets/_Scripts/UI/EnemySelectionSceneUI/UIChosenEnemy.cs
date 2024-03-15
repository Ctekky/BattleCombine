using System;
using BattleCombine.Gameplay;
using UnityEngine;
using UnityEngine.UI;

public class UIChosenEnemy : MonoBehaviour
{
    [SerializeField] private Button _firstEnemyButton;
    [SerializeField] private Button _secondEnemyButton;
    [SerializeField] private Button _thirdEnemyButton;

    [SerializeField] private Player firstEnemy;
    [SerializeField] private Player secondEnemy;
    [SerializeField] private Player thirdEnemy;

    public event Action<Player> onEnemyClick;
    public event Action onDeselectAll;

    private void Awake()
    {
        _firstEnemyButton.onClick.AddListener(() => CheckToggleGroup(0));
        _secondEnemyButton.onClick.AddListener(() => CheckToggleGroup(1));
        _thirdEnemyButton.onClick.AddListener(() => CheckToggleGroup(2));
    }

    private void CheckToggleGroup(int number)
    {
        switch (number)
        {
            case 0:
                firstEnemy.ChangeAvatarState(true);
                secondEnemy.ChangeAvatarState(false);
                thirdEnemy.ChangeAvatarState(false);
                onEnemyClick?.Invoke(firstEnemy);
                break;
            case 1:
                firstEnemy.ChangeAvatarState(false);
                secondEnemy.ChangeAvatarState(true);
                thirdEnemy.ChangeAvatarState(false);
                onEnemyClick?.Invoke(secondEnemy);
                break;
            case 2:
                firstEnemy.ChangeAvatarState(false);
                secondEnemy.ChangeAvatarState(false);
                thirdEnemy.ChangeAvatarState(true);
                onEnemyClick?.Invoke(thirdEnemy);
                break;
        }
    }

    public void DeselectAllEnemies()
    {
        firstEnemy.ChangeAvatarState(false);
        secondEnemy.ChangeAvatarState(false);
        thirdEnemy.ChangeAvatarState(false);
        onDeselectAll?.Invoke();
    }
}