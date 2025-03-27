using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public int gridSize = 1000; // Grid size (100x100)
    public int gridWidth = 192;
    public int gridHeight = 108;
    public int cellSize = 10; // Each cell is 10x10 pixels
    private Cell[,] grid; // 2D array to store cells
    public float updateInterval = 0.5f; // Time between updates
    private float timer;

    // Start is called before the first frame update
    public void Start()
    { 
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateGrid();
        }
    }

    public void CreateGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        for(int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                Debug.Log($"Creating cell at position: {position}");
        
                GameObject cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a cube instead of a sprite
                cellObj.transform.position = position;
                cellObj.transform.localScale = new Vector3(cellSize, cellSize, cellSize); // Ensure the cubes are scaled correctly



                cellObj.transform.SetParent(transform); // Ensure the parent is set correctly


                Cell cell = cellObj.AddComponent<Cell>();
                grid[x,y] = cell;
                
                int zone = GetZone(x, y);
                cell.Initialize(this, x, y, zone);


                if (grid[x, y] == null)
            {
                Debug.LogError($"Cell at ({x},{y}) is null.");
                continue;
            }
            }

        }
    }

    void UpdateGrid()
    {
        bool[,] newStates = new bool[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {

                if (grid[x, y] == null)
            {
                Debug.LogError($"Cell at ({x}, {y}) is null in UpdateGrid.");
                continue; // Skip this cell
            }
                int aliveNeighbors = grid[x, y].GetAliveNeighbors();
                bool isAlive = grid[x, y].isAlive;

                if (isAlive && (aliveNeighbors < 2 || aliveNeighbors > 3))
                    newStates[x, y] = false; // Cell dies
                else if (!isAlive && aliveNeighbors == 3)
                    newStates[x, y] = true; // Cell becomes alive
                else
                    newStates[x, y] = isAlive; // Remains the same
            }
        }

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                grid[x, y].SetState(newStates[x, y]);
            }
        }
    }    

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
    }   

    int GetZone(int x, int y)
    {
        if (x < gridWidth / 2 && y < gridHeight / 2) return 0; // Top-left
        if (x >= gridWidth / 2 && y < gridHeight / 2) return 1; // Top-right
        if (x < gridWidth / 2 && y >= gridHeight / 2) return 2; // Bottom-left
        return 3; // Bottom-right
    }

    

}

