using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
	public class WalletPanel : MonoBehaviour
	{
		//its only View, not data container
		[SerializeField] private TMP_Text _coinCountText;
		[SerializeField] private TMP_Text _diamondCountText;
		[SerializeField] private TMP_Text _scoreCountText;
		[SerializeField] private TMP_Text _bestScoreCountText;

		//todo - add score mechanics
		public void AddScore(int value)
		{
			if(_scoreCountText != null)
				_scoreCountText.text = value.ToString();

			CheckBestScore(value);
		}

		public void AddDiamonds(int value)
		{
			if(_diamondCountText != null)
				_coinCountText.text = value.ToString();
		}

		public void AddCoins(int value)
		{
			if(_coinCountText != null)
				_diamondCountText.text = value.ToString();
		}

		private void CheckBestScore(int value)
		{
			if(_bestScoreCountText == null)
				return;

			//0 = bestScore
			if(0 < value)
			{
				_bestScoreCountText.text = value.ToString();
			}
		}
	}
}