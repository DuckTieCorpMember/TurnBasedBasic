using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TBGrid : NetworkBehaviour
{
    [SerializeField] private int width = 3;
    [SerializeField] private int length = 3;
    [SerializeField] private GameObject tilePrefab = null;

    private float[,] grid = null;

    [Server]
    public override void OnStartServer()
    {
        grid = new float[length, width];
        for(int i = 0; i < length; i++)
        {
            for(int j = 0; j < width; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0,j), Quaternion.identity, gameObject.transform);
                NetworkServer.Spawn(tile);
                grid[i,j] = tile.GetComponentInChildren<Renderer>().gameObject.transform.localScale.y;
            }
        }
        Print2DArray();
    }

    public void Print2DArray()
    {
        for (int i = 0; i < grid.GetLength(0); i++)
        {
            string row = "";
            for (int j = 0; j < grid.GetLength(1); j++)
            {
                row += grid[i, j] + " ";
            }
            Debug.Log(row);
        }
    }
}
