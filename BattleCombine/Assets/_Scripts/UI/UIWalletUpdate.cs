using TMPro;
using UnityEngine;
using Zenject;

namespace _Scripts.UI
{
    public class UIWalletUpdate : MonoBehaviour
    {
        //its only View, not data container
        [SerializeField] private TMP_Text _coinCount;
        [SerializeField] private TMP_Text _diamondCount;

        [Inject] private PlayerAccount _playerAccount;

        private void Start()
        {
            UpdateWallet();
        }

        public void UpdateWallet()
        {
            if (_coinCount != null) _coinCount.text = _playerAccount.Gold.ToString();
            if (_diamondCount != null) _diamondCount.text = _playerAccount.Diamond.ToString();
            //_coinCount.text = _mainGameService.ArcadeCurrentCoins.ToString();
            //_diamondCount.text = _mainGameService.ArcadeBestDiamonds.ToString();
        }
    }
}