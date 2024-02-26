namespace BattleCombine.Enums
{
    public enum CellType : byte
    {
        Attack,
        Health,
        Shield,
        Empty,
        Void
    }
    public enum FieldSize : byte
    {  
        XTwoTiles,
        XThreeTiles,
        XFourTiles,
        XFiveTiles,
        XSixTiles,
        XSevenTiles,
        XEightTiles,
    }
    public enum StatsEnum : byte
    {
        Health,
        Attack,
        Shield,
    }

    public enum TileState : byte
    {
        AvailableForSelectionState,
        ChosenState,
        DisabledState,
        EnabledState,
        FinalChoiceState,
    }
    public enum IDPlayer : byte
    {
        Player1,
        Player2,
        AIPlayer
    }

    public enum TileModifier : byte
    {
        MinusNine,
        MinusEight,
        MinusSeven,
        MinusSix,
        MinusFive,
        MinusFour,
        MinusThree,
        MinusTwo,
        MinusOne,
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine
    }
    public enum InputMod : byte
    {
        Touch,
        TouchAndMove
    }
    public enum GameMod : byte
    {
        ArcadeMod,
        StoryMod,
        PvPMod

    }
}