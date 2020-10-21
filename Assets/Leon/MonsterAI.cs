using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public GameObject player;
    private NavMeshAgent nav;
    public Transform[] destinations;
    private string state = "default";
    private bool alive = true;
    private int dest;
    public Transform eyes;
    public float wait = 0f;
    // private bool alert = false;
    // private float alertness = 20f;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (alive)
        {
            if (state == "default")
            {
                // pick one of the points of destinations array
                dest = Random.Range(0, destinations.Length);
                nav.SetDestination(destinations[dest].position);
                state = "walk";
            }
            if (state == "walk")
            {
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "search";
                    wait = 5f;
                }
            }
            if (state == "search")
            {
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                    transform.Rotate(0f, 120f * Time.deltaTime, 0f);
                }
                else
                {
                    state = "default";
                }
            }
            if (state == "hunt")
            {
                nav.destination = player.transform.position;
            }
        }
    }

    public void Sight()
    {
        if (alive)
        {
            RaycastHit rayHit;
            if (Physics.Linecast(eyes.position, player.transform.position, out rayHit))
            {
                if (rayHit.collider.gameObject.name == "Player")
                {
                    if (state != "kill")
                    {
                        state = "hunt";

                        // increase speed when in hunting mode (we can remove this if we want)
                        nav.speed += 3.5f;
                    }
                }
            }
        }
    }

    public void Hunt()
    {
        state = "hunt";
        // NEED TO ADD A TIMER FOR THE HUNT
    }
}