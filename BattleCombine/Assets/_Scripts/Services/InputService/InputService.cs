using System;
using System.Collections.Generic;
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
            if (_movables.Count > 0 || _touchables.Count > 0)
            {
                _movables.Clear();
                _touchables.Clear();
                onFingerUp?.Invoke();
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
            obj.GetComponent<ITouchable>().Touch();
        }

        private void DetectMoveOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            if (obj.GetComponent<IMovable>() == null) return;
            _movables.Add(obj.GetComponent<IMovable>());
            obj.GetComponent<IMovable>().FingerMoved();
        }

        public void OnPress(InputAction.CallbackContext context)
        {
        }
    }
}