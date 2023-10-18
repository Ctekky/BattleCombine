using BattleCombine.ScriptableObjects;
using System;
using UnityEngine;

[Serializable]
public class AssetsData
{

    [Header("StatsIcon")]
    public Sprite iconSword;
    public Sprite iconHeart;
    public Sprite iconShild;
    public Sprite iconSpeed;

    [Header("ScriptableObject")]
    public TileType swordTile;
    public TileType heartTile;
    public TileType shieldTile;

    public GettingAsset<Sprite> icons;
    public GettingAsset<Sprite> portraits;
    public GettingAsset<Sprite> bg;

    public GettingAsset<AudioClip> music;
    public GettingAsset<AudioClip> sfx;

   



}
