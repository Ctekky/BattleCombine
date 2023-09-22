namespace BattleCombine.Enums
{
    public enum CellType : byte
    {
        Attack,
        Health,
        Shield,
        Empty,
    }
    public enum FieldSize : byte
    {
        Small,
        Medium,
        Large,
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
}