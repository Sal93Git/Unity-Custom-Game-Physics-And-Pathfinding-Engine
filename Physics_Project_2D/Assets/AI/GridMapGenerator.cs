using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GridMapGenerator : MonoBehaviour
{

    public GameObject tilePrefab;
    private float tileSize = 1.0f;
    private float padding = 0.1f;
    public int gridSize = 10;
    TileType[,] tileMap;

    public TileNode[,] grid;
    private TileNode startTile;
    private TileNode endTile;

    int _numberOfRows;
    int _numberOfColumns;
    //----------------
    public TileNode starter;
    public TileNode ender;
    public DijkstrasPathfinding pathFinder;

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         List<TileNode> path = new List<TileNode>();
    //         path = pathFinder.FindShortestPath(grid,starter , ender);
    //         foreach (TileNode node in path)
    //         {
    //             SpriteRenderer spriteRenderer = node.gameObject.GetComponent<SpriteRenderer>();
    //             // Ensure the spriteRenderer is not null before attempting to change its color
    //             if (spriteRenderer != null)
    //             {
    //                 spriteRenderer.color = Color.black;
    //             }
    //         }
    //     }
    // }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && pathFinder != null) // Ensure pathFinder is not null
        {
            List<TileNode> path = new List<TileNode>();
            path = pathFinder.FindShortestPath(grid,starter,ender);
            foreach (TileNode node in path)
            {
                SpriteRenderer spriteRenderer = node.gameObject.GetComponent<SpriteRenderer>();
                // Ensure the spriteRenderer is not null before attempting to change its color
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.black;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        pathFinder = GameObject.FindObjectOfType<DijkstrasPathfinding>();
        LoadCSV();
    }

    private void LoadCSV()
    {
        string csvFilePath = Path.Combine(Application.dataPath, "layout.csv");
        List<string> lines = new List<string>();
        using(StreamReader reader = new StreamReader(csvFilePath))
        {
            while(!reader.EndOfStream)
            {
                lines.Add(reader.ReadLine());
            }
        }

        int numRows = lines.Count;
        int numCols = lines[0].Split(',').Length;

        _numberOfRows = numRows;
        _numberOfColumns = numCols;

        tileMap = new TileType[numRows,numCols];
        for(int i = 0; i < numRows; i++)
        {
            string[] values = lines[i].Split(",");
            for (int j = 0; j < numCols; j++ )
            {
                switch(int.Parse(values[j].Trim()))
                {
                    case 0:
                        tileMap[i,j] = TileType.Open;
                        break;
                    case 1:
                        tileMap[i,j] = TileType.Goal;
                        break;
                    case 2:
                        tileMap[i,j] = TileType.Obstacle;
                        break;
                    case 3:
                        tileMap[i,j] = TileType.Start;
                        break;
                }
            }
        }

        grid = new TileNode[_numberOfRows,_numberOfColumns];


        for(int y = 0; y < numRows; y++)
        {
            for(int x = 0; x < numCols; x++)
            {
                Vector2 position = new Vector2(x*(tileSize + padding), (numRows - y -1)* (tileSize + padding));
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity);
                TileNode tileScript = tile.GetComponent<TileNode>();
                if(tileScript != null)
                {
                    tileScript.type = tileMap[y,x];
                    tileScript.rowIndex =  y;
                    tileScript.colIndex =  x;
                    grid[y,x] = tileScript;
                }
                tile.name = "Tile_" + x + "_"+y;
            }
        }

        // Debug.Log(csvFilePath);
        // foreach(string line in lines)
        // {
        //     Debug.Log(line);
        // }
    }

    public List<TileNode> getGridNodes()
    {
        List<TileNode> gridNodes = new List<TileNode>();

        for(int y = 0; y < _numberOfRows; y++)
        {
            for(int x = 0; x < _numberOfColumns; x++)
            {
               gridNodes.Add(grid[y,x]);
            }
        }
        return gridNodes;
    }
}
