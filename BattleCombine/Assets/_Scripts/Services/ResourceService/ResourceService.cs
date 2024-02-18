using BattleCombine.ScriptableObjects;
using UnityEngine;

public class ResourceService : MonoBehaviour
{
    [SerializeField] private SOEnemyAvatarTable avatarDB;

    public SOEnemyAvatarTable GetAvatarDB => avatarDB;

    public int GetAvatarID()
    {
        var result = 1;
        

        return result;
    }
}
