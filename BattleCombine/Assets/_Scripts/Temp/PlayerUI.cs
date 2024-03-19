using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Animations;
using BattleCombine.Services;
using BattleCombine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace _Scripts.Temp
{
	public class PlayerUI : MonoBehaviour
	{
		public event Action onEndRerollTrigger;

		[SerializeField] private TMP_Text attackText;
		[SerializeField] private TMP_Text healthText;
		[SerializeField] private TMP_Text playerLevelText;
		[SerializeField] private Slider playerLevelSlider;
		[SerializeField] private Image shieldSprite;
		[SerializeField] private GameObject speedArea;
		[SerializeField] private GameObject speedPrefab;
		[SerializeField] private GameObject speedPrefabBonus;
		[SerializeField] private List<GameObject> createdSpeedObjectList;
		[SerializeField] private Animator enemyRerollAnimator;
		[SerializeField] private AnimationTriggerToEventRelay animToTrigger;

		[Header("BoostTimer")] [SerializeField]
		private Image _heartImg;

		[SerializeField] private Image _swordsImg;
		[SerializeField] private Image _heartsBackImg;
		[SerializeField] private Image _swordsBackImg;
		[SerializeField] private List<Sprite> _cooldownSprites;
		[SerializeField] private GameObject _healthCooldownObj;
		[SerializeField] private GameObject _attackCooldownObj;
		[SerializeField] private TMP_Text _healthTimerText;
		[SerializeField] private TMP_Text _attackTimerText;

		[Header("Avatars")]
		[SerializeField] private GameObject avatarGameObject;


		private bool _isShielded;

		private bool _isPlayerBoostCooldownOn;

		private AnimationService _animationService;

		public List<GameObject> GetCreatedSpeedObjectList
		{
			get => createdSpeedObjectList;
		}

		private void OnEnable()
		{
			if(animToTrigger != null) animToTrigger.onRerollTrigger += EndRerollAnimation;
		}

		public void UpdateLevelSlider(float value)
		{
			playerLevelSlider.value = value;
		}

		public void UpdatePlayerLevel(int value)
		{
			playerLevelText.text = value.ToString();
		}

		public void SetupAnimationService(AnimationService animationService)
		{
			_animationService = animationService;
		}

		public void PlayRerollAnimation(string animName)
		{
			_animationService.PlayAnim(animName, enemyRerollAnimator);
		}

		private void EndRerollAnimation(string animName)
		{
			_animationService.StopAnim(animName, enemyRerollAnimator);
			onEndRerollTrigger?.Invoke();
		}

		private void SetupStat(TMP_Text text, string value)
		{
			text.text = value;
		}

		public void SetUpAllStats(string attackValue, string healthValue, bool isShielded)
		{
			SetupStat(attackText, attackValue);
			SetupStat(healthText, healthValue);
			SetShield(isShielded);
			ChangeShieldSprite();
		}

		private void SetShield(bool value)
		{
			_isShielded = value;
			ChangeShieldSprite();
		}

		private void ChangeShieldSprite()
		{
			shieldSprite.enabled = _isShielded;
		}

		public void SetupAvatar(Sprite enableState, Sprite disableState)
		{
			var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
			changeSpriteScript.SetupSprites(enableState, disableState);
		}

		public Sprite GetSprite(bool state)
		{
			var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
			var sprite = state ? changeSpriteScript.GetTrueStateSprite : changeSpriteScript.GetFalseStateSprite;
			return sprite;
		}

		public void ChangeAvatarState(bool state)
		{
			var changeSpriteScript = avatarGameObject.GetComponent<UISpriteImageStateChange>();
			if(state)
				changeSpriteScript.EnableState();
			else
				changeSpriteScript.DisableState();
		}

		public void SetupSpeed(int speed)
		{
			DeleteAllSpeedObject();
			for(var i = 0; i < speed; i++)
			{
				var speedObject = Instantiate(speedPrefab, speedArea.transform);
				createdSpeedObjectList.Add(speedObject);
			}

			var speedObjectBonus = Instantiate(speedPrefabBonus, speedArea.transform);
			createdSpeedObjectList.Add(speedObjectBonus);
			speedObjectBonus.SetActive(false);

			if(SceneManager.GetActiveScene().name == "ArcadeGameLoop")
			{
				speedArea.GetComponent<SpeedPanelAnimationHelper>().FindSpeedBallAnimationHelper();
			}
		}

		private void DeleteAllSpeedObject()
		{
			foreach (var speedObject in createdSpeedObjectList)
			{
				Destroy(speedObject);
			}

			createdSpeedObjectList.Clear();
		}

		public void EnabledBonusSpeedSrite()
		{
			createdSpeedObjectList.Last().SetActive(true);
			if(animToTrigger != null) animToTrigger.onRerollTrigger -= EndRerollAnimation;
			DeleteAllSpeedObject();
		}

		private void OnDisable()
		{
			DeleteAllSpeedObject();
		}

	}
}