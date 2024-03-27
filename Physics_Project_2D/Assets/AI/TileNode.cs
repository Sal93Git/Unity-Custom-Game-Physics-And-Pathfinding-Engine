using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    public GridMapGenerator test2;
    public List<TileNode> test;
    public TileType type;
    SpriteRenderer spriteRenderer;

    public int rowIndex;
    public int colIndex;
    // Start is called before the first frame update
    void Start()
    {
        test2 = GameObject.FindObjectOfType<GridMapGenerator>();

        test = new List<TileNode>();
        test = GetNeighbours(test2.grid);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(type == TileType.Open)
        {
            spriteRenderer.color = Color.white;
        }
        if(type == TileType.Obstacle)
        {
            spriteRenderer.color = Color.blue;
        }
        if(type == TileType.Goal)
        {
            spriteRenderer.color = Color.green;
        }
        if(type == TileType.Start)
        {
            spriteRenderer.color = Color.red;
        }
    }

    public int GetRowIndex()
    {
        return rowIndex;
    }

    public int GetColIndex()
    {
        return colIndex;
    }

    public List<TileNode> GetNeighbours_()
    {
        return test;
    }
    public List<TileNode> GetNeighbours(TileNode[,] tileMap)
    {
        List<TileNode> neighbours = new List<TileNode>();
        int numRows = tileMap.GetLength(0);
        int numCols = tileMap.GetLength(1);

        if(colIndex < numCols - 1)
        {
            TileNode rightTile = tileMap[rowIndex , colIndex +1];
            if (rightTile.type != TileType.Obstacle)
            {
                neighbours.Add(rightTile);
            }
        }

        if(colIndex > 0)
        {
            TileNode leftTile = tileMap[rowIndex , colIndex -1];
            if (leftTile.type != TileType.Obstacle)
            {
                neighbours.Add(leftTile);
            }
        }

        if(rowIndex < numRows -1)
        {
            TileNode topTile = tileMap[rowIndex + 1 , colIndex];
            if (topTile.type != TileType.Obstacle)
            {
                neighbours.Add(topTile);
            }
        }

         if(rowIndex > 0)
        {
            TileNode botTile = tileMap[rowIndex - 1 , colIndex];
            if (botTile.type != TileType.Obstacle)
            {
                neighbours.Add(botTile);
            }
        }

        return neighbours;
    }

  
}


public enum TileType
{
    Open,
    Obstacle,
    Goal,
    Start,
}
