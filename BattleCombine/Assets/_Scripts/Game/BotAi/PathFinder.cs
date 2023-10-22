using System;
using System.Collections.Generic;
using System.Linq;
using BattleCombine.Enums;
using BattleCombine.Gameplay;

namespace BattleCombine.Ai
{
    public class PathFinder
    {
        public CreateField Field { get; set; }
        public AiHandler AiHandler { get; set; }
        public List<Tile> CurrentWay { get; set; }
        public List<int> CurrentWeights { get; set; }
        public List<int> NextStanceWeights { get; set; }
        public AiBaseEnemy CurrentAiBaseEnemy { get; set; }
        public int AiSpeed { get; set; }

        private List<int> _lastTilesToFindNewPath = new();

        //todo - separate weights to data base and link it here
        public Dictionary<List<Tile>, int> PathDictionary = new();

        //find all exist pathes for current AiSpeed
        public void FindStartPath()
        {
            //todo - find path and write it to dict
            var count = -1;

            foreach (var tile in Field.GetTileList)
            {
                count++;
                if (tile != Field.GetAiStartTile) continue;
                FindPathsFromTile(count);
                break;
            }
        }

        public void KeepLastPathStarts(int currentIndex)
        {
            var tileList = (new List<Tile>(Field.GetTileList));
            var gridSize = Field.GetFieldSize;
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

        //find path, and if its done - add to dict
        private void FindPathsFromTile(int startIndex)
        {
            var tileList = (new List<Tile>(Field.GetTileList));
            var gridSize = Field.GetFieldSize;
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

                    if (newPath.Contains(tileList[index])) continue;

                    maxWeight = weight;
                    currentIndex = index;
                }
            }

            //todo - change path count to its weight
            if (newPath.Count < AiSpeed)
                return;

            AddSumOfWeights(newPath);
            
            if (CurrentWay != null) return;
            CurrentWay = newPath;
            AiHandler.CurrentWay = newPath;
        }

        //take pathes weights, and choose best one
        private void FindBestPath()
        {
            var maxValue = int.MinValue;
            var gridSize = Field.GetFieldSize;
            var tileList = (new List<Tile>(Field.GetTileList));
            var path = new List<Tile>();
            var candidateIndexes = new List<int>();
            var count = -1;

            foreach (var candidate in PathDictionary.Where(candidate => candidate.Key.Count < AiSpeed))
            {
                PathDictionary.Remove(candidate.Key);
            }


            foreach (var candidate in PathDictionary)
            {
                foreach (var tile in tileList)
                {
                    count++;
                    if (tile == candidate.Key.Last()) break;
                }

                FindCandidates(candidateIndexes, gridSize, count);

                var goodTiles
                    = candidateIndexes.Count(index =>
                        tileList[index].StateMachine.CurrentState != tileList[index].DisabledState);

                if (goodTiles == 0)
                    PathDictionary.Remove(candidate.Key);

                count = -1;
            }

            foreach (var entry in PathDictionary.Where(entry
                         => entry.Value > maxValue))
            {
                maxValue = entry.Value;
                path = entry.Key;
            }

            CurrentWay = new List<Tile>();
            CurrentWay = path;
            AiHandler.CurrentWay = new List<Tile>();
            AiHandler.CurrentWay = path;
            CurrentAiBaseEnemy.CurrentWay = new List<Tile>();
            CurrentAiBaseEnemy.CurrentWay = path;
            _lastTilesToFindNewPath = new List<int>();
        }

        private static void FindCandidates(List<int> candidateIndexes, int gridSize, int currentIndex)
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
            PathDictionary.Add(pathKey, weightValue);
        }
    }
}