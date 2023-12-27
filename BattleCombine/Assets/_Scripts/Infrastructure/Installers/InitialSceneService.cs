using BattleCombine.Services;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class InitialSceneService : MonoBehaviour
{
    [Inject] private SaveManager _saveManager;
    [Inject] private MainGameService _mainGameService;
    [SerializeField] private string arcadeSceneName;
    private bool _isLoadBattle;

    public void Initialize()
    {
        _isLoadBattle = false;
        _saveManager.Initialization();
        if (_saveManager.CheckForSavedData())
        {
            _saveManager.LoadGame();
            if (_mainGameService.IsBattleActive) _isLoadBattle = true;
        }
        else
        {
            _saveManager.NewGame();
            _saveManager.SaveGame();
        }
    }

    public void LoadBattleScene()
    {
        _mainGameService.IsEnemySelectionScene = false;
        if(_isLoadBattle)
            SceneManager.LoadScene(arcadeSceneName);
    }
}
