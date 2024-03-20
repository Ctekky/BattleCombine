using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace BattleCombine.UI
{
	public class UISpriteImageStateChange : MonoBehaviour
	{
		[SerializeField] private bool matchPanel;

		[SerializeField] private Sprite spriteStateTrue;
		[SerializeField] private Sprite spriteStateFalse;
		[SerializeField] private Image image;
		[SerializeField] private Image avatarBackgroud;
		[SerializeField] private Image HeartImage;
		[SerializeField] private Image attackImage;
		[SerializeField] private GameObject speedArea;

		[Header("Borders")]
		[SerializeField] private GameObject avatarBorder;
		[SerializeField] private GameObject statBorder;

		[Header("Для ЧБ при выборе перса")]
		[SerializeField] private Sprite _avatarBackgroudSprite;
		[SerializeField] private Sprite _avatarBackgroudSpriteNega;
		[SerializeField] private Sprite _attack;
		[SerializeField] private Sprite _attackNega;
		[SerializeField] private Sprite _heart;
		[SerializeField] private Sprite _heartNega;
		[SerializeField] private Sprite _speed;
		[SerializeField] private Sprite _speedNega;

		private List<Image> speedImages = new List<Image>();

		public Sprite GetTrueStateSprite => spriteStateTrue;

		public Sprite GetFalseStateSprite => spriteStateFalse;

		public void SetupSprites(Sprite spriteTrue, Sprite spriteFalse)
		{
			spriteStateTrue = spriteTrue;
			spriteStateFalse = spriteFalse;
			EnableState();
			ShowBorders(false);
		}

		private void Start()
		{
			DisableState();
		}

		private void ShowBorders(bool show)
		{
			if(avatarBorder == null && statBorder == null) return;
			UpdateSpeedArea(speedArea.transform);

			avatarBorder.SetActive(show);
			statBorder.SetActive(show);
		}

		public void EnableState()
		{
			image.sprite = spriteStateTrue;

			ShowBorders(true);

			if(avatarBackgroud == null) return;
			DeactivateNega();
		}

		public void DisableState()
		{
			if(matchPanel) return;

			image.sprite = spriteStateFalse;

			ShowBorders(false);

			if(avatarBackgroud == null) return;
			ActivateNega();
		}

		private void ActivateNega()
		{
			avatarBackgroud.sprite = _avatarBackgroudSpriteNega;
			HeartImage.sprite = _heartNega;
			attackImage.sprite = _attackNega;

			foreach (var ball in speedImages)
			{
				ball.sprite = _speedNega;
			}
		}

		private void DeactivateNega()
		{
			avatarBackgroud.sprite = _avatarBackgroudSprite;
			HeartImage.sprite = _heart;
			attackImage.sprite = _attack;

			foreach (var ball in speedImages)
			{
				ball.sprite = _speed;
			}
		}

		private void UpdateSpeedArea(Transform parent)
		{
			if(SceneManager.GetActiveScene().name != "EnemySelectionScene") return;

			foreach (Transform child in parent)
			{
				var imageComponent = child.GetComponent<Image>();

				if(imageComponent != null)
				{
					speedImages.Add(imageComponent);
				}

				UpdateSpeedArea(child);
			}
		}
	}
}