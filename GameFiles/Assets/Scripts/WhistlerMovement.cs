using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor.Animations;

public class WhistlerMovement : MonoBehaviour
{

    public Transform player;
    public Transform whistler;
    public NavMeshAgent agent;
    public NavMeshPath path;
    public triggerDamage triggerDamage;
    public BoxCollider whistlerCollider;

    public int fov = 90;
    public float aggro_speed = 2.5F;
    public float wander_speed = 1.5F;
    public float search_speed = 2F;
    public float rotate_speed = .001F;
    public float damage_range = 4F;
	public float attackAnimThreshold = 1f;
    // Use this for initialization
    void Start()
    {

        agent.SetDestination(player.position);
        agent.autoBraking = false;

        path = new NavMeshPath();
        bool hasFoundPath = agent.CalculatePath(player.position, path);
    }

    // Update is called once per frame
    void Update()
    {

        agent.SetDestination(player.position);
        path = new NavMeshPath();
        //whistler.LookAt(player);
        rotate();
        agent.CalculatePath(player.position, path);
        if (LineOfSight(whistler, player))
        {
            print("found player");
            aggro();
        }
        else
        {
            wander();
        }
        triggerDamage.OnTriggerStay(whistlerCollider);
		if(Vector3.Distance(whistler.position, player.position) <= (damage_range + attackAnimThreshold)){
			gameObject.GetComponent<Animator>().SetBool("IsAttacking", true);
		}else{
			gameObject.GetComponent<Animator>().SetBool("IsAttacking", false);
		}
		
        if(Vector3.Distance(whistler.position, player.position) <= damage_range)
        {
            print("attacking player");
            StartCoroutine(triggerDamage.applyPlayerDamage(player.gameObject));
        }
    }

    bool LineOfSight(Transform transform, Transform target)
    {
        if (Vector3.Angle(transform.position - target.position, transform.forward) <= fov &&
        Physics.Linecast(transform.position, target.position))
        {
            return true;
        }
        return false;
    }

    void rotate()
    {


        Quaternion wantedRotation = Quaternion.LookRotation(player.position - whistler.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, wantedRotation, Time.time * rotate_speed);

    }

    void aggro()
    {
        agent.speed = aggro_speed;
    }

    void wander()
    {

        agent.speed = wander_speed;
    }

    void search()
    {
        agent.speed = search_speed;
    }
}