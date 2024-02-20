using System;
using _Scripts.UI;
using BattleCombine.Gameplay;
using BattleCombine.Services;
using UnityEngine;
using Zenject;

namespace _Scripts.Game.Stats
{
	public class BoostHandler : MonoBehaviour
	{
		private event Action UpdateStats;
		
		[SerializeField] private Player _player;
		//todo - [Inject] 
		[SerializeField] private ArcadeGameService _arcadeGameService;
		
		//todo - boost values database?
		[SerializeField] private int _attackBoostValue;
		[SerializeField] private int _healthBoostValue;
		[SerializeField] private int _attackLongBoostValue;
		[SerializeField] private int _healthLongBoostValue;
		[SerializeField] private int _speedBoostValue;
		[SerializeField] private int _attackMaxDuration;
		[SerializeField] private int _healthMaxDuration;
		[SerializeField] private int _shieldMaxDuration;
		[SerializeField] private int _speedMaxDuration;

		private BoostMenu _boostMenu;

		//todo - active boost database?
		//todo - data of booster capacity
		private bool longDurationAttackBoostActive;
		private bool longDurationHealthBoostActive;
		private bool longDurationShieldBoostActive;
		private bool longDurationSpeedBoostActive;
		
		private int attackDuration;
		private int healthDuration;
		private int shieldDuration;
		private int speedDuration;

		private int defaultSpeed = 0;
		//todo --------------------------------------

		public bool IsAttackLongBoostActive => longDurationAttackBoostActive;
		public bool IsHealthLongBoostActive => longDurationHealthBoostActive;
		public bool IsShieldLongBoostActive => longDurationShieldBoostActive;
		public bool IsSpeedLongBoostActive => longDurationSpeedBoostActive;
		public int GetAttackBoostCurrentDuration => attackDuration;
		public int GetHealthBoostCurrentDuration => healthDuration;
		public int GetShieldBoostCurrentDuration => shieldDuration;
		public int GetSpeedBoostCurrentDuration => speedDuration;

		private void OnEnable()
		{
			_boostMenu = GetComponent<BoostMenu>();

			UpdateStats += _player.UpdateStats;
		}

		public void BoostUpdate()
		{
			ActiveBoosterCheck();
		}

		public void BoostAttack(bool longDuration)
		{
			if(longDuration)
			{
				if(attackDuration <= 0)
				{
					longDurationAttackBoostActive = true;
					attackDuration = _attackMaxDuration;
				}

				Debug.Log("longDuration attack Boost attack" + _attackLongBoostValue);
				_player.AddAttack(_attackLongBoostValue);
				//boostCount--
			}
			else
			{
				Debug.Log(_attackBoostValue +" attack Boost");
				_player.AddAttack(_attackBoostValue);
				//boostCount--
			}
			
			UpdateStats?.Invoke();
		}

		public void BoostHealth(bool longDuration)
		{
			if(longDuration)
			{
				if(healthDuration <= 0)
				{
					longDurationHealthBoostActive = true;
					healthDuration = _healthMaxDuration;
				}
				
				Debug.Log("longDuration health Boost hp" + _healthLongBoostValue);
				_player.ChangeHealth(_healthLongBoostValue);
				//boostCount--
			}
			else
			{
				Debug.Log("health Boost hp" + _healthBoostValue);
				_player.ChangeHealth(_healthBoostValue);
				//boostCount--
			}
			
			UpdateStats?.Invoke();
		}

		public void BoostShield()
		{
			if(shieldDuration <= 0)
			{
				Debug.Log("longDuration shield Boost");
				longDurationShieldBoostActive = true;
				shieldDuration = _shieldMaxDuration;
			}

			_player.AddShield();
			//boostCount--
			UpdateStats?.Invoke();
		}

		public void BoostSpeed()
		{
			if(longDurationSpeedBoostActive) return;
			if(speedDuration <= 0)
			{
				Debug.Log("Speed Boost+");
				longDurationSpeedBoostActive = true;
				speedDuration = _speedMaxDuration;
			}

			if(defaultSpeed == 0)
				defaultSpeed = _player.moveSpeedValue;
			
			_player.moveSpeedValue += _speedBoostValue;
			_arcadeGameService.UpdateCurrentPlayerSpeed();
			//boostCount--
			UpdateStats?.Invoke();
		}

		private void ActiveBoosterCheck()
		{
		
			if(longDurationAttackBoostActive)
			{
				attackDuration--;
				if(attackDuration <= 0)
				{
					longDurationAttackBoostActive = false;
					return;
				}

				BoostAttack(true);
			}

			if(longDurationHealthBoostActive)
			{
				healthDuration--;
				if(healthDuration <= 0)
				{
					longDurationHealthBoostActive = false;
					return;
				}

				BoostHealth(true);
			}

			if(longDurationShieldBoostActive)
			{
				shieldDuration--;
				if(shieldDuration <= 0)
				{
					longDurationShieldBoostActive = false;
					return;
				}

				BoostShield();
			}

			if(longDurationSpeedBoostActive)
			{
				speedDuration--;
				if(speedDuration <= 0)
				{
					longDurationSpeedBoostActive = false;
					return;
				}

				if(defaultSpeed != _player.moveSpeedValue)
				{
					_player.moveSpeedValue = defaultSpeed;
					_arcadeGameService.UpdateCurrentPlayerSpeed();
				}
				//BoostSpeed();
			}
			
			_boostMenu.ExitCooldown();
		}
	}
}