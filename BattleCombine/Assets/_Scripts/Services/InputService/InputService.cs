using BattleCombine.Interfaces;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace BattleCombine.Services.InputService
{
    public class InputService : MonoBehaviour, TouchAction.ITouchActions
    {
        private TouchAction _input;

        private void OnEnable()
        {
            if (_input != null) return;
            _input = new TouchAction();
            _input.Touch.SetCallbacks(this);
            _input.Touch.Enable();
            EnhancedTouch.TouchSimulation.Enable();
            EnhancedTouch.EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouch.TouchSimulation.Disable();
            EnhancedTouch.EnhancedTouchSupport.Disable();

            EnhancedTouch.Touch.onFingerDown -= FingerDown;
            EnhancedTouch.Touch.onFingerMove -= FingerMove;
        }

        private void Start()
        {
            EnhancedTouch.Touch.onFingerDown += FingerDown;
            EnhancedTouch.Touch.onFingerMove += FingerMove;
        }

        private void FingerMove(EnhancedTouch.Finger finger)
        {
            DetectMoveOnObject(finger);
        }

        private void FingerDown(EnhancedTouch.Finger finger)
        {
            DetectTouchOnObject(finger);
        }

        private void DetectTouchOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            if (obj.GetComponent<ITouchable>() != null)
            {
                obj.GetComponent<ITouchable>().Touch();
            }
        }
        
        private void DetectMoveOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            if (obj.GetComponent<IMovable>() != null)
            {
                obj.GetComponent<IMovable>().FingerMoved();
            }
        }

        public void OnPress(InputAction.CallbackContext context)
        {
        }
    }
}