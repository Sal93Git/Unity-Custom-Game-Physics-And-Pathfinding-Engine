using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    //  public List<TileNode> FindShortestPath(TileNode[,] grid, TileNode startTile, TileNode goalTile)
    // {
    //     Dictionary<TileNode, float> gCost = new Dictionary<TileNode, float>();
    //     Dictionary<TileNode, float> fCost = new Dictionary<TileNode, float>();

    //     // Dictionary<TileNode, float> distanceMap = new Dictionary<TileNode, float>();
    //     Dictionary<TileNode,TileNode> previousTileMap = new Dictionary<TileNode, TileNode>();
    //     // List of visited tiles
    //     List<TileNode> openSet = new List<TileNode>();

    //     foreach (TileNode tile in grid)
    //     {
    //         gCost[tile] = float.MaxValue;
    //         fCost[tile] = float.MaxValue;
    //     }
    //     gCost[startTile] = 0;
    //     fCost[startTile] = HCost(startTile, goalTile);

    //     openSet.Add(startTile);

    //     while(openSet.Count > 0)
    //     {
    //         TileNode currentTile = GetTileWithLowestFCost(openSet, gCost);
    //         openSet.Remove(currentTile);

    //         if(currentTile == goalTile)
    //         {
    //             List<TileNode> path = new List<TileNode>();
    //             TileNode current = goalTile;
    //             while(current !=null)
    //             {
    //                 path.Add(current);
    //                 current = previousTileMap[current];
    //             }
    //             path.Add(startTile);
    //             path.Reverse();
    //             return path;
    //         }

    //         foreach(TileNode neighbourTile in currentTile.GetNeighbours(grid))
    //         {
    //             // This is something to change , need to change if have tiles of varying cost that arent all 1 (different tile weights/ change the +1 to +Tile weight)
    //             float tentativeGScore = gCost[currentTile]+1;

    //             //if Neighbour tile is not in gcost libary or lower , then visit
    //             if(!gCost.ContainsKey(neighbourTile) || tentativeGScore < gCost[neighbourTile])
    //             {
    //                 previousTileMap[neighbourTile] = currentTile;
    //                 gCost[neighbourTile] = tentativeGScore;
    //                 fCost[neighbourTile] = tentativeGScore + HCost(neighbourTile,goalTile);

    //                 if(!openSet.Contains(neighbourTile))
    //                 {
    //                     openSet.Add(neighbourTile);
    //                 }
    //             }
    //         }
    //         Debug.Log("NO PATH FOUND");
    //         return null;
    //     }

    //     static TileNode GetTileWithLowestFCost(List<TileNode> tiles, Dictionary<TileNode, float> gCost)
    //     {
    //         TileNode minTile = null;
    //         float minCost = float.MaxValue;
    //         foreach(TileNode tile in tiles)
    //         {
    //             if(gCost[tile] < minCost)
    //             {
    //                 minTile = tile;
    //                 minCost = gCost[tile];
    //             }
    //         }
    //         return minTile;
    //     }

    //     static float HCost(TileNode currentTile, TileNode goalTile)
    //     {
    //         //Manhattan distance
    //         return Mathf.Abs(currentTile.GetColIndex() - goalTile.GetColIndex())+ Mathf.Abs(currentTile.GetRowIndex() - goalTile.GetRowIndex());
            
    //     }

    // }







}
