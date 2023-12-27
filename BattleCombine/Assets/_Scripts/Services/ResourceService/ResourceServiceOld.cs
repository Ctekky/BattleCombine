using BattleCombine.ScriptableObjects;
using UnityEngine;

public class ResourceServiceOld
{
    private AssetsData data;
    public Sprite IconSword => data.iconSword;
    public Sprite IconHeart => data.iconHeart;
    public Sprite IconShild => data.iconShild;
    public Sprite IconSpeed => data.iconSpeed;

    public TileType SwordTile => data.swordTile;
    public TileType HeartTile => data.heartTile;
    public TileType ShieldTile => data.shieldTile;

    public GettingAsset<Sprite> Icons => data.icons;
    public GettingAsset<Sprite> Portraits => data.portraits;
    public GettingAsset<Sprite> BG => data.bg;

    public GettingAsset<AudioClip> Music => data.music;
    public GettingAsset<AudioClip> SFX => data.sfx;


    public ResourceServiceOld(AssetsData data)
    {
        this.data = data;
    }

    
}
