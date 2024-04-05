using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


// Custom buttons for the inspector on the GridMapGenerator to generate new CSV files and add them to a list as well as the ability to update the list with what CSV files currently exist (in case user deletes etc)

[CustomEditor(typeof(GridMapGenerator))]
public class MyScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Cast the target to your script type
        if (target is GridMapGenerator gridGenScript)
        {
            // Add a button to the inspector
            if (GUILayout.Button("Generate CSV"))
            {
                // Call the method to generate the CSV file
                gridGenScript.GenerateCSV();
                gridGenScript.UpdateGeneratedCSVFilesList();
            }

            if (GUILayout.Button("Update 'generatedCSVFiles' List"))
            {
                // Call the method to update the csv file list
                gridGenScript.UpdateGeneratedCSVFilesList();
            }
        }
    }
}


public class GridMapGenerator : MonoBehaviour
{
    [Header("grid generation variables")]
    public GameObject tilePrefab;
    private float tileSize = 1.0f;
    private float padding = 0f;
    public int gridSize = 10;
    TileType[,] tileMap;

    public TileNode[,] grid;
    private TileNode startTile;
    private TileNode endTile;

    int _numberOfRows;
    int _numberOfColumns;
    //----------------
    [Header("PathFinding TESTING ")]
    public TileNode starter;
    public TileNode ender;
    //public Astar1 pathFinder2;
   // public DijkstrasPathfinding pathFinder;

    [Header("Generate CSV Files")]
    public int numberOfRowsToGenerate = 5;
    public int numberOfColumnsToGenerate = 6;
    public int minTwos = 3; // minimum number of times 2 should appear
    public int maxTwos = 6; // maximum number of times 2 should appear
    [SerializeField]
    private List<string> generatedCSVFiles = new List<string>();
    public int csvFilesListIndex;
    string csvFileToUse;
   

   // This function checks if a csv file already exists and creates a new one with a new name
    public void GenerateCSV()
    {
        Debug.Log("testing custom editor button");
        // Define the base file name
        string baseFileName = "RandomFile.csv";
        string fileName = baseFileName;
        string filePath = Application.dataPath + "/" + fileName;

        int fileNumber = 1;

        // Check if the file already exists and if so increment the fileNumber to be appended to create an unused file name
        while (File.Exists(filePath))
        {
            // File already exists, try the next number
            fileName = $"{Path.GetFileNameWithoutExtension(baseFileName)}_{fileNumber}.csv";
            filePath = Application.dataPath + "/" + fileName;
            fileNumber++;
        }
        // Open new streamwriter
        StreamWriter streamWriter = new StreamWriter(filePath);

        //GENERATE THE CSV FILE VALUES
        int twosCount = 0; // Counter for the number of times 2 has been used
        // randomly set the limit for how many 2 values can be used
        int randomlyGeneratedTwosLimit = Random.Range(minTwos, maxTwos);
        // Loop through generating random values between the specified range and using the value limits imposed 
        for (int i = 0; i < numberOfRowsToGenerate; i++)
        {
            string line = "";
            for (int j = 0; j < numberOfColumnsToGenerate; j++)
            {
                int randomNumber;
                if (twosCount < randomlyGeneratedTwosLimit)
                {
                    // Generate a random number between 0 and 2
                    randomNumber = Random.Range(0, 3);
                    // If the random number is 2, increment the twosCount
                    if (randomNumber == 2)
                    {
                        twosCount++;
                    }
                }
                else
                {
                    // If the maximum limit of 2 has been reached, generate random numbers between 0 and 1
                    randomNumber = Random.Range(0, 2);
                }
                line += randomNumber.ToString() + ",";
            }
            // Remove the trailing comma and write the line to the file
            line = line.TrimEnd(',');
            streamWriter.WriteLine(line);
        }
        //Close the streamwrite, output the file name and save the file to a list for the user to select a csv map from
        streamWriter.Close();
        generatedCSVFiles.Add(fileName);
        Debug.Log("Created : "+fileName);
    }

    public void UpdateGeneratedCSVFilesList()
    {
        // Clear the existing list
        generatedCSVFiles.Clear();

        // Scan the directory and add CSV files to the list
        string[] files = Directory.GetFiles(Application.dataPath, "*.csv");
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            generatedCSVFiles.Add(fileName);
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
       // pathFinder = GameObject.FindObjectOfType<DijkstrasPathfinding>();
       //pathFinder2 = GameObject.FindObjectOfType<Astar1>();
        csvFileToUse = generatedCSVFiles[csvFilesListIndex];
        LoadCSV();
    }

    // Currently being used to test pathfinding , should be cleared later
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space) && pathFinder2 != null) // Ensure pathFinder is not null
        // {
        //     List<TileNode> path = new List<TileNode>();
        //     path = pathFinder2.FindShortestPath(grid,starter,ender);
        //     foreach (TileNode node in path)
        //     {
        //         SpriteRenderer spriteRenderer = node.gameObject.GetComponent<SpriteRenderer>();
        //         // Ensure the spriteRenderer is not null before attempting to change its color
        //         if (spriteRenderer != null)
        //         {
        //             spriteRenderer.color = Color.black;
        //         }
        //     }
        // }
    }

    private void LoadCSV()
    {
       // string csvFilePath = Path.Combine(Application.dataPath, "layout.csv");
        string csvFilePath = Path.Combine(Application.dataPath, csvFileToUse);
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

        bool startTileGenerated = false;
        bool endTileGenerated = false;

        tileMap = new TileType[numRows,numCols];
        for(int i = 0; i < numRows; i++)
        {
            string[] values = lines[i].Split(",");
            for (int j = 0; j < numCols; j++ )
            {
                switch(int.Parse(values[j].Trim()))
                {
                    case 0:
                        if(!startTileGenerated)
                        {
                            tileMap[i,j] = TileType.Start;
                            startTileGenerated = true;
                            break;
                        }
                        if((i > numRows-2) && (j > numCols -5) && !endTileGenerated)
                        {
                            tileMap[i,j] = TileType.Goal;
                            endTileGenerated=true;
                            break;
                        }
                        else 
                        {
                            tileMap[i,j] = TileType.Open;
                            break;
                        }
                
                    case 1:
                        if((i > numRows-2) && (j > numCols -5) && !endTileGenerated)
                        {
                            tileMap[i,j] = TileType.Goal;
                            endTileGenerated=true;
                            break;
                        }
                        tileMap[i,j] = TileType.TallGrass;
                        break;
                    case 2:
                        tileMap[i,j] = TileType.Obstacle;
                        break;
                    // case 3:
                    //     tileMap[i,j] = TileType.Start;
                    //     break;
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


