using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WhistlerMovement : MonoBehaviour {

    Transform player;
    Transform whistler;
    NavMeshAgent agent;
    NavMeshPath path;
    // Use this for initialization
    void Start () {

        player = GameObject.FindGameObjectWithTag("Player").transform;
        whistler = GameObject.FindGameObjectWithTag("Whistler").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.position);  
        agent.autoBraking = false;

        path = new NavMeshPath();
        bool hasFoundPath = agent.CalculatePath(player.position, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            print("The agent can reach the destionation");
        }
        else if (path.status == NavMeshPathStatus.PathPartial)
        {
            print("The agent can only get close to the destination");
        }
        else if (path.status == NavMeshPathStatus.PathInvalid)
        {
            print("The agent cannot reach the destination");
            print("hasFoundPath will be false");
        }
    }

    // Update is called once per frame
    void Update () {

        agent.SetDestination(player.position);
        path = new NavMeshPath();
        agent.CalculatePath(player.position, path);
    }
}