using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar1 : MonoBehaviour
{
    public List<TileNode> FindShortestPath(TileNode[,] grid, TileNode startTile, TileNode goalTile)
    {
        Dictionary<TileNode, float> gCost = new Dictionary<TileNode, float>();
        Dictionary<TileNode, float> fCost = new Dictionary<TileNode, float>();
        // Keep track of how i got to a tile
        Dictionary<TileNode, TileNode> previousTileMap = new Dictionary<TileNode, TileNode>();
        // Keep track of what tiles we visit
        List<TileNode> openSet = new List<TileNode>();
        foreach (TileNode tile in grid)
        {
            gCost[tile] = float.MaxValue;
            fCost[tile] = float.MaxValue;
        }

        gCost[startTile] = 0f;
        fCost[startTile] = HCost(startTile, goalTile);

        openSet.Add(startTile);

        while (openSet.Count > 0)
        {
            TileNode currentTile = GetTileWithLowestGCost(openSet, gCost);
            openSet.Remove(currentTile);

            if (currentTile == goalTile) 
            {
                // make a path from goal to start, then reverse
                List<TileNode> path = new List<TileNode>();
                TileNode current = goalTile;
                while (current != startTile)
                {
                    path.Add(current);
                    current = previousTileMap[current];
                }
                path.Add(startTile);
                path.Reverse();
                return path;
            }

            foreach (TileNode neighbourTile in currentTile.GetNeighbours(grid))
            {
                // This is something you will need to change if you have tiles of varying cost that aren't always 1
                // IMPORTANT
                float tentativeGCost = gCost[currentTile] + 1;

                // If neighbouring tile is not in the gCost library or lower, then visit
                if (!gCost.ContainsKey(neighbourTile) || tentativeGCost < gCost[neighbourTile])
                {
                    previousTileMap[neighbourTile] = currentTile;
                    gCost[neighbourTile] = tentativeGCost;
                    fCost[neighbourTile] = tentativeGCost + HCost(neighbourTile, goalTile);

                    if (!openSet.Contains(neighbourTile))
                    {
                        openSet.Add(neighbourTile);
                    }
                }
            }
        }
        Debug.Log("THERE IS NO PATH");
        return null;

    }

    private static TileNode GetTileWithLowestGCost(List<TileNode> tiles, Dictionary<TileNode, float> gCost)
    {
        TileNode minTile = null;
        float minCost = float.MaxValue;
        foreach (TileNode tile in tiles)
        {
            if (gCost[tile] < minCost)
            {
                minTile = tile;
                minCost = gCost[tile];
            }
        }
        return minTile;
    }


    private static float HCost(TileNode currentTile, TileNode goalTile)
    {
        // Manhattan distance
        return Mathf.Abs(currentTile.GetColIndex() - goalTile.GetColIndex()) + Mathf.Abs(currentTile.GetRowIndex() - goalTile.GetRowIndex());
    }
}
