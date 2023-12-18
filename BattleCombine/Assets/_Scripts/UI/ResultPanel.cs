using TMPro;
using UnityEngine;

namespace _Scripts.UI
{
	public class ResultPanel : MonoBehaviour
	{
		[SerializeField] private TMP_Text _playerLevel;
	
		[Header("reward")]
		[SerializeField] private TMP_Text _coin;
		[SerializeField] private TMP_Text _diamonds;
		[SerializeField] private TMP_Text _exp;
		[SerializeField] private TMP_Text _attack;

		[Header("ScoreText")]
		[SerializeField] private TMP_Text _score;
		[SerializeField] private TMP_Text _bestScore;

		public void SetRewardText(int coin, int diamond, int exp, int attack)
		{
			_coin.text = coin.ToString();
			_diamonds.text = diamond.ToString();
			_exp.text = exp.ToString();
			_attack.text = attack.ToString();
		}		
		
		public void SetScore(int score, int bestScore)
		{
			_score.text = score.ToString();
			_bestScore.text = bestScore.ToString();
		}
	}
}
