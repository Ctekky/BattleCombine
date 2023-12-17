using BattleCombine.Services;
using UnityEngine;
using Zenject;

public class TestScript : MonoBehaviour
{
    [Inject] private MainGameService _mainGameService;
    private void Update()
    {
        Debug.Log(_mainGameService.ArcadeBestScore.ToString());
    }
}
