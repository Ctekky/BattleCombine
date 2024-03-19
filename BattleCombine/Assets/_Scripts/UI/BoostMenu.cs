using _Scripts.Game.Stats;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class BoostMenu : MonoBehaviour
	{
		[Header("BoostButtons")]
		[SerializeField] private Button _healthButton;
		[SerializeField] private Button _healthPlusButton;
		[SerializeField] private Button _attackButton;
		[SerializeField] private Button _attackPlusButton;
		[SerializeField] private Button _shieldButton;
		[SerializeField] private Button _speedButton;

		[Header("BoostPauseMenuButtons")]
		[SerializeField] private Button _healthPauseMenuButton;
		[SerializeField] private Button _healthPauseMenuPlusButton;
		[SerializeField] private Button _attackPauseMenuButton;
		[SerializeField] private Button _attackPauseMenuPlusButton;
		[SerializeField] private Button _shieldPauseMenuButton;
		[SerializeField] private Button _speedPauseMenuButton;

		[Header("Sprites")]
		[SerializeField] private Sprite[] _boostSprites;
		[SerializeField] private Sprite[] _boostCooldownSprites;

		private BoostHandler boostHandler;
		private PauseMenu pauseMenu;

		//todo - count value of boosters;

		private void OnEnable()
		{
			boostHandler = GetComponent<BoostHandler>();
		}

		private void Start()
		{
			_healthButton.onClick.AddListener(OnHealthBoostClick);
			_attackButton.onClick.AddListener(AttackCooldown);

			_healthPlusButton.onClick.AddListener(() => HealthPlusCooldown(boostHandler.IsHealthLongBoostActive));
			_attackPlusButton.onClick.AddListener(() => AttackPlusCooldown(boostHandler.IsAttackLongBoostActive));
			_shieldButton.onClick.AddListener(() => ShieldCooldown(boostHandler.IsShieldLongBoostActive));
			_speedButton.onClick.AddListener(() => SpeedCooldown(boostHandler.IsSpeedLongBoostActive));

			_healthPauseMenuButton.onClick.AddListener(OnHealthBoostClick);
			_attackPauseMenuButton.onClick.AddListener(AttackCooldown);

			_healthPauseMenuPlusButton.onClick.AddListener(() => HealthPlusCooldown(boostHandler.IsHealthLongBoostActive));
			_attackPauseMenuPlusButton.onClick.AddListener(() => AttackPlusCooldown(boostHandler.IsAttackLongBoostActive));
			_shieldPauseMenuButton.onClick.AddListener(() => ShieldCooldown(boostHandler.IsShieldLongBoostActive));
			_speedPauseMenuButton.onClick.AddListener(() => SpeedCooldown(boostHandler.IsSpeedLongBoostActive));
		}

		private void OnHealthBoostClick()
		{
			boostHandler.BoostHealth(false);
		}

		private void AttackCooldown()
		{
			boostHandler.BoostAttack(false);
		}

		private void HealthPlusCooldown(bool isHealthPlusCooldown)
		{
			if(isHealthPlusCooldown) return;

			boostHandler.BoostHealth(true);
			EnterCooldownView(_healthPlusButton.GetComponent<BoostButton>().GetImage, _healthPlusButton.GetComponent<BoostButton>().GetCooldownText, 1,
				boostHandler.GetHealthBoostCurrentDuration);

			EnterCooldownView(_healthPauseMenuPlusButton.GetComponent<BoostButton>().GetImage, _healthPauseMenuPlusButton.GetComponent<BoostButton>().GetCooldownText, 1,
				boostHandler.GetHealthBoostCurrentDuration);
		}

		private void AttackPlusCooldown(bool isAttackPlusCooldown)
		{
			if(isAttackPlusCooldown) return;

			boostHandler.BoostAttack(true);
			EnterCooldownView(_attackPlusButton.GetComponent<BoostButton>().GetImage, _attackPlusButton.GetComponent<BoostButton>().GetCooldownText, 3,
				boostHandler.GetAttackBoostCurrentDuration);

			EnterCooldownView(_attackPauseMenuPlusButton.GetComponent<BoostButton>().GetImage, _attackPauseMenuPlusButton.GetComponent<BoostButton>().GetCooldownText, 3,
				boostHandler.GetAttackBoostCurrentDuration);
		}

		private void ShieldCooldown(bool isShieldCooldown)
		{
			if(isShieldCooldown) return;

			boostHandler.BoostShield();
			EnterCooldownView(_shieldButton.GetComponent<BoostButton>().GetImage, _shieldButton.GetComponent<BoostButton>().GetCooldownText, 4,
				boostHandler.GetShieldBoostCurrentDuration);

			EnterCooldownView(_shieldPauseMenuButton.GetComponent<BoostButton>().GetImage, _shieldPauseMenuButton.GetComponent<BoostButton>().GetCooldownText, 4,
				boostHandler.GetShieldBoostCurrentDuration);
		}

		private void SpeedCooldown(bool isSpeedCooldown)
		{
			if(isSpeedCooldown) return;

			boostHandler.BoostSpeed();
			EnterCooldownView(_speedButton.GetComponent<BoostButton>().GetImage, _speedButton.GetComponent<BoostButton>().GetCooldownText, 5,
				boostHandler.GetSpeedBoostCurrentDuration);

			EnterCooldownView(_speedPauseMenuButton.GetComponent<BoostButton>().GetImage, _speedPauseMenuButton.GetComponent<BoostButton>().GetCooldownText, 5,
				boostHandler.GetSpeedBoostCurrentDuration);
		}


		private void EnterCooldownView(Image currentImage, TMP_Text currentText, int spriteIndex, int cooldownValue = 0)
		{
			currentImage.sprite = _boostCooldownSprites[spriteIndex];
			currentText.gameObject.SetActive(true);
			currentText.text = cooldownValue.ToString();
		}

		private void ExitCooldownView(Image currentImage, TMP_Text currentText, int spriteIndex)
		{
			currentImage.sprite = _boostSprites[spriteIndex];
			currentText.gameObject.SetActive(false);
		}

		public void ExitCooldown()
		{
			if(!boostHandler.IsAttackLongBoostActive)
			{
				ExitCooldownView(_attackPlusButton.GetComponent<BoostButton>().GetImage, _attackPlusButton.GetComponent<BoostButton>().GetCooldownText, 3);
				ExitCooldownView(_attackPauseMenuPlusButton.GetComponent<BoostButton>().GetImage,
					_attackPauseMenuPlusButton.GetComponent<BoostButton>().GetCooldownText, 3);
			}

			if(!boostHandler.IsHealthLongBoostActive)
			{
				ExitCooldownView(_healthPlusButton.GetComponent<BoostButton>().GetImage, _healthPlusButton.GetComponent<BoostButton>().GetCooldownText, 1);
				ExitCooldownView(_healthPauseMenuPlusButton.GetComponent<BoostButton>().GetImage,
					_healthPauseMenuPlusButton.GetComponent<BoostButton>().GetCooldownText, 1);
			}

			if(!boostHandler.IsShieldLongBoostActive)
			{
				ExitCooldownView(_shieldButton.GetComponent<BoostButton>().GetImage, _shieldButton.GetComponent<BoostButton>().GetCooldownText, 4);
				ExitCooldownView(_shieldPauseMenuButton.GetComponent<BoostButton>().GetImage,
					_shieldPauseMenuButton.GetComponent<BoostButton>().GetCooldownText, 4);
			}

			if(!boostHandler.IsSpeedLongBoostActive)
			{
				ExitCooldownView(_speedButton.GetComponent<BoostButton>().GetImage, _speedButton.GetComponent<BoostButton>().GetCooldownText, 5);
				ExitCooldownView(_speedPauseMenuButton.GetComponent<BoostButton>().GetImage, _speedPauseMenuButton.GetComponent<BoostButton>().GetCooldownText,
					5);
			}
		}

		private void OnDisable()
		{
			_healthButton.onClick.RemoveListener(OnHealthBoostClick);
			_attackButton.onClick.RemoveListener(AttackCooldown);
			_healthPlusButton.onClick.RemoveListener(() => HealthPlusCooldown(boostHandler.IsHealthLongBoostActive));
			_attackPlusButton.onClick.RemoveListener(() => AttackPlusCooldown(boostHandler.IsAttackLongBoostActive));
			_shieldButton.onClick.RemoveListener(() => ShieldCooldown(boostHandler.IsShieldLongBoostActive));
			_speedButton.onClick.RemoveListener(() => SpeedCooldown(boostHandler.IsSpeedLongBoostActive));

			_healthPauseMenuButton.onClick.RemoveListener(OnHealthBoostClick);
			_healthPauseMenuPlusButton.onClick.RemoveListener(AttackCooldown);
			_attackPauseMenuButton.onClick.RemoveListener(() => HealthPlusCooldown(boostHandler.IsHealthLongBoostActive));
			_attackPauseMenuPlusButton.onClick.RemoveListener(() => AttackPlusCooldown(boostHandler.IsAttackLongBoostActive));
			_shieldPauseMenuButton.onClick.RemoveListener(() => ShieldCooldown(boostHandler.IsShieldLongBoostActive));
			_speedPauseMenuButton.onClick.RemoveListener(() => SpeedCooldown(boostHandler.IsSpeedLongBoostActive));
		}
	}
}