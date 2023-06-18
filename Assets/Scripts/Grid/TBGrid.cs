using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Data;
using System.Diagnostics;
using UnityEngine.Rendering;
using Unity.Mathematics;

public class TBGrid : NetworkBehaviour
{
    public bool findDistance = false;

    [SerializeField] private int columns = 3;
    [SerializeField] private int rows = 3;
    [SerializeField] private GameObject tilePrefab = null;

    private TBGridTile[,] grid = null;
    private int scale = -1;

    public int startX = 0;
    public int startY = 0;

    public int endX = 0;
    public int endY = 0;

    List<TBGridTile> path = new List<TBGridTile>();

    [Server]
    public override void OnStartServer()
    {
        grid = new TBGridTile[rows, columns];
        for(int i = 0; i < rows; i++)
        {
            for(int j = 0; j < columns; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0,j), Quaternion.identity, gameObject.transform);
                NetworkServer.Spawn(tile);
                TBGridTile tileComponent = tile.GetComponent<TBGridTile>();
                tileComponent.x = i;
                tileComponent.y = j;
                grid[i, j] = tileComponent;
            }
        }
    }
    [ServerCallback]
    private void Update()
    {
        if (findDistance)
        {
            SetDistance();
            SetPath();
            findDistance = false;
        }
    }

    void SetDistance()
    {
        InitalSetup();
        int x = startX;
        int y = startY;
        int[] testArray = new int[rows * columns];
        for(int step=1; step < rows*columns; step++)
        {
            foreach(TBGridTile obj in grid)
            {
                if(obj && obj.visited == step - 1)
                {
                    TestFourDirections(obj.x, obj.y, step);
                }
            }
        }
    }
    void SetPath()
    {
        int step;
        int x = endX;
        int y = endY;
        List<TBGridTile> tempList = new List<TBGridTile>();
        path.Clear();
        if (grid[endX, endY] && grid[endX, endY].visited > 0)
        {
            path.Add(grid[x, y]);
            step = grid[x, y].visited - 1;
        }
        else
        {
            UnityEngine.Debug.Log("Cannot reach the desired location");
            return;
        }
        for(int i = step; step > -1; step--)
        {
            if (TestDirection(x, y, step, 1))
                tempList.Add(grid[x, y + 1]);
            if (TestDirection(x, y, step, 2))
                tempList.Add(grid[x+1, y]);
            if (TestDirection(x, y, step, 3))
                tempList.Add(grid[x, y - 1]);
            if (TestDirection(x, y, step, 4))
                tempList.Add(grid[x-1, y]);

            TBGridTile tempObj = FindClosest(grid[endX, endY].transform.position, tempList);
            path.Add(tempObj);
            x = tempObj.x; y = tempObj.y;
            tempList.Clear();
        }
        
    }

    void InitalSetup()
    {
        foreach(TBGridTile obj in grid)
        {
            obj.GetComponent<TBGridTile>().visited = -1;
        }
        grid[startX, startY].GetComponent<TBGridTile>().visited = 0;
    }
    void TestFourDirections(int x, int y, int step)
    {
        if (TestDirection(x, y, -1, 1))
            SetVisited(x, y + 1, step);
        if (TestDirection(x, y, -1, 2))
            SetVisited(x+1, y, step);
        if (TestDirection(x, y, -1, 3))
            SetVisited(x, y - 1, step);
        if (TestDirection(x, y, -1, 4))
            SetVisited(x-1, y, step);
    }
    bool TestDirection(int x, int y, int step, int direction)
    {
        // 1 is up, 2 is right, 3 is down, 4 is left
        switch (direction)
        {
            case 1:
                if (y + 1 < rows && grid[x, y + 1] && grid[x, y + 1].visited == step)
                {
                    return true;
                }
                else return false;
            case 2:
                if (x + 1 < columns && grid[x+1, y] && grid[x+1, y].visited == step)
                {
                    return true;
                }
                else return false;
            case 3:
                if (y - 1 > -1 && grid[x, y - 1] && grid[x, y - 1].visited == step)
                {
                    return true;
                }
                else return false;
            case 4:
                if (x - 1 > -1 && grid[x - 1, y] && grid[x - 1, y].visited == step)
                {
                    return true;
                }
                else return false;
        }
        return false;
    }

    void SetVisited(int x, int y, int step)
    {
        if (grid[x,y])
            grid[x,y].visited = step;
    }

    TBGridTile FindClosest(Vector3 targetLocation, List<TBGridTile> list)
    {
        float currentDistance = scale * rows * columns;
        int indexNumber = 0;
        for(int i=0; i<list.Count; i++)
        {
            if(Vector3.Distance(targetLocation, list[i].transform.position) < currentDistance){
                currentDistance = Vector3.Distance(targetLocation, list[i].transform.position);
                indexNumber = i;
            }
        }
        return list[indexNumber];
    }
}
