using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.Services.InputService;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Ai
{
    public class AiHandler : MonoBehaviour
    {
        public static Action StartAiMove;
        public static Action ChangeEnemyStance;
        
        public List<Tile> CurrentWay { get; set; }
        public int GetMoodHealthPercent { get; private set; }
        public int AiSpeed { get; private set; }
        
        //todo - if HP == X, then change stance;
        [field: SerializeField] private AiArchetypes currentArchetype { get; set; }

        [Header("Weights and other")] [SerializeField]
        private int[] tankFullHealthWeights;

        [SerializeField] private int[] tankDamagedWeights;
        [SerializeField] private int tankHealthToChangeMood;
        [SerializeField] private int[] attackFullHealthWeights;
        [SerializeField] private int[] attackDamagedWeights;
        [SerializeField] private int attackHealthToChangeMood;
        [SerializeField] private int[] balanceFullHealthWeights;
        [SerializeField] private int[] balanceDamagedWeights;
        [SerializeField] private int balanceHealthToChangeMood;

        private Random _rand;
        private InputService _inputService;
        private AiBaseEnemy _currentAiBaseEnemy;
        private CreateField _field;
        private NextTurnButton _nextTurnButton;
        private PathFinder _pathFinder;
        private Coroutine _movePathRoutine;
        private Coroutine _giveTurnToAiRoutine;
        private int _lastStepIndex = -1;
        private bool _isAiTurn;
        
        private const int ArchetypeCount = 3;

        private void OnEnable()
        {
            _inputService = FindObjectOfType<InputService>();
            
            StartAiMove += MovePath;
            ChangeEnemyStance += ChangeAiStance;
            //todo - change to ai speed
            //_nextTurnButton.onButtonPressed += GiveAiTurn;
            GameManager.OnPlayerChange += GiveAiTurn;
        }

        private void Awake()
        {
            AiSpeed = FindObjectOfType<TileStack>().SpeedPlayer;
            _nextTurnButton = FindObjectOfType<NextTurnButton>();
            _field = FindObjectOfType<CreateField>();
            
            _pathFinder = new PathFinder();
            
            _pathFinder.AiSpeed = AiSpeed;
            _pathFinder.Field = _field;
            _pathFinder.AiHandler = this;
        }

        private void Start()
        {
            FindFirstPathToAi();
        }

        private void FindFirstPathToAi()
        {
            ChooseArchetype();
            
            if (_lastStepIndex < 0)
                _pathFinder.FindStartPath();
        }

        private void GiveAiTurn()
        {
            _isAiTurn = !_isAiTurn;
            _giveTurnToAiRoutine = StartCoroutine(GiveTurnToAiRoutine());
        }

        private void MovePath()
        {
            if (!_isAiTurn) return;

            if (_lastStepIndex >= 0)
                _pathFinder.KeepLastPathStarts(_lastStepIndex);
            
            if (!_currentAiBaseEnemy.IsAiInitialised)
                _currentAiBaseEnemy.Init();

            _movePathRoutine = StartCoroutine(MovePathRoutine());
        }

        [ContextMenu("Change enemy stance")]
        private void ChangeAiStance()
        {
            //todo - if enemy can return stance back - rewrite func!..
            _pathFinder.CurrentWeights = new();
            _pathFinder.CurrentWeights = _pathFinder.NextStanceWeights;
        }

        private void ChooseArchetype()
        {
            _rand = new Random();
            //todo - take max value from enemy archetype count
            switch (_rand.Next(0, ArchetypeCount))
            {
                case 0:
                    currentArchetype = AiArchetypes.Tank;
                    break;
                case 1:
                    currentArchetype = AiArchetypes.Attack;
                    break;
                case 2:
                    currentArchetype = AiArchetypes.Balance;
                    break;
                default:
                    break;
            }

            ApplyAiArchetype(currentArchetype);
        }

        private void ApplyAiArchetype(AiArchetypes enemyType)
        {
            switch (enemyType)
            {
                case AiArchetypes.Tank:
                    AddWeightToList(tankFullHealthWeights, tankDamagedWeights);
                    GetMoodHealthPercent = tankHealthToChangeMood;
                    _currentAiBaseEnemy = new Tank();
                    break;
                case AiArchetypes.Attack:
                    AddWeightToList(attackFullHealthWeights, attackDamagedWeights);
                    GetMoodHealthPercent = attackHealthToChangeMood;
                    _currentAiBaseEnemy = new Attack();
                    break;
                case AiArchetypes.Balance:
                    AddWeightToList(balanceFullHealthWeights, balanceDamagedWeights);
                    GetMoodHealthPercent = balanceHealthToChangeMood;
                    _currentAiBaseEnemy = new Balance();
                    break;
                case AiArchetypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentAiBaseEnemy._aiHandler = this;
            _pathFinder.CurrentAiBaseEnemy = _currentAiBaseEnemy;
        }

        private void AddWeightToList(IEnumerable<int> arrayFirst, IEnumerable<int> arraySecond)
        {
            _pathFinder.CurrentWeights = new List<int>();
            _pathFinder.NextStanceWeights = new List<int>();
            
            _pathFinder.CurrentWeights.AddRange(arrayFirst);
            _pathFinder.NextStanceWeights.AddRange(arraySecond);
        }

        private IEnumerator MovePathRoutine()
        {
            var currentStep = 0;

            while (currentStep < CurrentWay.Count)
            {
                _currentAiBaseEnemy.MakeStep();
                //todo - addEffects
                yield return new WaitForSeconds(.5f);
                currentStep++;
            }

            var lastTileCount = -1;
            foreach (var tile in _field.GetTileList)
            {
                lastTileCount++;
                if (tile == CurrentWay.Last())
                    _lastStepIndex = lastTileCount;
            }

            _currentAiBaseEnemy.EndAiTurn();
            //todo - write it right :D
            //var _turnButton = FindObjectOfType<NextTurnButton>();
            //_turnButton.Touch();
            _inputService.onFingerUp?.Invoke();
            _pathFinder.PathDictionary.Clear();

            StopCoroutine(_movePathRoutine);
        }

        private IEnumerator GiveTurnToAiRoutine()
        {
            yield return new WaitForSeconds(1f);
            MovePath();

            StopCoroutine(_giveTurnToAiRoutine);
        }

        private void OnDisable()
        {
            StartAiMove -= MovePath;
            ChangeEnemyStance -= ChangeAiStance;
            //_nextTurnButton.onButtonPressed -= GiveAiTurn;
            GameManager.OnPlayerChange -= GiveAiTurn;
            StopAllCoroutines();
        }
    }
}