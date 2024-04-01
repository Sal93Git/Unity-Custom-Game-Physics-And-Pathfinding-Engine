using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AgentState
{
    Idle,
    Chase,
}


public class AiStateMachineAgent : MonoBehaviour
{
    public Astar1 astar1;
    public AgentState currentState;
    public float speed = 5;
    public TileNode target;

    private List<TileNode> path = new List<TileNode>();
    private Vector2 destination;
    private Coroutine gameCoroutine = null;
    private GridMapGenerator gridGen;
    //public TileNode[,] grid;

    // Start is called before the first frame update
    void Start()
    {
        currentState = AgentState.Idle;
        astar1 = gameObject.GetComponent<Astar1>();
        gridGen = GameObject.FindObjectOfType<GridMapGenerator>();
        //TileNode x = calculateNearestTile();
    }

    // Update is called once per frame
    void Update()
    {
        // do checks for state change condition

        if (Input.GetKeyDown(KeyCode.Space)) // Ensure pathFinder is not null
        {
            gameCoroutine =  StartCoroutine(TravelState(target));
        }
        
    }

    private void ResetPath()
    {
        path.Clear();
    }

    public IEnumerator TravelState(TileNode _target)
	{
        ResetPath();
        TileNode starterTile = calculateNearestTile();
        path = astar1.FindShortestPath(gridGen.grid,starterTile, _target);
        foreach(TileNode tile in path)
        {
            while (Vector3.Distance(transform.position, tile.gameObject.transform.position) > 0.5f)
            {
                // Calculate the direction vector towards the target
                Vector3 direction = (tile.gameObject.transform.position - transform.position).normalized;
                // Move the GameObject towards the target
                transform.position += direction * speed * Time.deltaTime;
                yield return this;
            }
        }
        
	}


    // This function is used to work out which Tile this agent is currently sitting on or is closest to
    TileNode calculateNearestTile()
    {
        TileNode tileReferenceHolder = null;
        float tileDistance = float.MaxValue;
        List<TileNode> tiles = gridGen.getGridNodes();

        foreach(TileNode tile in tiles)
        {
            float tempDistance = Vector3.Distance(transform.position, tile.transform.position);
            if(tile.type == TileType.Obstacle)
            {
                continue;
            }
            if(tempDistance < tileDistance)
            {
                tileDistance = tempDistance;
                tileReferenceHolder = tile;               
            }
        }
        if(tileReferenceHolder != null)
        {
            SpriteRenderer spriteRenderer = tileReferenceHolder.gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = Color.black;
        }
        return tileReferenceHolder;
    }
}



// private Coroutine gameCoroutine = null;
// gameCoroutine =  StartCoroutine(currentState.DoState(gameObject));
// StopCoroutine(gameCoroutine);