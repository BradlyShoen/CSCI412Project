using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{

    Transform player;

    void Start()
    {
        // Set up the references.
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //nav = GetComponent(UnityEngine.AI.NavMeshAgent);
        //playerHealth = player.GetComponent<PlayerHealth>();
        //enemyHealth = GetComponent<EnemyHealth>();
        //nav = GetComponent<NavMeshAgent>();
    }


    void Update()
    {
        // If the enemy and the player have health left...
       // if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0)
     //   {
            // ... set the destination of the nav mesh agent to the player.
   //         nav.SetDestination(player.position);
   //     }
        // Otherwise...
     //   else
    //    {
            // ... disable the nav mesh agent.
   //         nav.enabled = false;
  //      }
    }
}