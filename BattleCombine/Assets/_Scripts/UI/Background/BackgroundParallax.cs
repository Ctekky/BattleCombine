using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.UI.Background
{
	public class BackgroundParallax : MonoBehaviour
	{
		[Header("Image")]
		[SerializeField] private GameObject _background;
		
		[SerializeField] private float _parallaxEffectMultiplier = 0.5f;
		[SerializeField] private float _smoothRate = 0.5f;
		[SerializeField] private float _minXCoordinates; 
		[SerializeField] private float _maxXCoordinates;

		private float targetPositionX;
    
		private void Start()
		{
			targetPositionX = _background.transform.position.x;
		}

		private void Update()
		{
			var acceleration = Input.acceleration;
			var position = _background.transform.position;

			//calculate paralax + position
			targetPositionX += acceleration.x * _parallaxEffectMultiplier;

			//set limits
			targetPositionX = Mathf.Clamp(targetPositionX, _minXCoordinates, _maxXCoordinates);

			//Lerp image
			position = new Vector3(
				Mathf.Lerp(position.x, targetPositionX, _smoothRate * Time.deltaTime),
				position.y,
				position.z
			);

			_background.transform.position = position;

			//var smoothPositionX = Mathf.SmoothStep(transform.position.x, targetPositionX, smoothRate * Time.deltaTime);
			//transform.position = new Vector3(smoothPositionX, transform.position.y, transform.position.z);
		}
	}
}