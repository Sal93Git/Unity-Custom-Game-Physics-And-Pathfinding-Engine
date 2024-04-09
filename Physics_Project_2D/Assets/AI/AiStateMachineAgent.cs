using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AgentState
{
    Idle,
    Chase,
    Evolve,
    Mitosis,
    Die,
}


public class AiStateMachineAgent : MonoBehaviour
{
    public Astar1 astar1;
    public AgentState currentState;
    public float speed = 5;
    public TileNode target;
    public int maximumBlobSizeIndex = 5;
    public GameObject agent;

    private int previousDestinationReachedCount = 0;
    private int destinationReachedCount = 0;
    public int currentBlobSizeIndex = 0;
    private List<TileNode> path = new List<TileNode>();
    private Vector2 destination;
    private Coroutine gameCoroutine = null;
    private GridMapGenerator gridGen;
    private bool agentIsActive = false;
    private bool agentBusy = false;
    //public TileNode[,] grid;
    
    // Start is called before the first frame update
    void Start()
    {
        astar1 = gameObject.GetComponent<Astar1>();
        gridGen = GameObject.FindObjectOfType<GridMapGenerator>();
        if(!agentIsActive)
        {
            agentIdleStartUp();
        }
        else
        {
            activateAgentBehaviour();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //A way for user input to begin agent behaviour
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!agentIsActive)
            {
                gameCoroutine =  StartCoroutine(TravelState(target));
                agentIsActive = true;
            }
        }
       
        // do checks for state change condition
        if(lifeExpectancyReached())
        {
            Debug.Log("lifeExpectancyReached is true");
            updateStateCoroutine(DieState());
        }
        else if(readyToTravel())
        {
            Debug.Log("readyToTravel is true");
            updateStateCoroutine(TravelState(target=getNewRandomDestination()));
        }
        else if(atDestination())
        {
            Debug.Log("atDestination is true");
            if(currentBlobSizeIndex == maximumBlobSizeIndex-3)
            {
                updateStateCoroutine(MitosisState());
            }
            else if(currentBlobSizeIndex != maximumBlobSizeIndex-3)
            {
                updateStateCoroutine(EvolveState());
            }
        }
    }

    //To be used when instantiating an instance of this agent to alter the Start() behavior
    public void setAgentToActive()
    {
        agentIsActive=true;
    }

    //This Function is used in the start function to soft start the agent in an idle state
    void agentIdleStartUp()
    {
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
        currentState = AgentState.Idle;
    }

    // This function clears the currently running state coroutine and takes in a new IEnumerator to start the new coroutine
    private void updateStateCoroutine(IEnumerator newState)
    {
        if(gameCoroutine!=null)
        {
            StopCoroutine(gameCoroutine);
        }
        gameCoroutine =  StartCoroutine(newState);
    }

     //This Function is used in the start function to immediatley start the agent in an chase state
    private void activateAgentBehaviour()
    {
      
        //target = getNewRandomDestination();
        gameCoroutine =  StartCoroutine(TravelState(target = getNewRandomDestination()));
        agentIsActive = true;
        
    }

    // Reset the current tile path
    private void ResetPath()
    {
        if(path != null)
        {
            path.Clear();
        }
    }

    // This function is used to work out which Tile this agent is currently sitting on or is closest to
    private TileNode calculateNearestTile()
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

    //This function gets a new TileNode at random to be set as the target destination, it also checks that the tile selected is traversable and can be reached before returning
    private TileNode getNewRandomDestination()
    {
        List<TileNode> tiles = gridGen.getGridNodes();
        bool suitableTileFound = false;
        TileNode currentFoundTile = null;
        TileNode currentPositionTile = calculateNearestTile();
        if(tiles == null || tiles.Count == 0)
        {
            Debug.Log("could not get a populated list of tiles when calling getNewRandomDestination() on AiStateMachineAgent");
            return null;
        }
        while(!suitableTileFound)
        {
            ResetPath();
            int randomTileIndex = Random.Range(0,tiles.Count-1);
            currentFoundTile = tiles[randomTileIndex];
            path = astar1.FindShortestPath(gridGen.grid,calculateNearestTile(), currentFoundTile);
    
            if((path != null)&&(currentFoundTile.type != TileType.Obstacle) && currentFoundTile!=currentPositionTile)
            {
                suitableTileFound = true;
            }
        }
        //return tiles[randomTileIndex];
        return currentFoundTile;
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
        agentBusy = true;
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
        transform.localScale = new Vector3(originalScaleX*1.30f,originalScaleY*1.30f, transform.localScale.z);
        currentBlobSizeIndex++;
        agentBusy = false;
	}

    public IEnumerator MitosisState()
    {
        agentBusy = true;
        currentState = AgentState.Mitosis;
        float  stateCountDown = 2f;
        while(stateCountDown > 0)
        {
            yield return new WaitForSeconds(1f);
            stateCountDown -= 1f;
        }
        GameObject instantiatedObject = Instantiate(agent);
        instantiatedObject.GetComponent<AiStateMachineAgent>().setAgentToActive();
        currentBlobSizeIndex++;
        transform.localScale = new Vector3(transform.localScale.x*0.9f,transform.localScale.y*0.9f, transform.localScale.z);
        agentBusy = false;
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
    private bool atDestination()
    {
        if((target != null) && (Vector3.Distance(transform.position, target.gameObject.transform.position) <= 0.15f))
        {
            destinationReachedCount++;  
            target = null;
            return true;
        }
        else
        {
            return false;
        }
    }

    // is it time for agent blob to die
    private bool lifeExpectancyReached()
    {
        if(currentBlobSizeIndex >= maximumBlobSizeIndex && (currentState != AgentState.Die))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool readyToTravel()
    {
        if((previousDestinationReachedCount < destinationReachedCount) && (currentState != AgentState.Chase) && (target == null) && (!agentBusy))
        {
            previousDestinationReachedCount++;
            return true;
        }
        else
        {
            return false;
        }
    }
}
