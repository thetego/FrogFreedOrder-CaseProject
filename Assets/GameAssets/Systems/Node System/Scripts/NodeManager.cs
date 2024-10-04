using System.Collections.Generic;
using UnityEngine;

public class NodeManager : Singleton<NodeManager>
{
    private Node[,] grid;
    private List<Node> _nodes =new List<Node>();
    [SerializeField] private Node nodePrefab;
    [SerializeField] private int rows = 4;
    [SerializeField] private int  columns = 4;
    [SerializeField] float spacing = 1.5f;
    

    public void GenerateNodes()
    {
        grid = new Node[rows, columns];

        float totalWidth = (columns - 1) * spacing;
        float totalHeight = (rows - 1) * spacing;
        
        Vector3 gridOffset = new Vector3(totalWidth / 2, 0, totalHeight / 2);

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = new Vector3(column * spacing, 0, -row * spacing);
                Vector3 centeredPosition = position - gridOffset;

                Node node = Instantiate(nodePrefab.gameObject, centeredPosition, Quaternion.identity, transform).GetComponent<Node>();
                node.SetRowAndColumn(row,column);

                _nodes.Add(node);
                grid[row, column] = node;
            }
        }
    }
    
    public void ClearNodes()
    {
        for (int i = 0; i < _nodes.Count; i++)
        {
            _nodes[i].Deactivate();
        }
        _nodes.Clear();
    }
    
    public Node GetNeighbor(Node currentNode, Vector3 direction)
    {
        int row = currentNode.GetRow;
        int col = currentNode.GetColumn;

        int neighborRow = row + (direction == Vector3.forward ? -1 : direction == Vector3.back ? 1 : 0);
        int neighborCol = col + (direction == Vector3.left ? -1 : direction == Vector3.right ? 1 : 0);

        if (neighborRow >= 0 && neighborRow < rows && neighborCol >= 0 && neighborCol < columns)
        {
            return grid[neighborRow, neighborCol];
        }

        return null; // Return null if no valid neighbor is found
    }
    
    public bool IsBorderNode(Node currentNode)
    {
        int row = currentNode.GetRow;
        int col = currentNode.GetColumn;

        return row == 0 || row == rows - 1 || col == 0 || col == columns - 1;
    }
    
    public List<Node> GetNodes()
    {
        return _nodes;
    }
}
