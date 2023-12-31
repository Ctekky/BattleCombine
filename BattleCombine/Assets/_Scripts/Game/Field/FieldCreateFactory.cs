using UnityEngine;
using Zenject;

namespace BattleCombine.Gameplay
{
    public class FieldCreateFactory : PlaceholderFactory<Transform, GameObject>
    {
        private GameObject _template;

        public FieldCreateFactory(GameObject template)
        {
            _template = template;
        }
   
        public override GameObject Create(Transform parent)
        {
            return GameObject.Instantiate(_template, parent);
        }
    }
}