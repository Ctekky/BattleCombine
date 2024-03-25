using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.UI;
using UnityEngine;

public class EnemySmoothChoose : MonoBehaviour
{
	[SerializeField] private UIChosenEnemy _toggleGroup;

	[Header("Скорость выбора")]
	[SerializeField] private float _speed = 1f;	
	[Header("Отложенный старт")]
	[SerializeField] private float _startDelay = 1f;
	
    private Coroutine rutine;

    private void Start()
    {
        rutine = StartCoroutine(MotionRoutine());
    }

    private IEnumerator MotionRoutine()
    {
	    var count = 0;
	    
	    yield return new WaitForSeconds(_startDelay);
	    
	    _toggleGroup.DeselectAllEnemies();
	    
	    while (count < 3)
	    {
		    _toggleGroup.ManualToggle(count);
		    
		    count++;

		    yield return new WaitForSeconds(_speed);
	    }
	    
	    _toggleGroup.DeselectAllEnemies();
	    
	    this.gameObject.SetActive(false);
	    
	    StopCoroutine(rutine);
    }
}
