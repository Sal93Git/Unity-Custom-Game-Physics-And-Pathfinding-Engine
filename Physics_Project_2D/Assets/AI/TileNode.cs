using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileNode : MonoBehaviour
{
    public GridMapGenerator gridMapGenerator;
    public List<TileNode> neighbourNodes;
    public TileType type;
    SpriteRenderer spriteRenderer;

    public int rowIndex;
    public int colIndex;

    public int tileWeight;
    // Start is called before the first frame update
    void Start()
    {
        gridMapGenerator = GameObject.FindObjectOfType<GridMapGenerator>();

        neighbourNodes = new List<TileNode>();
        neighbourNodes = GetNeighbours(gridMapGenerator.grid);

        spriteRenderer = GetComponent<SpriteRenderer>();
        if(type == TileType.Open)
        {
            spriteRenderer.color = Color.white;
            tileWeight = 1;
        }
        if(type == TileType.Obstacle)
        {
            spriteRenderer.color = Color.blue;
            tileWeight = 0;
        }
        if(type == TileType.Goal)
        {
            spriteRenderer.color = Color.green;
            tileWeight = 2;
        }
        if(type == TileType.Start)
        {
            spriteRenderer.color = Color.red;
            tileWeight = 3;
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

    public int GetTileWeight()
    {
        return tileWeight;
    }

    public void SetTileWeight(int _tileWeight)
    {
        tileWeight = _tileWeight;
    }

    public List<TileNode> GetNeighbours_()
    {
        return neighbourNodes;
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

        //-----------------------------------------
        // Check top-left neighbour
        if (rowIndex < numRows - 1 && colIndex > 0)
        {
            TileNode topLeftTile = tileMap[rowIndex + 1, colIndex - 1];
            if (topLeftTile.type != TileType.Obstacle)
            {
                neighbours.Add(topLeftTile);
            }
        }

        // Check top-right neighbour
        if (rowIndex < numRows - 1 && colIndex < numCols - 1)
        {
            TileNode topRightTile = tileMap[rowIndex + 1, colIndex + 1];
            if (topRightTile.type != TileType.Obstacle)
            {
                neighbours.Add(topRightTile);
            }
        }

        // Check bottom-left neighbour
        if (rowIndex > 0 && colIndex > 0)
        {
            TileNode bottomLeftTile = tileMap[rowIndex - 1, colIndex - 1];
            if (bottomLeftTile.type != TileType.Obstacle)
            {
                neighbours.Add(bottomLeftTile);
            }
        }

        // Check bottom-right neighbour
        if (rowIndex > 0 && colIndex < numCols - 1)
        {
            TileNode bottomRightTile = tileMap[rowIndex - 1, colIndex + 1];
            if (bottomRightTile.type != TileType.Obstacle)
            {
                neighbours.Add(bottomRightTile);
            }
        }
        //-----------------------------------------
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
