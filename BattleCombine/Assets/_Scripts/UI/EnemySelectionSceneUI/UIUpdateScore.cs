using BattleCombine.Services;
using TMPro;
using UnityEngine;
using Zenject;

namespace BattleCombine.UI
{
    public class UIUpdateScore : MonoBehaviour
    {
        [Inject] private MainGameService _mainGameService;
        [SerializeField] private TMP_Text currentScore;
        [SerializeField] private TMP_Text bestScore;


        private void Start()
        {
            currentScore.text = _mainGameService.ArcadeCurrentScore.ToString();
            bestScore.text = _mainGameService.ArcadeBestScore.ToString();
        }
    }
}