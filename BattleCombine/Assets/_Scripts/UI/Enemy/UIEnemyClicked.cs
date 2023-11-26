using System;
using UnityEngine;

namespace BattleCombine.UI
{
    public class UIEnemyClicked : MonoBehaviour
    {
        public Action onUIEnemyClicked;
        public void OnEnemyClicked()
        {
            onUIEnemyClicked?.Invoke();
            Debug.Log("Clicked!!!");
        }
    }
}