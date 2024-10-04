
public enum CellType { Empty, Frog, Arrow, Berry }
public enum Direction { None, Right, Left, Up, Down }
public enum ColorType { None, Blue, Yellow, Green, Red }

[System.Serializable]
public class GridCell
{
    public ColorType color = ColorType.None;
    public CellType type = CellType.Empty;
    public Direction direction = Direction.None;


}
