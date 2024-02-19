using System;
using UnityEngine;

namespace BattleCombine.Services
{
    public class GlobalEventService : MonoBehaviour
    {
        public Action onFieldRefresh;
        public Action onTileRefresh;

        public void OnFieldRefreshInvoke()
        {
            onFieldRefresh?.Invoke();
        }

        public void OnTileRefreshInvoke()
        {
            onTileRefresh?.Invoke();
        }
        
    }
}