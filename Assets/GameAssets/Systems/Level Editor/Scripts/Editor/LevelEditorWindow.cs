using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

public class LevelEditorWindow : EditorWindow
{
    private int rows = 4;
    private int columns = 4;
    private int selectedLayer = 0;
    
    private int _moveLimit;
    private GridCell[,] grid;
    private GridCell selectedCell;
    private Dictionary<int, GridCell[,]> layers = new Dictionary<int, GridCell[,]>();

    private LevelData levelData;
    

    [MenuItem("Tools/LevelEditor")]
    public static void ShowWindow()
    {
        GetWindow<LevelEditorWindow>("LevelEditor");
    }

    private void OnEnable()
    {
        grid = new GridCell[rows, columns];
        InitializeLayer(0);
    }

    private void InitializeLayer(int layer)
    {
        if (!layers.ContainsKey(layer))
        {
            layers[layer] = new GridCell[rows, columns];
        }
        
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                if (layers[layer][row, col] == null)
                {
                    layers[layer][row, col] = new GridCell();
                }
            }
        }
    }
    private void OnGUI()
    {
        GUILayout.Label("Grid Settings", EditorStyles.boldLabel);
        rows = EditorGUILayout.IntField("Rows", rows);
        columns = EditorGUILayout.IntField("Columns", columns);
        _moveLimit = EditorGUILayout.IntField("Move Limit", _moveLimit);

        GUILayout.Space(10);

        GUILayout.Label("Layer Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("Add New Layer"))
        {
            int newLayer = layers.Count;
            InitializeLayer(newLayer);
            selectedLayer = newLayer;
        }

        if (GUILayout.Button("Remove Current Layer"))
        {
            if (layers.Count > 1)
            {
                layers.Remove(selectedLayer);
                selectedLayer = Mathf.Max(0, selectedLayer - 1);
            }
        }

        selectedLayer = EditorGUILayout.IntSlider("Selected Layer", selectedLayer, 0, layers.Count - 1);

        DrawGrid();
        ShowCellOptions(selectedCell);

        GUILayout.Space(20);
        GUILayout.Label("Save/Load", EditorStyles.boldLabel);

        levelData = (LevelData)EditorGUILayout.ObjectField("Level Data", levelData, typeof(LevelData), false);

        if (GUILayout.Button("Save Level"))
        {
            if (levelData != null)
            {
                SaveLevel();
                EditorUtility.SetDirty(levelData); // Mark the ScriptableObject as dirty so Unity saves the changes
            }
        }

        if (GUILayout.Button("Load Level"))
        {
            if (levelData != null)
            {
                LoadLevel();
            }
        }
    }

    private void DrawGrid()
    {
        if (!layers.ContainsKey(selectedLayer))
            return;

        for (int row = 0; row < rows; row++)
        {
            GUILayout.BeginHorizontal();
            for (int col = 0; col < columns; col++)
            {
                GridCell cell = layers[selectedLayer][row, col];
                
                switch (cell.color)
                {
                    case ColorType.None:
                        GUI.backgroundColor = Color.white;
                        break;
                    case ColorType.Blue:
                        GUI.backgroundColor = Color.blue;
                        break;
                    case ColorType.Yellow:
                        GUI.backgroundColor = Color.yellow;
                        break;
                    case ColorType.Green:
                        GUI.backgroundColor = Color.green;
                        break;
                    case ColorType.Red:
                        GUI.backgroundColor = Color.red;
                        break;
                }

                if (GUILayout.Button(cell.type.ToString()+"\n"+cell.direction.ToString(), GUILayout.Width(70), GUILayout.Height(70)))
                {
                    selectedCell = cell;
                }
            }
            GUILayout.EndHorizontal();
        }
        GUI.backgroundColor = Color.white;
    }

    private void ShowCellOptions(GridCell cell)
    {
        if (cell == null)
            return;

        GUILayout.Space(10);
        GUILayout.Label("Cell Properties", EditorStyles.boldLabel);

        cell.color = (ColorType)EditorGUILayout.EnumPopup("Color", cell.color);
        cell.type = (CellType)EditorGUILayout.EnumPopup("Type", cell.type);
        cell.direction = (Direction)EditorGUILayout.EnumPopup("Direction", cell.direction);

        if (GUILayout.Button("Apply"))
        {
            // Apply changes to the cell
            selectedCell = null; // Deselect the cell after applying changes
        }
    }

    private void SaveLevel()
    {
        levelData.layers.Clear(); // Clear existing layers

        foreach (var layerEntry in layers)
        {
            int layerNumber = layerEntry.Key;
            GridCell[,] gridCells = layerEntry.Value;

            GridLayer newLayer = new GridLayer(layerNumber, rows, columns);

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    GridCell cell = gridCells[row, col];
                    GridCellData cellData = newLayer.cells.Find(c => c.row == row && c.column == col);

                    cellData.color = cell.color;
                    cellData.type = cell.type;
                    cellData.direction = cell.direction;
                }
            }
            levelData.moveLimit = _moveLimit;
            levelData.layers.Add(newLayer);
        }
    }
    private void LoadLevel()
    {
        layers.Clear(); // Clear existing layers

        foreach (var layer in levelData.layers)
        {
            GridCell[,] gridCells = new GridCell[rows, columns];
            InitializeLayer(layer.layerNumber);

            foreach (var cellData in layer.cells)
            {
                GridCell cell = new GridCell
                {
                    color = cellData.color,
                    type = cellData.type,
                    direction = cellData.direction
                };

                gridCells[cellData.row, cellData.column] = cell;
            }

            layers[layer.layerNumber] = gridCells;
        }
        _moveLimit = levelData.moveLimit;
        selectedLayer = 0; // Reset to the first layer
    }
}

