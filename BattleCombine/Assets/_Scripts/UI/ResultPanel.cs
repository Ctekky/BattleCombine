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

		public void SetRewardText(string coin, string diamond, string exp, string attack)
		{
			_coin.text = coin;
			_diamonds.text = diamond;
			_exp.text = exp;
			_attack.text = attack;
		}		
		
		public void SetScore(string score, string bestScore)
		{
			_score.text = score;
			_bestScore.text = bestScore;
		}
	}
}
