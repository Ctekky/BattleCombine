using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.Animations
{
    public class SpawnFieldTrigger : MonoBehaviour
    {
        [SerializeField] private CreateField fieldScript;

        public void OnSpawnFieldAnimationTriggerStart()
        {
            fieldScript.ChangeBackImageCanvasState(false);
        }
    }
    
}
