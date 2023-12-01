using System.Collections.Generic;
using BattleCombine.Data;
using UnityEngine;

namespace BattleCombine.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New EnemyAvatarTable", menuName = "Avatars table")]
    public class SOEnemyAvatarTable : ScriptableObject
    {
        public List<EnemyAvatarStruct> avatarList;
    }
}