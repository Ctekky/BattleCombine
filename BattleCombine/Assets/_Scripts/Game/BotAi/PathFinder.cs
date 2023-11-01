using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;
using UnityEngine;

namespace BattleCombine.Ai
{
    public class PathFinder
    {
        public List<int> CurrentWay { get; private set; } = new ();
        public List<int> CurrentWeights { get; set; } = new ();
        public List<int> NextStanceWeights { get; set; } = new ();
        public AiBaseEnemy CurrentAiBaseEnemy { get; set; }

        private int _aiSpeed;
        private int _pathID;
        private int _gridSize;
        private CreateField _field;
        private AiHandler _aiHandler;
        private List<int> _lastTilesToFindNewPath = new ();

        private readonly Dictionary<int, List<int>> allPaths;

        public PathFinder(int aiSpeed, CreateField field, AiHandler aiHandler)
        {
            _aiSpeed = aiSpeed;
            _field = field;
            _aiHandler = aiHandler;
            _gridSize = _field.GetFieldSize;
            _pathID = 0;

            allPaths = new Dictionary<int, List<int>>();
        }

        public void FindStartPath()
        {
            var count = -1;

            CurrentWay = new List<int>();

            foreach (var tile in _field.GetTileList)
            {
                count++;
                if (tile != _field.GetAiStartTile) continue;
                FindAllPathsFromStart(count);
                break;
            }
        }

        public void FindAllPathsFromStart(int startIndex)
        {
            allPaths.Clear();

            FindPaths(startIndex, _aiSpeed);
            
            ChooseBestPath();
        }
        
        public void KeepLastPathStarts(int currentIndex)
        {
            var tileList = (new List<Tile>(_field.GetTileList));
            var gridSize = _field.GetFieldSize;
            
            var candidateIndexes  = FindCandidates(currentIndex);

            foreach (var index in candidateIndexes.Where(index
                         => tileList[index].StateMachine.CurrentState != tileList[index].DisabledState))
            {
                _lastTilesToFindNewPath.Add(index);
            }

            if (_lastTilesToFindNewPath == null) return;
            {
                foreach (var index in _lastTilesToFindNewPath)
                {
                    FindPaths(index, _aiSpeed);
                }
                
                
                ChooseBestPath();
            }
            _lastTilesToFindNewPath.Clear();
        }

        private void FindPaths(int startIndex, int speed)
        {
            Stack<(int currentIndex, List<int> currentPath, int remainingSpeed)> stack = new();

            stack.Push((startIndex, new List<int> { startIndex }, speed-1));

            while (stack.Count > 0)
            {
                var (currentIndex, currentPath, remainingSpeed) = stack.Pop();

                if (remainingSpeed <= 0)
                {
                    AddPathToDictionary(new List<int>(currentPath));
                    continue;
                }

                var neighborIndices = FindCandidates(currentIndex);

                foreach (var neighborIndex in neighborIndices)
                {
                    if (_field.GetTileList[neighborIndex].StateMachine.CurrentState
                        == _field.GetTileList[neighborIndex].ChosenState
                        && _field.GetTileList[neighborIndex].StateMachine.CurrentState
                        == _field.GetTileList[neighborIndex].DisabledState) continue;
                    
                    if(currentPath.Contains(neighborIndex)) continue;
                    
                    var newPath = new List<int>(currentPath) { neighborIndex };
                    stack.Push((neighborIndex, newPath, remainingSpeed - 1));
                }
            }
        }
        
        private void ChooseBestPath()
        {
            Debug.Log(allPaths.Count + " - Available paths");
            List<int> chosenList = new();
            
            var maxWeight = int.MinValue;
            var minNegativeModifier = int.MaxValue;

            foreach (var kvp in allPaths)
            {
                var sumWeight = kvp.Value.Sum(GetTileWeight);
                var negativeModifier = kvp.Value.Min(tile => _field.GetTileList[tile].TileModifier);

                if (sumWeight <= maxWeight &&
                    (sumWeight != maxWeight || negativeModifier <= minNegativeModifier)) continue;
                
                chosenList = kvp.Value;
                maxWeight = sumWeight;
                minNegativeModifier = negativeModifier;
            }
            
            CurrentWay.Clear();
            CurrentWay = chosenList;
            CurrentAiBaseEnemy.CurrentWay.Clear();
            CurrentAiBaseEnemy.CurrentWay = MakeTileList(chosenList);
            allPaths.Clear();

            PathDebugger(maxWeight);
        }

        private void PathDebugger(int maxWeight)
        {
            Debug.Log("===================================" +
                      " \n #Current Archetype = " + _aiHandler.GetCurrentArchetype);
            Debug.Log("Defence stance activated: " + CurrentAiBaseEnemy.GetStance);
            Debug.Log("Sum of path weight: " + maxWeight);
            foreach (var index in CurrentWay)
            {
                Debug.Log(index + " (weight = " + GetTileWeight(index) +"): " +
                          "Tile type = " + _field.GetTileList[index].GetTileType);
            }
            Debug.Log($"==================================");
        }

        private int GetTileWeight(int tile)
        {
            return _field.GetTileList[tile].GetTileType switch
            {
                CellType.Attack => CurrentWeights[0],
                CellType.Health => CurrentWeights[1],
                CellType.Shield => CurrentWeights[2],
                CellType.Empty => 0,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        
        private List<int> FindCandidates(int currentIndex)
        {
            var candidateIndexes = new List<int>();

            //Add tile from Left
            if (currentIndex % _gridSize != 0)
                candidateIndexes.Add(currentIndex - 1);
            //Add tile from Right
            if (currentIndex % _gridSize != _gridSize - 1)
                candidateIndexes.Add(currentIndex + 1);
            //Add tile from Top
            if (currentIndex >= _gridSize)
                candidateIndexes.Add(currentIndex - _gridSize);
            //Add tile from Bottom
            if (currentIndex < _gridSize * (_gridSize - 1))
                candidateIndexes.Add(currentIndex + _gridSize);

            return candidateIndexes;
        }

        private void AddPathToDictionary(List<int> path)
        {
            var count = 0;
            
            if (path.Count == _aiSpeed)
            {
                allPaths.Add(_pathID++, path);
            }
        }

        private List<Tile> MakeTileList(IEnumerable<int> tempList)
        {
            var finalList = tempList.Select(tile => _field.GetTileList[tile]).ToList();

            return finalList;
        }
    }
}