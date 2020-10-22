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
    private bool spawn = true;
    private int pathPicker;

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
                // dest = Random.Range(0, destinations.Length);
                // nav.SetDestination(destinations[dest].position);

                if (spawn)
                {
                    nav.SetDestination(destinations[0].position);
                    spawn = false;
                    dest = 0;
                }
                else
                {
                    pathPicker = Random.Range(0, 2);
                    PickPath(pathPicker);
                }

                state = "walk";
            }
            if (state == "walk")
            {
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    state = "search";
                    wait = 3f;
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
                // when hunting time is over, return to default state
            }
        }
    }

    private void PickPath(int picker)
    {
        if (picker == 0)
        {
            print("going to previous");
            if (destinations[dest].GetComponent<LinkedDest>().GetPrev())
            {
                nav.SetDestination(destinations[dest].GetComponent<LinkedDest>().GetPrev().position);

                // find current pos in list
                for (int i = 0; i < destinations.Length; i++)
                {
                    if (destinations[i].position == destinations[dest].GetComponent<LinkedDest>().GetPrev().position)
                    {
                        dest = i;
                    }
                }
            }
        }
        if (picker == 1)
        {
            print("going to next");
            if (destinations[dest].GetComponent<LinkedDest>().GetNext())
            {
                nav.SetDestination(destinations[dest].GetComponent<LinkedDest>().GetNext().position);

                // find current pos in list
                for (int i = 0; i < destinations.Length; i++)
                {
                    if (destinations[i].position == destinations[dest].GetComponent<LinkedDest>().GetNext().position)
                    {
                        dest = i;
                    }
                }
            }
        }
    }

    // enemy eyes
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

                        //nav.speed += 3.5f;
                    }
                }
            }
        }
    }

    public void Hunt(Vector3 pos)
    {
        // monster goes to player's last seen position
        nav.destination = pos;
    }
}