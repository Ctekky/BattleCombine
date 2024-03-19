using UnityEngine;
using UnityEngine.UI;

namespace BattleCombine.UI
{
	public class UISpriteImageStateChange : MonoBehaviour
	{
		[SerializeField] private Sprite spriteStateTrue;
		[SerializeField] private Sprite spriteStateFalse;
		[SerializeField] private Image image;
		[SerializeField] private Image avatarBackgroud;

		[Header("Borders")]
		[SerializeField] private GameObject avatarBorder;
		[SerializeField] private GameObject statBorder;

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

			avatarBorder.SetActive(show);
			statBorder.SetActive(show);
		}

		public void EnableState()
		{
			image.sprite = spriteStateTrue;

			ShowBorders(true);

			if(avatarBackgroud == null) return;
			avatarBackgroud.color = new Color(1, 1, 1, 1);
		}

		public void DisableState()
		{
			image.sprite = spriteStateFalse;

			ShowBorders(false);
			
			if(avatarBackgroud == null) return;
			avatarBackgroud.color = new Color(1, 1, 1, 0.5f);
		}
	}
}