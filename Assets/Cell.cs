using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public bool isAlive = false; // State of the cell
    private GridManager gridManager;
    private int x, y; // Cell position in the grid
    private int zone;
    


    public void Initialize(GridManager manager, int x, int y, int zone)
    {
        this.gridManager = manager;
        this.x = x;
        this.y = y;
        this.zone = zone;
        SetRandomState();
        UpdateAppearance();
    }

    void SetRandomState()
    {
        isAlive = Random.value > 0.5f; // 50% chance to be alive
    }


    public void SetState(bool state)
    {
        isAlive = state;
        UpdateAppearance();
    }
  
    // Update is called once per frame
    void UpdateAppearance()
    {

        switch (zone)
        {
            case 0: // Top-left
                GetComponent<Renderer>().material.color = Color.red; // Red for top-left
                break;
            case 1: // Top-right
                GetComponent<Renderer>().material.color = Color.green; // Green for top-right
                break;
            case 2: // Bottom-left
                GetComponent<Renderer>().material.color = Color.blue; // Blue for bottom-left
                break;
            case 3: // Bottom-right
                GetComponent<Renderer>().material.color = Color.yellow; // Yellow for bottom-right
                break;
        }

        if (isAlive)
            GetComponent<Renderer>().material.color = GetComponent<Renderer>().material.color; // Retain the zone color
        else
            GetComponent<Renderer>().material.color = Color.black;
    }    
    

    public int GetAliveNeighbors()
    {
        int aliveCount = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue; // Skip itself

                int nx = x + dx;
                int ny = y + dy;

                if (gridManager.IsValidCell(nx, ny) && gridManager.GetCell(nx, ny).isAlive)
                {
                    aliveCount++;
                }
            }
        }
        return aliveCount;
    }

    // Cell.cs

public void MakeAlive()
{
    isAlive = true;
    UpdateAppearance(); // Update the appearance of the cell (color change, etc.)
}

}
