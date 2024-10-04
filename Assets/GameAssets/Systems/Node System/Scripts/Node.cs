using System.Collections.Generic;
using UnityEngine;

public class Node : NodeBase
{

    private int _row, _column;
    [SerializeField] private List<Cell> _cells;
    [SerializeField] private Cell _activeCell;

    public int GetColumn =>_column;
    public int GetRow =>_row;
    public Cell GetActiveCell => _activeCell;

    void Awake()
    {
        _cells = new List<Cell>();
        LevelManager.Instance.OnFinishCreatingLevel.AddListener(InitializeActiveCell);
    }

    public void AddCell(GridCellData data)
    {
        Cell newCell = Instantiate(GetCellPrefabByColor(data.color));
        newCell.AssignData(data);

        newCell.gameObject.transform.position = new Vector3
        (
            transform.position.x,
            _activeCell==null? transform.position.y : _activeCell.transform.position.y+.1f,
            transform.position.z
        );

        newCell.transform.SetParent(this.transform);
        _cells.Add(newCell);
        _activeCell = newCell;
    }
    
    public void InitializeActiveCell()
    {
        _activeCell.Initialize(this);
    }
    
    public Cell GetCellPrefabByColor(ColorType colorType)
    {
        switch (colorType)
        {
            case ColorType.Blue:
                 return Resources.Load<Cell>(Constants.Prefabs.BlueCell);
            case ColorType.Red:
                 return Resources.Load<Cell>(Constants.Prefabs.RedCell);
            case ColorType.Yellow:
                return Resources.Load<Cell>(Constants.Prefabs.YellowCell);
            case ColorType.Green:
                return Resources.Load<Cell>(Constants.Prefabs.GreenCell);
            default:
                return Resources.Load<Cell>(Constants.Prefabs.EmptyCell);
        }
    }
    
    public List<Cell> GetCells()
    {
        return _cells;
    }
    
    public void SetRowAndColumn(int row, int column)
    {
        _row=row;
        _column=column;
    }
    
    public void RemoveCell(CellBase cell)
    {
        _cells.Remove(cell as Cell);
        _activeCell = _cells[_cells.Count-1];
        LevelManager.Instance.CheckForLevelState();
        InitializeActiveCell();
    }

    private void OnDestroy()
    {
        LevelManager.Instance.OnFinishCreatingLevel.RemoveListener(InitializeActiveCell);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(transform.position,Vector3.one);
    }
    #endif
}
