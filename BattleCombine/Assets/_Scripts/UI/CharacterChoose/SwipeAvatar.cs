using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI.CharacterChoose
{
	public class SwipeAvatar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		//[SerializeField] private SwipeProvider provider;

		[SerializeField] private Transform currentAvatar;

		private Vector3 _initialPosition;
		private float _distanceMoved;
		private bool _swipeLeft;

		private void OnEnable()
		{
			transform.localPosition = Vector3.zero;
			transform.localEulerAngles = Vector3.zero;
		}

		public void OnDrag(PointerEventData eventData)
		{
			transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);

			if(transform.localPosition.x - _initialPosition.x > 0)
			{
				transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 0,
					(_initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
			}
			else
			{
				transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 0,
					(_initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_initialPosition = transform.localPosition;
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x);
			if(_distanceMoved < 0.2 * Screen.width)
			{
				transform.localPosition = _initialPosition;
				transform.localEulerAngles = Vector3.zero;
			}
			else
			{
				_swipeLeft = !(transform.localPosition.x > _initialPosition.x);
				StartCoroutine(MovedCard());
			}
		}

		private IEnumerator MovedCard()
		{
			float time = 0;
			var colorFadeDuration = 0.5f; 
			var moveSpeed = 2f; 

			while (time < colorFadeDuration)
			{
				time += Time.deltaTime;

				var newPositionX = Mathf.SmoothStep(currentAvatar.localPosition.x, currentAvatar.localPosition.x + (_swipeLeft ? -1 : 1) * Screen.width,
					moveSpeed * Time.deltaTime);

				currentAvatar.localPosition = new Vector3(newPositionX, currentAvatar.localPosition.y, currentAvatar.localPosition.z);

				var image = currentAvatar.GetComponent<Image>();
				if(image != null)
				{
					image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, time / colorFadeDuration));
				}
				
				yield return null;
			}

			// if (_swipeLeft)
			//     provider.SwipeLeft();
			// else
			//     provider.SwipeRight();
		}
	}
}