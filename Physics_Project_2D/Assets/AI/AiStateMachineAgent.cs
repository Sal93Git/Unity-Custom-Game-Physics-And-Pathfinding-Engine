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
    public Astar1 pathFinder2;
    public AgentState currentState;
    List<TileNode> path = new List<TileNode>();
    Vector2 destination;

    // Start is called before the first frame update
    void Start()
    {
        currentState = AgentState.Idle;
        pathFinder2 = gameObject.GetComponent<Astar1>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetPath()
    {
        path.Clear();
    }

    public IEnumerator DoState(GameObject _target)
	{

        //path = pathFinder2.FindShortestPath(grid,starter,ender);
        ResetPath();

        while (true)
        {
            //agent.SetDestination(target.transform.position);
            yield return this;
		}
	}
}
