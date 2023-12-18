using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
	public class UIWalletUpdate : MonoBehaviour
	{
		//its only View, not data container
		[SerializeField] private TMP_Text _coinCount;
		[SerializeField] private TMP_Text _diamondCount;

		private void Start()
		{
			UpdateWallet();
		}
		
		public void UpdateWallet()
		{
			//_coinCount.text = _mainGameService.ArcadeCurrentCoins.ToString();
			//_diamondCount.text = _mainGameService.ArcadeBestDiamonds.ToString();
		}
	}
}