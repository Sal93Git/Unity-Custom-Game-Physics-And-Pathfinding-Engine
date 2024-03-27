using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPathfinder : MonoBehaviour
{
    public GridMapGenerator gridNodesMap;
    public TileNode startNode;
    public TileNode endNode;

    Dictionary<TileNode, float> distance;
    Dictionary<TileNode, TileNode> previous;
    List<TileNode> unvisitedNodes;
    public List<TileNode> allNodes;
    void Start()
    {
        distance = new Dictionary<TileNode, float>();
        previous = new Dictionary<TileNode, TileNode>();
        unvisitedNodes = new List<TileNode>();

        // List<TileNode> allNodes = GetAllNodesInGrid();
        allNodes = gridNodesMap.getGridNodes();

    }

    private void FindShortestPath(TileNode startTile, TileNode endTile)
    {
        foreach (TileNode node in allNodes)
        {
            distance[node] = float.MaxValue;
            previous[node] = null;
            unvisitedNodes.Add(node);
        }
        distance[startTile] = 0;


        // Main loop until all nodes are visited or end node is reached
        // while (unvisitedNodes.Count > 0)
        // {
        //     // Find node with lowest combined cost (actual distance + heuristic) from start to goal
        //     TileNode currentNode = GetNodeWithLowestCombinedCost(unvisitedNodes, endTile);
        //     unvisitedNodes.Remove(currentNode);

        //     // If current node is the end node, terminate the loop
        //     if (currentNode == endTile)
        //     {
        //         break;
        //     }
               

        //     // Iterate through neighbors of current node
        //     foreach (TileNode neighbor in currentNode.GetNeighbours(gridNodesMap.grid))
        //     {
        //         // Skip unpassable nodes *********REVIST THIS
        //         // if (!neighbor.passable)
        //         //     continue;

        //         // Calculate tentative distance from start to neighbor through current node
        //         float tentativeDistance = distance[currentNode] + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);
                
        //         // Update distance and previous node if tentative distance is shorter
        //         if (tentativeDistance < distance[neighbor])
        //         {
        //             distance[neighbor] = tentativeDistance;
        //             previous[neighbor] = currentNode;
        //         }
        //     }
        // }

        // // Reconstruct shortest path
        // List<TileNode> path = GetShortestPath(endTile);
        // // Now 'path' contains the shortest path from start to end

    }

}
