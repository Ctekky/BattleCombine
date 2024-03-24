using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpeedPanelAnimationHelper : MonoBehaviour
{
    [SerializeField] private List<GameObject> speedBallGameObj;
    [SerializeField] private List<SpeedBallAnimationHelper> speedBallAnimationHelpers;
    [SerializeField] private TileStack tileStack;
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private IDPlayer idPlayer;

    [SerializeField] private int countTileInList;

    public int CountTilesInList
    {
        get => countTileInList;
        set => countTileInList = value;
    }

    private void Awake()
    {
        tileStack = FindObjectOfType<TileStack>();
        currentPlayer = transform.parent.parent.gameObject;
        if (currentPlayer.name == "Player")
        {
            idPlayer = IDPlayer.Player1;
        }
        else if (currentPlayer.name == "Enemy")
        {
            idPlayer = IDPlayer.Player2;
        }
    }
    private void OnEnable()
    {
        if (SceneManager.GetActiveScene().name == "ArcadeGameLoop")
        {
            tileStack.addTileInStack += OnAnimationSpeedBallTriggerEnabled;
            tileStack.reduceTileInStack += OnAnimationSpeedBallTriggerDisabled;
            tileStack.refreshCountTileAndSpeed += OnAnimationSpeedBallTriggerRefresh;
        }
    }
    private void OnDisable()
    {
        if (SceneManager.GetActiveScene().name == "ArcadeGameLoop")
        {
            tileStack.addTileInStack -= OnAnimationSpeedBallTriggerEnabled;
            tileStack.reduceTileInStack -= OnAnimationSpeedBallTriggerDisabled;
            tileStack.refreshCountTileAndSpeed -= OnAnimationSpeedBallTriggerRefresh;
        }
    }
    public void FindSpeedBallAnimationHelper()
    {
        speedBallGameObj = GetComponentInParent<PlayerUI>().GetCreatedSpeedObjectList;

        foreach (GameObject i in speedBallGameObj)
        {
            speedBallAnimationHelpers.Add(i.GetComponentInChildren<SpeedBallAnimationHelper>());
        }
        
    }
    private void SetAnimationSpeedBallBool(bool flag, int tileID)
    {
        if (currentPlayer.name == "Player" && tileStack.IDPlayer == idPlayer)
        {
            var speedAnimHelp = speedBallAnimationHelpers[tileID - 1];
            speedAnimHelp.SetAnimationBool(flag);
        }
        else if (currentPlayer.name == "Enemy" && tileStack.IDPlayer == idPlayer)
        {
            var speedAnimHelp = speedBallAnimationHelpers[tileID - 1];
            speedAnimHelp.SetAnimationBool(flag);
        }

    }
    private void OnAnimationSpeedBallTriggerEnabled()
    {
        countTileInList += 1;
        SetAnimationSpeedBallBool(true, countTileInList);
        Debug.Log("Add");
    }
    private void OnAnimationSpeedBallTriggerDisabled()
    {
        SetAnimationSpeedBallBool(false, countTileInList);
        countTileInList -= 1;
        Debug.Log("Reduce");
    }
    private void OnAnimationSpeedBallTriggerRefresh()
    {
        countTileInList = 0;
        foreach (var tileAnimHelper in speedBallAnimationHelpers)
        {
            tileAnimHelper.SetAnimationBool(false);
        }

        Debug.Log("Refresh");
    }


}
