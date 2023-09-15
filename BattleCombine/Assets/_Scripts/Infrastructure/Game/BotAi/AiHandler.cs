using System;
using System.Collections.Generic;
using System.Linq;
using _Scripts;
using BattleCombine.Enums;
using BattleCombine.Interfaces;
using UnityEngine;
using Random = System.Random;

namespace BattleCombine.Ai
{
    public class AiHandler : MonoBehaviour
    {
        public static Action MakeAiTurn;
        public static Action FindAiPath;

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
        private Dictionary<List<_Scripts.Tile>, int> pathDictionary = new();
        private Random _rand;
        private EnemyAi currentEnemy;
        private int maxOwnedTiles;
        public List<int> CurrentWeights { get; private set; }
        public List<_Scripts.Tile> CurrentWay { get; private set; }
        public int GetMoodHealthPercent { get; private set; }
        public int AiSpeed { get; private set; }
        public int Rounds { get; private set; }

        private void Start()
        {
            maxOwnedTiles = AiSpeed * Rounds;
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
            AiSpeed = 5;
            foreach (var tile in CreateField.GetTileList.OrderBy(x => Guid.NewGuid()))
            {
                if (tile != CreateField.GetAiStartTile) continue;
                //todo - ChangeState for Chosen
                Debug.Log(tile);
                
                //tile.StateMachine.ChangeState(tile.ChosenState);
                //if (tile.GetComponent<ITouchable>() != null)
                //{
                //    tile.GetComponent<ITouchable>().Touch();
                //}
                
                FindPathsFromTile(new List<_Scripts.Tile> { tile });
            }
        }

        //find path, and if its done - add to dict
        private void FindPathsFromTile(IReadOnlyCollection<_Scripts.Tile> path)
        {
            var nextTiles = GetNextTiles(path.Last());

            foreach (var nextTile in nextTiles.OrderBy(x => Guid.NewGuid()))
            {
                var newPath = new List<_Scripts.Tile>(path) { nextTile };

                if (newPath.Count == AiSpeed)
                {
                    //todo - change path count to its weight
                    pathDictionary.Add(newPath, newPath.Count);
                    Debug.Log("done");
                }
                else
                {
                    FindPathsFromTile(newPath);
                }
            }
        }

        //Get tiles near current tile (list is empty now :'))
        private IEnumerable<_Scripts.Tile> GetNextTiles(_Scripts.Tile currentTile)
        { 
            var adjacentTiles
                = ConvertTileList(currentTile.TilesNearThisTile);
            return adjacentTiles.Where(t => CanMoveToTile(currentTile, t)).ToList();
        }

        //Convert list<GameObject> to list<Tile>
        private IEnumerable<_Scripts.Tile> ConvertTileList(IEnumerable<GameObject> oldList)
        {
            var newList = oldList.Select(obj
                => obj.GetComponent<_Scripts.Tile>()).ToList();
            return newList;
        }

        //Temporary patch;
        //todo - return tile no after bool check, but Only after checking the weights
        private bool CanMoveToTile(_Scripts.Tile currentTile, _Scripts.Tile nextTile)
        {
            return true;
        }

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
                    currentEnemy = new TankAi();
                    break;
                case AiArchetypes.Attack:
                    AddWeightToList(attackFullHealthWeights, attackDamagedWeights);
                    GetMoodHealthPercent = attackHealthToChangeMood;
                    currentEnemy = new AttackAi();
                    break;
                case AiArchetypes.Balance:
                    AddWeightToList(balanceFullHealthWeights, balanceDamagedWeights);
                    GetMoodHealthPercent = balanceHealthToChangeMood;
                    currentEnemy = new BalanceAi();
                    break;
                case AiArchetypes.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            currentEnemy._aiHandler = this;
            currentEnemy.Init();
        }

        private void AddWeightToList(IEnumerable<int> arrayFirst, IEnumerable<int> arraySecond)
        {
            CurrentWeights.AddRange(arrayFirst);
            CurrentWeights.AddRange(arraySecond);
        }
    }
}