using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{

    public bool isAlive = false; // State of the cell
    private GridManager gridManager;
    private int x, y; // Cell position in the grid

    


    public void Initialize(GridManager manager, int x, int y)
    {
        this.gridManager = manager;
        this.x = x;
        this.y = y;
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
        if (isAlive)
            GetComponent<Renderer>().material.color = Color.white;
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
}
