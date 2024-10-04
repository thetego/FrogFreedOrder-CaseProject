using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Funrado-Case-Project/LevelData")]
public class LevelData : ScriptableObject
{   
    public int moveLimit;
    public List<GridLayer> layers = new List<GridLayer>();
}

[System.Serializable]
public class GridLayer
{
    public int layerNumber;
    
    public List<GridCellData> cells = new List<GridCellData>();

    public GridLayer(int layerNumber, int rows, int columns)
    {
        this.layerNumber = layerNumber;
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                cells.Add(new GridCellData(row, col));
            }
        }
    }
}
[System.Serializable]
public class GridCellData
{
    public int row;
    public int column;
    public ColorType color = ColorType.None;
    public CellType type = CellType.Empty;
    public Direction direction = Direction.None;
    public GridCellData(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    public GameElement DetermineCellContent()
    {
        GameElement result = null;
        if (type.Equals(CellType.Frog))
        {
            switch (color)
            {
                case ColorType.Blue: result= Resources.Load<GameElement>(Constants.Prefabs.BlueFrog);break;
                case ColorType.Red: result= Resources.Load<GameElement>(Constants.Prefabs.RedFrog);break;
                case ColorType.Green: result= Resources.Load<GameElement>(Constants.Prefabs.GreenFrog);break;
                case ColorType.Yellow: result= Resources.Load<GameElement>(Constants.Prefabs.YellowFrog);break;
            }
        }
        else if (type.Equals(CellType.Berry))
        {
            switch (color)
            {
                case ColorType.Blue: result= Resources.Load<GameElement>(Constants.Prefabs.BlueGrape); break;
                case ColorType.Red: result= Resources.Load<GameElement>(Constants.Prefabs.RedGrape);break;
                case ColorType.Green: result= Resources.Load<GameElement>(Constants.Prefabs.GreenGrape);break;
                case ColorType.Yellow: result= Resources.Load<GameElement>(Constants.Prefabs.YellowGrape);break;
            }
        }
        else if (type.Equals(CellType.Arrow))
        {
            switch (color)
            {
                case ColorType.Blue: result= Resources.Load<GameElement>(Constants.Prefabs.BlueArrow);break;
                case ColorType.Red: result= Resources.Load<GameElement>(Constants.Prefabs.RedArrow);break;
                case ColorType.Green: result= Resources.Load<GameElement>(Constants.Prefabs.GreenArrow);break;
                case ColorType.Yellow: result= Resources.Load<GameElement>(Constants.Prefabs.YellowArrow);break;
            }
        }
        return result;
    }
    public Vector3 DetermineDirection()
    {
        Vector3 dir=Vector3.zero;
        switch(direction)
        {
            case Direction.Right: dir = Vector3.right ;break;
            case Direction.Up: dir = Vector3.forward ;break;
            case Direction.Down: dir = Vector3.back ;break;
            case Direction.Left: dir = Vector3.left ;break;
            case Direction.None: dir = Vector3.zero ;break;
        }
        return dir;
    }
}
