using System;
using BattleCombine.Enums;

[Serializable]
public class TileData
{
    public int position;
    public TileState tileCurrentState;
    public int tileModifier;
    public int tileTypeID;
    public bool isStartTile;

}
