using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Scripts.UI.CharacterChoose
{
	public class SwipeAvatar : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField] private SwipeAvatarProvider _provider;
		[SerializeField] private List<Transform> avatars;

		private Transform currentAvatar;
		private Vector3 initialPosition;
		private Vector3 initialPositionNext;
		private Vector3 initialPositionPrev;
		
		private float distanceMoved;
		private bool swipeLeft;

		private void OnEnable()
		{
			transform.localPosition = Vector3.zero;
	
			currentAvatar = this.gameObject.transform;
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			initialPosition = transform.localPosition;
			initialPositionNext = avatars[2].localPosition;
			initialPositionPrev = avatars[0].localPosition;
		}

		public void OnDrag(PointerEventData eventData)
		{
			//transform.localPosition = new Vector2(transform.localPosition.x + eventData.delta.x, transform.localPosition.y);
//
			//if(transform.localPosition.x - initialPosition.x > 0)
			//{
			//	transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 0,
			//		(initialPosition.x + transform.localPosition.x) / (Screen.width / 2)));
//
			//	swipeLeft = true;
			//}
			//else
			//{
			//	transform.localEulerAngles = new Vector3(0, 0, Mathf.LerpAngle(0, 0,
			//		(initialPosition.x - transform.localPosition.x) / (Screen.width / 2)));
			//	
			//	swipeLeft = false;
			//}
			
			foreach (var card in avatars)
			{
				card.localPosition += new Vector3(eventData.delta.x, 0, 0);
			}

			var selectedCard = avatars[0]; 
			
			selectedCard.localEulerAngles = 
				selectedCard.localPosition.x - initialPosition.x > 0 
					? new Vector3(0, 0, Mathf.LerpAngle(0, 0, (initialPosition.x + selectedCard.localPosition.x) / (Screen.width / 2))) 
					: new Vector3(0, 0, Mathf.LerpAngle(0, 0, (initialPosition.x - selectedCard.localPosition.x) / (Screen.width / 2)));
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			distanceMoved = Mathf.Abs(transform.localPosition.x - initialPosition.x);
			if(distanceMoved < 0.2 * Screen.width)
			{
				transform.localPosition = initialPosition;
				avatars[2].localPosition = initialPositionNext;
				avatars[0].localPosition = initialPositionPrev;
			}
			else
			{
				swipeLeft = !(transform.localPosition.x > initialPosition.x);
				avatars[2].localPosition = initialPositionNext;
				avatars[0].localPosition = initialPositionPrev;
				StartCoroutine(MovedCard(true));
			}
		}

		public void SwipeLeftButtonClick()
		{
			swipeLeft = true;
			StartCoroutine(MovedCard(false));
		}	
		
		public void SwipeRightButtonClick()
		{
			swipeLeft = false;
			StartCoroutine(MovedCard(false));
		}

		private IEnumerator MovedCard(bool swipe)
		{
			float time = 0;
			var colorFadeDuration = 0.2f; 
			var moveSpeed = 5f; 

			if (swipeLeft)
			    _provider.AvatarMoveLeft();
			else
			    _provider.AvatarMoveRight();

			if(!swipe)
			{
				while (time < colorFadeDuration)
				{
					time += Time.deltaTime;
					var position = currentAvatar.localPosition;

					var newPositionX = Mathf.SmoothStep(position.x, position.x + (swipeLeft ? -1 : 1) * Screen.width,
						moveSpeed * Time.deltaTime);

					currentAvatar.localPosition = new Vector3(newPositionX, position.y, position.z);

					if(currentAvatar.TryGetComponent<Image>(out var image))
					{
						image.color = new Color(image.color.r, image.color.g, image.color.b, Mathf.Lerp(1, 0, time / colorFadeDuration));
					}

					yield return null;
				}
			}


			currentAvatar.localPosition = initialPosition;
			if(currentAvatar.TryGetComponent<Image>(out var images))
			{
				images.color = new Color(images.color.r, images.color.g, images.color.b, 1);
			}
		}
	}
}