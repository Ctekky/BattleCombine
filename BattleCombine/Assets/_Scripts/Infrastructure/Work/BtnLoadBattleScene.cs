using BattleCombine.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BtnLoadBattleScene : MonoBehaviour, ICoroutineRunner
{
    private SceneNameService _sceneNameService;
    private SceneLoader _sceneLoader;

    [Inject]
    public void GetSceneNameService(SceneNameService sceneNameService)
    {
        _sceneNameService = sceneNameService;
    }

    private void Awake()
    {
        _sceneLoader = new SceneLoader(this);
    }
    public void OnClick()
    {
        _sceneLoader.Load(_sceneNameService.BattleName);
    }
}
