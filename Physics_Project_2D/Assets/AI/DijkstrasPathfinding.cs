using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstrasPathfinding : MonoBehaviour
{

    public List<TileNode> FindShortestPath(TileNode[,] grid, TileNode startTile, TileNode goalTile)
    {
        Dictionary<TileNode, float> distanceMap = new Dictionary<TileNode, float>();
        Dictionary<TileNode,TileNode> previousTileMap = new Dictionary<TileNode, TileNode>();
        List<TileNode> unvisitedTiles = new List<TileNode>();
        //List<TileNode> unvisitedTilesCopy = new List<TileNode>(unvisitedTiles);
        foreach (TileNode tile in grid)
        {
            distanceMap[tile] = float.MaxValue;
            previousTileMap[tile] = null;
            unvisitedTiles.Add(tile);
        }
        distanceMap[startTile] = 0;


        while(unvisitedTiles.Count > 0)
        {
            Debug.Log("NEXT TILE"); 

            TileNode currentTile = GetTileWithMinDistance(distanceMap, unvisitedTiles);
            unvisitedTiles.Remove(currentTile);

            if(currentTile != null)
            {
                Debug.Log("TILE NAME " + currentTile.gameObject.name);
                Debug.Log("unvisitedTiles.Count = " + unvisitedTiles.Count);
            }
            else
            {
                Debug.Log("CURRENT TILE RETURNED NULL");
                break;
            }
            
            //Check Neighbouring tiles and updates its distance if shorter than shortest known distance
            foreach(TileNode neighbourTile in currentTile.GetNeighbours(grid))
            {
                Debug.Log("LOOP STARTED"+ neighbourTile.gameObject.name + "___"+ currentTile.gameObject.name );

                // if (unvisitedTiles.Contains(neighbourTile))
                // {
                //     float tentativeDistance = distanceMap[currentTile]+1;
                //     if(tentativeDistance < distanceMap[neighbourTile])
                //     {
                //         distanceMap[neighbourTile] = tentativeDistance;
                //         previousTileMap[neighbourTile] = currentTile;
                //     }
                // }

                // float tentativeDistance = distanceMap[neighbourTile]+1;
                float tentativeDistance = distanceMap[currentTile]+1;
                if(tentativeDistance < distanceMap[neighbourTile])
                {
                    distanceMap[neighbourTile] = tentativeDistance;
                    previousTileMap[neighbourTile] = currentTile;
                }
            } 
        }

        List<TileNode> path = new List<TileNode>();
        TileNode current = goalTile;
        while(current !=null)
        {
            path.Add(current);
            current = previousTileMap[current];
        }
        path.Reverse();
        return path;
    }

    private static TileNode GetTileWithMinDistance(Dictionary<TileNode, float> distanceMap, List<TileNode> tiles)
    {
        TileNode minTile = null;
        float minDistance = float.MaxValue;
        foreach(TileNode tile in tiles)
        {
            if(distanceMap[tile] < minDistance)
            {
                minTile = tile;
                minDistance = distanceMap[tile];
            }
        }
        return minTile;
    }
    

}






