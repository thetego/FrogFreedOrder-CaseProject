public class MoveRightModel
{
    public int TotalMoves { get; private set; }
    public int RemainingMoves { get; private set; }

    public MoveRightModel(int totalMoves)
    {
        TotalMoves = totalMoves;
        RemainingMoves = totalMoves;
    }

    public bool CanMove()
    {
        return RemainingMoves > 0;
    }

    public void UseMove()
    {
        if (CanMove())
        {
            RemainingMoves--;
        }
    }

    public void ResetMoves()
    {
        RemainingMoves = TotalMoves;
    }
}
