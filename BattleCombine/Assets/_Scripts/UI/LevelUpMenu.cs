using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Scripts.UI
{
	public class LevelUpMenu : MonoBehaviour
	{
		[SerializeField] private TMP_Text _playerLevel;

		[Header("Level Up Stats")]
		[SerializeField] private TMP_Text _attack;
		[SerializeField] private TMP_Text _health;
		[SerializeField] private TMP_Text _speed;

		[Header("Buttons")]
		[SerializeField] private Button _attackButton;
		[SerializeField] private Button _healthButton;
		[SerializeField] private Button _speedButton;

		private LevelUpColorChanger attackColorChanger;
		private LevelUpColorChanger healthColorChanger;
		private LevelUpColorChanger speedColorChanger;

		public event Action<int> StatChangeEvent;

		private void OnEnable()
		{
			attackColorChanger = _attackButton.GetComponent<LevelUpColorChanger>();
			_attackButton.onClick.AddListener(() => CheckToggleGroup(0));

			healthColorChanger = _healthButton.GetComponent<LevelUpColorChanger>();
			_healthButton.onClick.AddListener(() => CheckToggleGroup(1));

			if(_speed == null) 
				return;
			
			speedColorChanger = _speedButton.GetComponent<LevelUpColorChanger>();
			_speedButton.onClick.AddListener(() => CheckToggleGroup(2));
		}

		public void SetLevelUpText(int newLevel = 0, int attack = 0, int health = 0, int speed = 0)
		{
			_playerLevel.text = newLevel.ToString();
			//todo currentLevel + 1

			_attack.text = attack.ToString();
			_health.text = health.ToString();
			_speed.text = speed.ToString();
		}

		//todo - rework, check links
		private void CheckToggleGroup(int number)
		{
			switch (number)
			{
				case 0:
					attackColorChanger.EnableState();
					healthColorChanger.DisableState();
					speedColorChanger.DisableState();
					//StatChangeEvent?.Invoke(_attackButton);
					StatChangeEvent?.Invoke(1);
					break;
				case 1:
					attackColorChanger.DisableState();
					healthColorChanger.EnableState();
					speedColorChanger.DisableState();
					//StatChangeEvent?.Invoke(_healthButton);
					StatChangeEvent?.Invoke(2);
					break;
				case 2:
					attackColorChanger.DisableState();
					healthColorChanger.DisableState();
					speedColorChanger.EnableState();
					//StatChangeEvent?.Invoke(_speedButton);
					StatChangeEvent?.Invoke(3);
					break;
			}
		}

		private void OnDisable()
		{
			_attackButton.onClick.RemoveListener(() => CheckToggleGroup(0));
			_healthButton.onClick.RemoveListener(() => CheckToggleGroup(1));

			if(_speed == null) 
				return;
			_speedButton.onClick.RemoveListener(() => CheckToggleGroup(2));
		}
	}
}