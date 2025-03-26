using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public int gridSize = 100; // Grid size (100x100)
    public int cellSize = 10; // Each cell is 10x10 pixels
    private Cell[,] grid; // 2D array to store cells
    public float updateInterval = 0.5f; // Time between updates
    private float timer;

    // Start is called before the first frame update
    void Start()
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

    void CreateGrid()
    {
        grid = new Cell[gridSize, gridSize];

        for(int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {

                
                Vector3 position = new Vector3(x * cellSize, y * cellSize, 0);
                Debug.Log($"Creating cell at position: {position}");
        
                GameObject cellObj = GameObject.CreatePrimitive(PrimitiveType.Cube); // Create a cube instead of a sprite
                cellObj.transform.position = position;
                cellObj.transform.localScale = new Vector3(cellSize, cellSize, 1); // Ensure the cubes are scaled correctly



                cellObj.transform.SetParent(transform); // Ensure the parent is set correctly


                Cell cell = cellObj.AddComponent<Cell>();
                grid[x,y] = cell;

                cell.Initialize(this, x, y);


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
        bool[,] newStates = new bool[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
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

        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                grid[x, y].SetState(newStates[x, y]);
            }
        }
    }    

    public bool IsValidCell(int x, int y)
    {
        return x >= 0 && x < gridSize && y >= 0 && y < gridSize;
    }

    public Cell GetCell(int x, int y)
    {
        return grid[x, y];
    }   


    void AddGridBorder(GameObject cellObj, int x, int y)
{
    // Create borders (lines) around the cells using LineRenderer or creating thin cubes
    GameObject borderObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
    borderObj.transform.position = new Vector3(x * cellSize, y * cellSize, 0);
    borderObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f); // Small scale for the border

    // Set the border color to a distinguishable color
    Renderer borderRenderer = borderObj.GetComponent<Renderer>();
    borderRenderer.material.color = Color.white;  // Change this to any color for the border

    // Set border as a child of the cell (to keep things organized)
    borderObj.transform.SetParent(cellObj.transform);
}


}

