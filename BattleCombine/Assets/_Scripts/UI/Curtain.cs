using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Curtain : MonoBehaviour
{
	private const string initialScene = "Initial";
	private const string arcadeScene = "EnemySelectionScene";
	private const string battleScene = "ArcadeGameLoop";

	[SerializeField] private GameObject _curtainPanel;

	private Coroutine _curtainRoutine;
	private Image _curtainImage;
	private float _changeRate = 0.01f;

	private void Awake()
	{
		_curtainImage = _curtainPanel.GetComponent<Image>();
	}

	private void Start()
	{
		_curtainRoutine = StartCoroutine(OnStartSceneRoutine());
	}

	public void MoveToAnotherScene(string arcadeScene)
	{
		_curtainRoutine = StartCoroutine(OnSceneLoadRoutine(arcadeScene));
	}

	private IEnumerator OnStartSceneRoutine()
	{
		_curtainPanel.SetActive(true);
		_curtainImage.color = new Color(_curtainImage.color.r, _curtainImage.color.g, _curtainImage.color.b, 1);
		
		while (_curtainImage.color.a > 0f)
		{
			var newColor = _curtainImage.color;
			newColor.a -= _changeRate;
			_curtainImage.color = newColor;
			yield return new WaitForEndOfFrame();
		}

		_curtainImage.raycastTarget = false;

		_curtainPanel.SetActive(false);
		StopCoroutine(_curtainRoutine);
	}

	private IEnumerator OnSceneLoadRoutine(string sceneName)
	{
		_curtainPanel.SetActive(true);
		_curtainImage.raycastTarget = true;

		_curtainImage.color = new Color(_curtainImage.color.r, _curtainImage.color.g, _curtainImage.color.b, 0);
		while (_curtainImage.color.a < 1.0f)
		{
			var newColor = _curtainImage.color;
			newColor.a += _changeRate;
			_curtainImage.color = newColor;
			yield return new WaitForEndOfFrame();
		}

		SceneManager.LoadScene(sceneName);
		StopCoroutine(_curtainRoutine);
	}
}