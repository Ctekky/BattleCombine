using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using BattleCombine.Services;
using BattleCombine.Services.InputService;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Ai
{
    public class AiHandler : MonoBehaviour
    {
        public static event Action ChangeEnemyStance;

        public PathFinder GetPathFinder { get; private set; }
        public int GetMoodHealthPercent { get; private set; }
        public int AiSpeed { get; private set; }
        private bool _isPaused;
        public AiArchetypes GetCurrentArchetype => currentArchetype;

        //todo - if HP == X, then change stance;
        [field: SerializeField] private AiArchetypes currentArchetype { get; set; }

        [Header("AI Move Animation Speed")] [SerializeField]
        private float speed = 0.5f;

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
        private ArcadeGameService _gameManager;
        private AiBaseEnemy _currentAiBaseEnemy;
        private CreateField _field;
        private NextTurnButton _nextTurnButton;
        private PathFinder _pathFinder;
        private Coroutine _movePathRoutine;
        private Coroutine _giveTurnToAiRoutine;
        private Coroutine _fieldCheckRoutine;
        private int _lastStepIndex = -1;
        private bool _isAiTurn;
        private bool _isStanceChanged;

        private const int ArchetypeCount = 3;

        public void SetupAIHandler(ArcadeGameService arcadeGameService, InputService inputService)
        {
            _gameManager = arcadeGameService;
            _inputService = inputService;
            ChangeEnemyStance += ChangeAiStance;
            _gameManager.onPlayerChange += GiveAiTurn;
            _gameManager.onBattleEnd += PauseAi;
        }

        private void PauseAi()
        {
            _isPaused = true;
        }

        private void Awake()
        {
            AiSpeed = FindObjectOfType<TileStack>().SpeedPlayer;
            _nextTurnButton = FindObjectOfType<NextTurnButton>();
            _field = FindObjectOfType<CreateField>();
            _isPaused = false;
        }

        private void Start()
        {
            _isStanceChanged = false;

            _fieldCheckRoutine = StartCoroutine(FieldCheckRoutine());
        }

        private void FindFirstPathToAi()
        {
            ChooseArchetype();

            if (_lastStepIndex < 0)
            {
                _pathFinder.FindStartPath();
            }
        }

        private void GiveAiTurn()
        {
            if (_isPaused) return;
            _isAiTurn = !_isAiTurn;

            if (HealthToChangeStance() && !_isStanceChanged)
            {
                ChangeEnemyStance();
                Debug.Log("Stance Changed");
                _isStanceChanged = true;
            }

            _giveTurnToAiRoutine = StartCoroutine(GiveTurnToAiRoutine());
        }

        private void MovePath()
        {
            if (!_isAiTurn) return;

            if (_lastStepIndex >= 0)
            {
                _pathFinder.KeepLastPathStarts(_lastStepIndex);
            }

            if (!_currentAiBaseEnemy.GetStance)
                _currentAiBaseEnemy.Init();

            _movePathRoutine = StartCoroutine(MovePathRoutine());
        }

        [ContextMenu("Change enemy stance")]
        private void ChangeAiStance()
        {
            //todo - if enemy can return stance back - rewrite func!..
            _pathFinder.CurrentWeights = new List<int>();
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

            _currentAiBaseEnemy.AiHandler = this;
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

            while (currentStep < _pathFinder.CurrentWay.Count)
            {
                _currentAiBaseEnemy.MakeStep();
                //todo - addEffects
                yield return new WaitForSeconds(speed);
                currentStep++;
            }

            _lastStepIndex = _pathFinder.CurrentWay.Last();

            _currentAiBaseEnemy.EndAiTurn();
            _inputService.onFingerUp?.Invoke();

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
            ChangeEnemyStance -= ChangeAiStance;
            _gameManager.onPlayerChange -= GiveAiTurn;
            StopAllCoroutines();
        }

        private bool HealthToChangeStance()
        {
            return _gameManager.GetPlayerAiHealth < Convert.ToInt32(20 * GetMoodHealthPercent) / 100;
        }

        private IEnumerator FieldCheckRoutine()
        {
            var isFieldNull = false;

            while (!isFieldNull)
            {
                yield return new WaitForSeconds(0.5f);
                if (_field.GetAiStartTileIndex > 0)
                    isFieldNull = true;
            }

            _pathFinder = new PathFinder(AiSpeed, _field, this);
            GetPathFinder = _pathFinder;

            FindFirstPathToAi();
            StopCoroutine(_fieldCheckRoutine);
        }
    }
}