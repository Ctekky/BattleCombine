using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI
{
	public class BoostMenu : MonoBehaviour
	{
		[Header("Boost Cooldown")]
		[SerializeField] private int _cooldown = 7;
		
		[Header("BoostImg")]
		[SerializeField] private Image _health;
		[SerializeField] private Image _healthPlus;
		[SerializeField] private Image _attack;
		[SerializeField] private Image _attackPlus;
		[SerializeField] private Image _shield;
		[SerializeField] private Image _speed;

		[Header("Sprites")]
		[SerializeField] private Sprite[] _boostSprites;
		[SerializeField] private Sprite[] _boostCooldownSprites;

		[Header("Values")]
		[SerializeField] private TMP_Text _healthText;
		[SerializeField] private TMP_Text _healthPlusText;
		[SerializeField] private TMP_Text _attackText;
		[SerializeField] private TMP_Text _attackPlusText;
		[SerializeField] private TMP_Text _shieldText;
		[SerializeField] private TMP_Text _speedText;

		[Header("CooldownTextValues")]
		[SerializeField] private TMP_Text _healthCooldownText;
		[SerializeField] private TMP_Text _healthPlusCooldownText;
		[SerializeField] private TMP_Text _attackCooldownText;
		[SerializeField] private TMP_Text _attackPlusCooldownText;
		[SerializeField] private TMP_Text _shieldCooldownText;
		[SerializeField] private TMP_Text _speedCooldownText;

		private bool isHealthCooldown;
		private bool isHealthPlusCooldown;
		private bool isAttackCooldown;
		private bool isAttackPlusCooldown;
		private bool isShieldCooldown;
		private bool isSpeedCooldown;

		private Button _healthButton;
		private Button _healthPlusButton;
		private Button _attackButton;
		private Button _attackPlusButton;
		private Button _shieldButton;
		private Button _speedButton;
		
		public string SetHealthText {get => _healthText.text; set => _healthText.text = value;}
		public string SetHealthPlusText {get => _healthPlusText.text; set => _healthPlusText.text = value;}
		public string SetAttackText {get => _attackText.text; set => _attackText.text = value;}
		public string SetAttackPlusText {get => _attackPlusText.text; set => _attackPlusText.text = value;}
		public string SetShieldText {get => _shieldText.text; set => _shieldText.text = value;}
		public string SetSpeedText {get => _speedText.text; set => _speedText.text = value;}

		private void Start()
		{
			_healthButton = _health.GetComponent<Button>();
			_healthButton.onClick.AddListener(() => HealthCooldown(_cooldown));
			_healthPlusButton = _healthPlus.GetComponent<Button>();
			_healthPlusButton.onClick.AddListener(() => HealthPlusCooldown(_cooldown));
			_attackButton = _attack.GetComponent<Button>();
			_attackButton.onClick.AddListener(() => AttackCooldown(_cooldown));
			_attackPlusButton = _attackPlus.GetComponent<Button>();
			_attackPlusButton.onClick.AddListener(() => AttackPlusCooldown(_cooldown));
			_shieldButton = _shield.GetComponent<Button>();
			_shieldButton.onClick.AddListener(() => ShieldCooldown(_cooldown));
			_speedButton = _speed.GetComponent<Button>();
			_speedButton.onClick.AddListener(() => SpeedCooldown(_cooldown));
		}

		public void HealthCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isHealthCooldown, _health, _healthCooldownText, 0, cooldownValue);
		}		
		
		public void HealthPlusCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isHealthPlusCooldown, _healthPlus, _healthPlusCooldownText, 1, cooldownValue);
		}		
		
		public void AttackCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isAttackCooldown, _attack, _attackCooldownText, 2, cooldownValue);
		}		
		
		public void AttackPlusCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isAttackPlusCooldown, _attackPlus, _attackPlusCooldownText, 3, cooldownValue);
		}		
		
		public void ShieldCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isShieldCooldown, _shield, _shieldCooldownText, 4, cooldownValue);
		}		
		
		public void SpeedCooldown(int cooldownValue)
		{
			SwitchCooldown(ref isSpeedCooldown, _speed, _speedCooldownText, 5, cooldownValue);
		}		
		

		private void SwitchCooldown(ref bool currentBool, Image currentImage, TMP_Text currentText, int spriteIndex, int cooldownValue)
		{
			currentBool = !currentBool;

			switch (currentBool)
			{
				case true:
					currentImage.sprite = _boostCooldownSprites[spriteIndex];
					currentText.gameObject.SetActive(true);
					currentText.text = cooldownValue.ToString();
					break;
				default:
					currentImage.sprite = _boostSprites[spriteIndex];
					currentText.gameObject.SetActive(false);
					break;
			}
		}
	}
}