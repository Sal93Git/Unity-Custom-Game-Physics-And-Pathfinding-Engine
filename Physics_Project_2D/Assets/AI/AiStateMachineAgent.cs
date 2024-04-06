using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AgentState
{
    Idle,
    Chase,
    Evolve,
    Die,
}


public class AiStateMachineAgent : MonoBehaviour
{
    public Astar1 astar1;
    public AgentState currentState;
    public float speed = 5;
    public TileNode target;
    public int maximumBlobSizeIndex = 0;

    private List<TileNode> path = new List<TileNode>();
    private Vector2 destination;
    private Coroutine gameCoroutine = null;
    private GridMapGenerator gridGen;
    //public TileNode[,] grid;
    
    // Start is called before the first frame update
    void Start()
    {
        currentState = AgentState.Chase;
        astar1 = gameObject.GetComponent<Astar1>();
        gridGen = GameObject.FindObjectOfType<GridMapGenerator>();

        List<TileNode> tiles = gridGen.getGridNodes();
        foreach(TileNode tile in tiles)
        {
            if(tile.type == TileType.Start)
            {
                gameObject.transform.position = tile.transform.position;
            }
            if(tile.type == TileType.Goal)
            {
                target = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Ensure pathFinder is not null
        {
            gameCoroutine =  StartCoroutine(TravelState(target));
        }

        // do checks for state change condition
        if(atDestination())
        {
            updateStateCoroutine(EvolveState());
        }

        if(lifeExpectancyReached())
        {
            updateStateCoroutine(DieState());
        }
    }

    void updateStateCoroutine(IEnumerator newState)
    {
        StopCoroutine(gameCoroutine);
        gameCoroutine =  StartCoroutine(newState);
    }

    // Reset the current tile path
    private void ResetPath()
    {
        path.Clear();
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
            spriteRenderer.color = Color.white;
        }
        return tileReferenceHolder;
    }


    // *************************STATE COROUTINES***********************************

    // This coroutine takes a target tile node and moves the agent to that target destination
    public IEnumerator TravelState(TileNode _target)
	{
        currentState = AgentState.Chase;
        ResetPath();
        TileNode starterTile = calculateNearestTile();
        path = astar1.FindShortestPath(gridGen.grid,starterTile, _target);
        foreach(TileNode tile in path)
        {
            while (Vector3.Distance(transform.position, tile.gameObject.transform.position) > 0.15f)
            {
                // Calculate the direction vector towards the target
                Vector3 direction = (tile.gameObject.transform.position - transform.position).normalized;
                // Move the GameObject towards the target
                transform.position += direction * speed * Time.deltaTime;
                yield return this;
            }
        }  
	}

    // This coroutine simulates the blob evolving and then increases its size/scale
    public IEnumerator EvolveState()
	{
        currentState = AgentState.Evolve;
        float originalScaleX = transform.localScale.x;
        float originalScaleY = transform.localScale.y;
        float  evolutionCountDown = 6f;
        bool scaleAxisToggle = false;

        while(evolutionCountDown > 0)
        {
            yield return new WaitForSeconds(0.5f);
            evolutionCountDown -= 0.5f;
            if(scaleAxisToggle)
            {
                transform.localScale = new Vector3(transform.localScale.x*2,transform.localScale.y/2, transform.localScale.z);
                scaleAxisToggle = false;
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x / 2f,transform.localScale.y*2, transform.localScale.z);
                scaleAxisToggle = true;
            }
        }
        transform.localScale = new Vector3(originalScaleX*1.25f,originalScaleY*1.25f, transform.localScale.z);
        maximumBlobSizeIndex++;
	}

    // This coroutine simulates the blob dying and then destroys/kills the blob game object
    public IEnumerator DieState()
	{
        currentState = AgentState.Die;
        float dieCountDown = 5f;
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.color = Color.red;

        while(dieCountDown > 0)
        {
            yield return new WaitForSeconds(0.5f);
            dieCountDown -= 0.5f;
            transform.localScale = new Vector3(transform.localScale.x * 0.8f,transform.localScale.y*0.8f, transform.localScale.z);
        }
        Destroy(gameObject);
    }

    // *************************STATE CONDITION BOOLS***********************************

    // did agent blob reach its destination
    bool atDestination()
    {
        if((target != null) && (Vector3.Distance(transform.position, target.gameObject.transform.position) <= 0.15f) && (currentState != AgentState.Evolve))
        {
            target = null;  
            return true;
        }
        else
        {
            return false;
        }
    }

    // is it time for agent blob to die
    bool lifeExpectancyReached()
    {
        if(maximumBlobSizeIndex >= 1 && (currentState != AgentState.Die))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

// private Coroutine gameCoroutine = null;
// gameCoroutine =  StartCoroutine(currentState.DoState(gameObject));
// StopCoroutine(gameCoroutine);