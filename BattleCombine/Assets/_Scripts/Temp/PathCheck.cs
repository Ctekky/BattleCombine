using BattleCombine.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BattleCombine.Gameplay
{
    public class PathCheck
    {
        public bool FindPath(GameObject startTileGO, int playerSpeed)
        {
            var stackPath = new Stack<(GameObject currentTileGO, List<GameObject> currentPath, int remainingSpeed)>();

            stackPath.Push((startTileGO, new List<GameObject> {startTileGO, }, playerSpeed - 1));

            while (stackPath.Count > 0)
            {
                var (currentTileGO, currentPath, remainingSpeed) = stackPath.Pop();

                if(remainingSpeed <= 0)
                {
                    return true;
                }

                List<GameObject> tilesGONearCurrentTile = currentTileGO.GetComponent<Tile>().TilesNearThisTile;
                foreach (var tileGO in tilesGONearCurrentTile)
                {
                    int i = 0;
                    Tile tile = tileGO.GetComponent<Tile>();
                    if (tile.GetTileState != TileState.EnabledState || currentPath.Contains(tileGO))
                    {
                        if(i == tilesGONearCurrentTile.Count)
                        {
                            return false;
                        }
                        i++;
                        continue;
                    }
                    var newPath = new List<GameObject>(currentPath) { tileGO, };

                    stackPath.Push((tileGO, newPath, remainingSpeed - 1));
                }
            }
            return false;
        }
    }
}
