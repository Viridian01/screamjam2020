using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public Transform[] destinations;
    public float speed;
    private int cPos;

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if ((transform.position.x != destinations[cPos].position.x) && (transform.position.y != destinations[cPos].position.y))
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, destinations[cPos].position, speed * Time.deltaTime);
            GetComponent<Rigidbody>().MovePosition(pos);
        }
        else
        {
            cPos = (cPos + 1) % destinations.Length;
        }
    }

    // goes to last seen position player looked at enemy
    private void Alert()
    {

    }

    private void Hunting()
    {
        
    }

    // public NavMeshAgent agent;
    // public Transform player;
    // public LayerMask theGround, thePlayer;
    // public Vector3 point;
    // public float walkRange;
    // bool foundPoint;
    // public float sightRange;
    // public bool playerFound;
    // // pre-determined locations
    // public Transform[] destinations;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     player = GameObject.Find("Player").transform;
    //     agent = GetComponent<NavMeshAgent>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     playerFound = CheckPlayer();
    //     // if the player has not been found then roam
    //     if (!playerFound)
    //     {
    //         Roam();
    //     }
    // }

    // private bool CheckPlayer()
    // {
    //     // checks if the player is in the monster's range of sight
    //     return Physics.CheckSphere(transform.position, sightRange, thePlayer);
    // }

    // private void Roam()
    // {
    //     if (!foundPoint)
    //     {
    //         WalkPath();
    //     }
    //     agent.SetDestination(point);

    //     Vector3 walkDestination = transform.position - point;
    //     // if distance walked is less than 1 then destination has been reached
    //     if (walkDestination.magnitude < 1f)
    //     {
    //         foundPoint = false;
    //     }
    // }

    // private void WalkPath()
    // {
    //     // random point
    //     // float pathX = Random.Range(-walkRange, walkRange);
    //     // float pathY = Random.Range(-walkRange, walkRange);

    //     // random point destination
    //     point = new Vector3(transform.position.x + pathX, transform.position.y, transform.position.z + pathY);

    //     // check if walk destination is valid
    //     if (Physics.Raycast(point, -transform.up, 2f, theGround))
    //     {
    //         foundPoint = true;
    //     }
    // }

    // private void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, sightRange);
    // }
}