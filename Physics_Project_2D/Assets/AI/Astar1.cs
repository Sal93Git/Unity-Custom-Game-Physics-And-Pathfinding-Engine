using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar1 : MonoBehaviour
{
    Dictionary<TileNode, float> gCost = new Dictionary<TileNode, float>();
    Dictionary<TileNode, float> fCost = new Dictionary<TileNode, float>();
    List<TileNode> openSet = new List<TileNode>(); 
    Dictionary<TileNode, TileNode> previousTileMap = new Dictionary<TileNode, TileNode>();

    public List<TileNode> FindShortestPath(TileNode[,] grid, TileNode startTile, TileNode goalTile)
    {
        // Dictionary<TileNode, float> gCost = new Dictionary<TileNode, float>();
        // Dictionary<TileNode, float> fCost = new Dictionary<TileNode, float>();

        gCost.Clear();
        fCost.Clear();
        previousTileMap.Clear();
        openSet.Clear();
        
        // // Keep track of how i got to a tile
        // Dictionary<TileNode, TileNode> previousTileMap = new Dictionary<TileNode, TileNode>();
        // // Keep track of what tiles we visit
        // List<TileNode> openSet = new List<TileNode>();
    
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

            // The GetNeighbours function has been amended to take into account all surrounding traversable tiles including diagonal tiles.
            foreach (TileNode neighbourTile in currentTile.GetNeighbours(grid))
            {
                /* 
                Here i have made changes to take into account the tile weights(a value to represent terrain difficulty)
                Tile nodes all have a self contained weight value which we can retrieve and use in our calculations to decide which neighbouring tile to go to next
                Lower tile node value will indicate a cheaper path, 0 has been reserved for unpassable objects
                */
                int neighbourTileWeight = neighbourTile.GetTileWeight();
                float tentativeGCost = gCost[currentTile] + neighbourTileWeight;
                // float tentativeGCost = gCost[currentTile] + 1;

                // If the neighboring tile hasn't been visited or if the new path is cheaper, update its info and add it for further exploration.
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
        // Using manhattan distance
        return Mathf.Abs(currentTile.GetColIndex() - goalTile.GetColIndex()) + Mathf.Abs(currentTile.GetRowIndex() - goalTile.GetRowIndex());
    }
}
