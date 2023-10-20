using BattleCombine.Interfaces;
using UnityEngine;

namespace BattleCombine.Tests
{
    public class Square : MonoBehaviour, ITouchable, IMovable
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private bool _isSelected;

        public void FingerMoved()
        {
            if(_isSelected) return;
            _isSelected = true;
            spriteRenderer.color = Color.green;
        }
        public void Touch()
        {
            if (_isSelected)
            {
                _isSelected = false;
                spriteRenderer.color = Color.red;
            }
            else
            {
                _isSelected = true;
                spriteRenderer.color = Color.green;
            }
        }
    }
}