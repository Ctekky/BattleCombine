using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace BattleCombine.Services.InputService
{
    public class InputService : MonoBehaviour, TouchAction.ITouchActions
    {
        private TouchAction _input;
        private List<ITouchable> _touchables;
        private List<IMovable> _movables;
        public Action onFingerUp;

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
            EnhancedTouch.Touch.onFingerUp -= FingerUp;
        }

        private void Start()
        {
            EnhancedTouch.Touch.onFingerDown += FingerDown;
            EnhancedTouch.Touch.onFingerMove += FingerMove;
            EnhancedTouch.Touch.onFingerUp += FingerUp;
            _touchables = new List<ITouchable>();
            _movables = new List<IMovable>();
        }

        private void FingerMove(EnhancedTouch.Finger finger)
        {
            DetectMoveOnObject(finger);
        }

        private void FingerDown(EnhancedTouch.Finger finger)
        {
            DetectTouchOnObject(finger);
        }

        private void FingerUp(EnhancedTouch.Finger finger)
        {
            switch (_movables.Count)
            {
                case <= 0 when _touchables.Count <= 0:
                    return;
                case <= 0:
                    _touchables.Last().Touch();
                    onFingerUp?.Invoke();
                    _touchables.Clear();
                    break;
                case 1:
                    _touchables.Last().Touch();
                    onFingerUp?.Invoke();
                    _movables.Clear();
                    _touchables.Clear();
                    break;
                default:
                    _movables.Clear();
                    _touchables.Clear();
                    onFingerUp?.Invoke();
                    break;
            }
        }

        private void DetectTouchOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            if (obj.GetComponent<ITouchable>() == null) return;
            _touchables.Add(obj.GetComponent<ITouchable>());
        }

        private void DetectMoveOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            var iMovableObj = obj.GetComponent<IMovable>();
            if (iMovableObj == null) return;
            if (_movables.Contains(iMovableObj))
            {
                iMovableObj.FingerMoved();
                _movables.Remove(iMovableObj);
                return;
            }

            _movables.Add(obj.GetComponent<IMovable>());
            obj.GetComponent<IMovable>().FingerMoved();
        }

        public void OnPress(InputAction.CallbackContext context)
        {
        }
    }
}