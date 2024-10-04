using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;

public abstract class CellBase : MonoBehaviour
{

    public Node currentNode;
    public GridCellData _cellData;
    public GameElement cellContent;
    public Transform tonguePos;

    private Vector3 _initialSize;

    void Awake()
    {
        //Activate();
    }
    public void AssignData(GridCellData data)
    {
        _cellData = data;
    }
    public virtual void Initialize(Node node)
    {
        currentNode = node;
        cellContent = Instantiate(_cellData.DetermineCellContent(), transform.position+(Vector3.up*.15f),Quaternion.identity);
        cellContent.transform.SetParent(this.transform);
        
        if (cellContent is Berry)
        {
            GameManager.Instance.AddBerry(cellContent as Berry);
        }
        
        if (_cellData.type.Equals(CellType.Arrow)||_cellData.type.Equals(CellType.Frog))
            SetContentOrientation();
        else 
            cellContent.transform.Rotate(new Vector3(0,-150,0));



        AssignCell();

    }
    public virtual void AssignCell()
    {
        
    }
    void SetContentOrientation()
    {
        float angle=0;

        switch (_cellData.direction)
        {
            case Direction.Right: angle = 270; break;
            case Direction.Left: angle = 90; break;
            case Direction.Up: angle = 180; break;
            case Direction.Down: angle = 0; break;
        }
        
        cellContent.transform.DOLocalRotate(Vector3.up*angle,.1f);
        
    }
    public void Activate()
    {
        _initialSize=transform.localScale;
        transform.localScale=Vector3.zero;
        transform.DOScale(_initialSize,.5f).SetEase(Ease.Linear).OnComplete(()=>transform.DOKill());
    }
    public void Deactivate()
    {
        transform.DOScale(Vector3.zero,1).SetEase(Ease.InExpo).OnComplete(()=>
        {
            currentNode.RemoveCell(this);
            LevelManager.Instance.CheckForLevelState();
            transform.DOKill();
            Destroy(gameObject);
        });
        
    }
}
