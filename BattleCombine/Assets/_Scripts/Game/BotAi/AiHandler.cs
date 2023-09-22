using System;
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
        public static Action MakeAiTurn;
        public static Action FindAiPath;

        [field: SerializeField] private AiArchetypes currentArchetype { get; set; }

        [Header("Weights and other")] 
        [SerializeField] private int[] tankFullHealthWeights;
        [SerializeField] private int[] tankDamagedWeights;
        [SerializeField] private int tankHealthToChangeMood;
        [SerializeField] private int[] attackFullHealthWeights;
        [SerializeField] private int[] attackDamagedWeights;
        [SerializeField] private int attackHealthToChangeMood;
        [SerializeField] private int[] balanceFullHealthWeights;
        [SerializeField] private int[] balanceDamagedWeights;
        [SerializeField] private int balanceHealthToChangeMood;

        //todo - separate weights to data base and link it here
        private Dictionary<List<Tile>, int> pathDictionary = new();
        private Random _rand;
        private EnemyAi _currentEnemy;
        private CreateField _field;
        private int _maxOwnedTiles;
        public List<int> CurrentWeights { get; private set; }
        public List<Tile> CurrentWay { get; private set; }
        public int GetMoodHealthPercent { get; private set; }
        public int AiSpeed { get; private set; }
        public int Rounds { get; private set; }

        private void OnValidate()
        {
            _field = FindObjectOfType<CreateField>();
        }

        private void Start()
        {
            MakeAiTurn += RunAiPathMove;
        }

        [ContextMenu("START")]
        private void RunAiPathMove()
        {
            _maxOwnedTiles = AiSpeed * Rounds;
            CurrentWeights = new();
            ChooseArchetype();
            FindAllPaths();
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
            AiSpeed = 4;
            var count = -1;
            foreach (var tile in _field.GetTileList)
            {
                count++;
                if (tile != _field.GetAiStartTile) continue;
                FindPathsFromTile(count);
                break;
            }
        }

        //find path, and if its done - add to dict
        private void FindPathsFromTile(int startIndex)
        {
            var tileList = (new List<Tile>(_field.GetTileList));
            
            var gridSize = _field.GetFieldSize;
            var newPath = new List<Tile>();
            var currentIndex = startIndex;
            
            for(var i = 0; i < AiSpeed; i++) {
                newPath.Add(tileList[currentIndex]);

               var candidateIndexes = new List<int>();
        
               //Add tile from Left
               if(currentIndex % gridSize != 0) 
                   candidateIndexes.Add(currentIndex-1);
               //Add tile from Right
               if(currentIndex % gridSize != gridSize-1) 
                   candidateIndexes.Add(currentIndex+1);
               //Add tile from Top
               if(currentIndex >= gridSize) 
                   candidateIndexes.Add(currentIndex-gridSize);
               //Add tile from Bottom
               if(currentIndex < gridSize * (gridSize-1)) 
                   candidateIndexes.Add(currentIndex+gridSize);

               //Choose the one with the maximum weight
               //todo - link the weights
               var maxWeight = -1;
               foreach(var index in candidateIndexes) {
                   var weight = FindWeight(tileList[index]);
                   if (weight <= maxWeight || newPath.Contains(tileList[index])) continue;
                   maxWeight = weight;
                   currentIndex = index;
               }
            }
            
            //todo - change path count to its weight
            pathDictionary.Add(newPath, newPath.Count);
            CurrentWay = newPath;
            _currentEnemy.Init();

            //foreach (var nextTile in nextTiles.OrderBy(x => Guid.NewGuid()))
            //{
            //    var newPath = new List<Tile>(path) { nextTile };
//
            //    if (newPath.Count == AiSpeed)
            //    {
            // //    
            //        pathDictionary.Add(newPath, newPath.Count);
            //        Debug.Log("done");
            //    }
            //    else
            //    {
            //        FindPathsFromTile(newPath);
            //    }
            //}
        }//
        
        private int FindWeight(Tile tile)
        {
            return tile.GetTileType switch
            {
                CellType.Attack => CurrentWeights[0],
                CellType.Health => CurrentWeights[1],
                CellType.Shield => CurrentWeights[2],
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        //Get tiles near current tile (list is empty now :'))
        //private IEnumerable<Tile> GetNextTiles(Tile currentTile, int startIndex)
        //{
        //    //var adjacentTiles
        //    //    = ConvertTileList(currentTile.TilesNearThisTile);
        //    return null;
        //}

        //Convert list<GameObject> to list<Tile>
        //private IEnumerable<Tile> ConvertTileList(IEnumerable<GameObject> oldList)
        //{
        //    var newList = oldList.Select(obj
        //        => obj.GetComponent<Tile>()).ToList();
        //    return newList;
        //}

        //todo - no pathes, no best pathes 0))
        //take pathes weights, and choose best one
        private void FindBestPath()
        {
            var maxPosition = -1;
            var maxValue = int.MinValue;

            foreach (var entry in pathDictionary.Where(entry
                         => entry.Value > maxValue))
            {
                maxValue = entry.Value;
                maxPosition = pathDictionary.Keys.ToList().IndexOf(entry.Key);
            }
        }

        private void ApplyAiArchetype(AiArchetypes enemyType)
        {
            switch (enemyType)
            {
                case AiArchetypes.Tank:
                    AddWeightToList(tankFullHealthWeights, tankDamagedWeights);
                    GetMoodHealthPercent = tankHealthToChangeMood;
                    _currentEnemy = new TankAi();
                    break;
                case AiArchetypes.Attack:
                    AddWeightToList(attackFullHealthWeights, attackDamagedWeights);
                    GetMoodHealthPercent = attackHealthToChangeMood;
                    _currentEnemy = new AttackAi();
                    break;
                case AiArchetypes.Balance:
                    AddWeightToList(balanceFullHealthWeights, balanceDamagedWeights);
                    GetMoodHealthPercent = balanceHealthToChangeMood;
                    _currentEnemy = new BalanceAi();
                    break;
                case AiArchetypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _currentEnemy._aiHandler = this;
        }

        private void AddWeightToList(IEnumerable<int> arrayFirst, IEnumerable<int> arraySecond)
        {
            CurrentWeights.AddRange(arrayFirst);
            CurrentWeights.AddRange(arraySecond);
        }
    }
}