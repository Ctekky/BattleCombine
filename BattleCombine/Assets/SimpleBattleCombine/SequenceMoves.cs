using BattleCombine.Gameplay;

public class SequenceMoves
{
    private Player _currentPlayer;
    private Player _nextPlayer;

    public Player CurrentPlayer => _currentPlayer;
    public Player NextPlayer => _nextPlayer;

    public SequenceMoves(Player currentPlayer, Player nextPlayer)
    {
        _currentPlayer = currentPlayer;
        _nextPlayer = nextPlayer;
    }

    public void Next()
    {
        (_currentPlayer, _nextPlayer) = (_nextPlayer, _currentPlayer);
    }
}