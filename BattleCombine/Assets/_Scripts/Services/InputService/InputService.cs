using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

namespace BattleCombine.Services.InputService
{
    public class InputService : MonoBehaviour, TouchAction.ITouchActions
    {
        [SerializeField] private List<GameObject> _touchGO;
        [SerializeField] private List<GameObject> _moveGO;
        private TouchAction _input;
        private List<ITouchable> _touchables;
        private List<IMovable> _movables;
        private TileStack _tileStack;

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
            _tileStack = FindObjectOfType<TileStack>();
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
            if (_tileStack.GetCurrentPlayerTileList().Count == 1 && _movables.Count != 0)
            {
                _touchables.Clear();
                _touchGO.Clear();
                var obj = _tileStack.GetCurrentPlayerTileList().First().GetComponent<ITouchable>();
                _touchables.Add(obj);
                _touchables.Last().Touch();
                onFingerUp?.Invoke();
                _movables.Clear();
                _moveGO.Clear();
                _touchables.Clear();
                _touchGO.Clear();
            }
            switch (_movables.Count)
            {
                case <= 0 when _touchables.Count <= 0:
                    return;
                case <= 0:
                    _touchables.Last().Touch();
                    onFingerUp?.Invoke();
                    _touchables.Clear();
                    _touchGO.Clear();
                    break;
                case 1:
                    //_touchables.Last().Touch();
                    onFingerUp?.Invoke();
                    _movables.Clear();
                    _moveGO.Clear();
                    _touchables.Clear();
                    _touchGO.Clear();
                    break;
                default:
                    _movables.Clear();
                    _moveGO.Clear();
                    _touchables.Clear();
                    _touchGO.Clear();
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
            _touchGO.Add(obj);
        }

        private void DetectMoveOnObject(EnhancedTouch.Finger finger)
        {
            var raycast = Camera.main.ScreenPointToRay(finger.screenPosition);
            var hit = Physics2D.Raycast(raycast.origin, raycast.direction);
            if (hit.collider == null) return;
            var obj = hit.transform.gameObject;
            var iMovableObj = obj.GetComponent<IMovable>();
            TileState tileState = obj.GetComponent<Tile>().GetTileState;
            Tile tile = obj.GetComponent<Tile>();
            if (iMovableObj == null || tileState == TileState.EnabledState || tile.CantUse == true) return;
            tile.CantUse = true;
            /*if (_movables.Contains(iMovableObj))
            {
                iMovableObj.FingerMoved();
                _movables.Remove(iMovableObj);
                _moveGO.Remove(obj);
                return;
            }*/
            _movables.Clear();
            _moveGO.Clear();

            _movables.Add(obj.GetComponent<IMovable>());
            _moveGO.Add(obj);
            obj.GetComponent<IMovable>().FingerMoved();
        }

        public void OnPress(InputAction.CallbackContext context)
        {
        }
    }
}