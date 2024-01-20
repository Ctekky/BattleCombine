using System;
using _Scripts.UI;
using BattleCombine.Gameplay;
using UnityEngine;

namespace _Scripts.Game.Stats
{
	public class BoostHandler : MonoBehaviour
	{
		private event Action UpdateStats;
		
		[SerializeField] private Player _player;
		
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
		private bool longDurationAttackBoostActive;
		private bool longDurationHealthBoostActive;
		private bool longDurationShieldBoostActive;
		private bool longDurationSpeedBoostActive;
		
		private int attackDuration;
		private int healthDuration;
		private int shieldDuration;
		private int speedDuration;
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

		private void Start()
		{
			//todo - round change
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
			}
			else
			{
				Debug.Log(_attackBoostValue +" attack Boost");
				_player.AddAttack(_attackBoostValue);
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
			}
			else
			{
				Debug.Log("health Boost hp" + _healthBoostValue);
				_player.ChangeHealth(_healthBoostValue);
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
			UpdateStats?.Invoke();
		}

		public void BoostSpeed()
		{
			if(speedDuration <= 0)
			{
				Debug.Log("longDuration speed Boost");
				longDurationSpeedBoostActive = true;
				speedDuration = _speedMaxDuration;
			}

			//todo - speed add
			UpdateStats?.Invoke();
		}

		private void ActiveBoosterCheck()
		{
			if(longDurationAttackBoostActive)
			{
				if(attackDuration <= 0)
				{
					longDurationAttackBoostActive = false;
					return;
				}

				attackDuration--;
				BoostAttack(true);
			}

			if(longDurationHealthBoostActive)
			{
				if(healthDuration <= 0)
				{
					longDurationHealthBoostActive = false;
					return;
				}

				healthDuration--;
				BoostHealth(true);
			}

			if(longDurationShieldBoostActive)
			{
				if(shieldDuration <= 0)
				{
					longDurationShieldBoostActive = false;
					return;
				}

				shieldDuration--;
				BoostShield();
			}

			if(longDurationSpeedBoostActive)
			{
				if(speedDuration <= 0)
				{
					longDurationSpeedBoostActive = false;
					return;
				}

				speedDuration--;
				BoostSpeed();
			}
			
			_boostMenu.ExitCooldown();
		}
	}
}