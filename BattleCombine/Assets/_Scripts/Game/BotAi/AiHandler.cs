using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Ai
{
    public class AiHandler : MonoBehaviour
    {
        public static Action StartAiMove;
        public static Action ChangeEnemyStance;

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

        //todo - separate weights to data base and link it here
        private Dictionary<List<Tile>, int> _pathDictionary = new();
        private List<int> _lastTilesToFindNewPath = new();
        private Random _rand;
        private AiBaseEnemy _currentAiBaseEnemy;
        private CreateField _field;
        private NextTurnButton _nextTurnButton;
        private Coroutine _movePathRoutine;
        private Coroutine _giveTurnToAiRoutine;
        private int _lastStepIndex = -1;
        private bool _isAiTurn;

        public List<int> CurrentWeights { get; private set; }
        public List<int> NextStanceWeights { get; private set; }
        public List<Tile> CurrentWay { get; private set; }
        public int GetMoodHealthPercent { get; private set; }
        public int AiSpeed { get; private set; }

        private void OnValidate()
        {
            _field = FindObjectOfType<CreateField>();
        }

        private void OnEnable()
        {
            StartAiMove += MovePath;
            ChangeEnemyStance += ChangeAiStance;
            //todo - change to ai speed
            _nextTurnButton.onButtonPressed += GiveAiTurn;
        }

        private void Awake()
        {
            AiSpeed = FindObjectOfType<TileStack>().SpeedPlayer;
            _nextTurnButton = FindObjectOfType<NextTurnButton>();
        }

        private void FindFirstPathToAi()
        {
            if (_currentAiBaseEnemy != null) return;
            CurrentWeights = new();
            NextStanceWeights = new();
            ChooseArchetype();

            if (_lastStepIndex < 0)
                FindAllPaths();
        }

        private void GiveAiTurn()
        {
            _isAiTurn = !_isAiTurn;
            _giveTurnToAiRoutine = StartCoroutine(GiveTurnToAiRoutine());
        }

        private void MovePath()
        {
            if (!_isAiTurn) return;

            FindFirstPathToAi();
            if (_lastStepIndex >= 0)
                KeepLastPathStarts(_lastStepIndex);

            _movePathRoutine = StartCoroutine(MovePathRoutine());
        }

        [ContextMenu("Change enemy stance")]
        private void ChangeAiStance()
        {
            //todo - if enemy can return stance back - rewrite func!..
            CurrentWeights = new();
            CurrentWeights = NextStanceWeights;
        }

        private void ChooseArchetype()
        {
            _rand = new();
            //todo - take max value from enemy archetype count
            var archetypeCount = 3;

            switch (_rand.Next(0, archetypeCount))
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

        //find all exist pathes for current AiSpeed
        private void FindAllPaths()
        {
            //todo - find path and write it to dict
            var count = -1;
            foreach (var tile in _field.GetTileList)
            {
                count++;
                if (tile != _field.GetAiStartTile) continue;
                FindPathsFromTile(count);
                break;
            }

            if (!_currentAiBaseEnemy.IsAiInitialised)
                _currentAiBaseEnemy.Init();
        }

        //find path, and if its done - add to dict
        private void FindPathsFromTile(int startIndex)
        {
            var tileList = (new List<Tile>(_field.GetTileList));
            var gridSize = _field.GetFieldSize;
            var newPath = new List<Tile>();
            var currentIndex = startIndex;

            for (var i = 0; i < AiSpeed; i++)
            {
                newPath.Add(tileList[currentIndex]);
                var candidateIndexes = new List<int>();

                FindCandidates(candidateIndexes, gridSize, currentIndex);
                //Choose the one with the maximum weight
                //todo - link the weights
                var maxWeight = -1;
                foreach (var index in candidateIndexes)
                {
                    var weight = FindWeight(tileList[index]);
                    if (weight <= maxWeight
                        || newPath.Contains(tileList[index])
                        || tileList[index].StateMachine.CurrentState == tileList[index].DisabledState)
                        continue;
                    
                    if(newPath.Contains(tileList[index])) continue;

                    maxWeight = weight;
                    currentIndex = index;
                }
            }

            //todo - change path count to its weight
            if (newPath.Count < AiSpeed)
                return;

            AddSumOfWeights(newPath);
            CurrentWay ??= newPath;
        }


        private void KeepLastPathStarts(int currentIndex)
        {
            var tileList = (new List<Tile>(_field.GetTileList));
            var gridSize = _field.GetFieldSize;
            var candidateIndexes = new List<int>();

            FindCandidates(candidateIndexes, gridSize, currentIndex);

            foreach (var index in candidateIndexes.Where(index
                         => tileList[index].StateMachine.CurrentState != tileList[index].DisabledState))
            {
                _lastTilesToFindNewPath.Add(index);
            }

            if (_lastTilesToFindNewPath == null) return;
            {
                foreach (var index in _lastTilesToFindNewPath)
                {
                    FindPathsFromTile(index);
                }

                FindBestPath();
            }
        }

        private void FindCandidates(List<int> candidateIndexes, int gridSize, int currentIndex)
        {
            //Add tile from Left
            if (currentIndex % gridSize != 0)
                candidateIndexes.Add(currentIndex - 1);
            //Add tile from Right
            if (currentIndex % gridSize != gridSize - 1)
                candidateIndexes.Add(currentIndex + 1);
            //Add tile from Top
            if (currentIndex >= gridSize)
                candidateIndexes.Add(currentIndex - gridSize);
            //Add tile from Bottom
            if (currentIndex < gridSize * (gridSize - 1))
                candidateIndexes.Add(currentIndex + gridSize);
        }

        private int FindWeight(Tile tile)
        {
            return tile.GetTileType switch
            {
                CellType.Attack => CurrentWeights[0],
                CellType.Health => CurrentWeights[1],
                CellType.Shield => CurrentWeights[2],
                CellType.Empty => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void AddSumOfWeights(List<Tile> pathKey)
        {
            var weightValue = pathKey.Sum(FindWeight);
            _pathDictionary.Add(pathKey, weightValue);
        }

        //take pathes weights, and choose best one
        private void FindBestPath()
        {
            var maxValue = int.MinValue;
            var gridSize = _field.GetFieldSize;
            var tileList = (new List<Tile>(_field.GetTileList));
            var path = new List<Tile>();
            var candidateIndexes = new List<int>();
            var count = -1;

            foreach (var candidate in _pathDictionary.Where(candidate => candidate.Key.Count < AiSpeed))
            {
                _pathDictionary.Remove(candidate.Key);
            }
            
            
            foreach (var candidate in _pathDictionary)
            {
                foreach (var tile in tileList)
                {
                    count++;
                    if (tile == candidate.Key.Last()) break;
                }
                
                FindCandidates(candidateIndexes, gridSize, count);

                var goodTiles 
                    = candidateIndexes.Count(index => tileList[index].StateMachine.CurrentState != tileList[index].DisabledState);

                if (goodTiles == 0)
                    _pathDictionary.Remove(candidate.Key);

                count = -1;
            }

            foreach (var entry in _pathDictionary.Where(entry
                         => entry.Value > maxValue))
            {
                maxValue = entry.Value;
                path = entry.Key;
            }

            CurrentWay = new();
            CurrentWay = path;
            _currentAiBaseEnemy.CurrentWay = path;
            _lastTilesToFindNewPath = new();
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
        }

        private void AddWeightToList(IEnumerable<int> arrayFirst, IEnumerable<int> arraySecond)
        {
            CurrentWeights.AddRange(arrayFirst);
            NextStanceWeights.AddRange(arraySecond);
        }

        private IEnumerator MovePathRoutine()
        {
            var currentStep = 0;

            while (currentStep < CurrentWay.Count)
            {
                if (CurrentWay[currentStep].StateMachine.CurrentState != CurrentWay[currentStep].AvailableForSelectionState)
                {
                    CurrentWay[currentStep].StateMachine.ChangeState(CurrentWay[currentStep].AvailableForSelectionState);
                }
                
                _currentAiBaseEnemy.MakeStep();
                //todo - addEffects
                yield return new WaitForSeconds(.7f);
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
            var _turnButton = FindObjectOfType<NextTurnButton>();
            _turnButton.Touch();
            _pathDictionary.Clear();


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
            _nextTurnButton.onButtonPressed -= GiveAiTurn;
        }
    }
}