using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public PlayerEngine player;
    private NavMeshAgent nav;
    public Transform[] destinations;

    private enum State
    {
        Walk,
        Wait,
        Hunt
    }

    private State currentState = State.Walk;

    public Transform eyes;
    public Vector3 monsterPos;
    public float wait = 0f;
    // private bool alert = false;
    // private float alertness = 20f;

    private LinkedDest currentNode = null;
    static LinkedDest[] nodeCache;

    void Start()
    {
        player = FindObjectOfType<PlayerEngine>();
        monsterPos = gameObject.transform.position;
        nav = GetComponent<NavMeshAgent>();
        InitializeNodeCache();
        GoToNode(ClosestNode());
    }

    void Update()
    {
        if (player.isAlive)
        {
            if (currentState == State.Walk)
            {
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    currentState = State.Wait;
                    wait = 3f;
                }
            }
            if (currentState == State.Wait)
            {
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                }
                else
                {
                    GoToNode(currentNode.GetNext());
                    currentState = State.Walk;
                }
            }
            if (currentState == State.Hunt)
            {
                nav.destination = player.transform.position;
                // when hunting time is over, return to default state
            }
        }
    }

    private void GoToNode(LinkedDest nodeToGoTo)
    {
        nav.SetDestination(nodeToGoTo.nodeLocation);
        if (nav.pathStatus == NavMeshPathStatus.PathComplete)
        {
            currentNode = nodeToGoTo;
        }
    }

    private void InitializeNodeCache()
    {
        nodeCache = FindObjectsOfType<LinkedDest>();
    }

    private LinkedDest ClosestNode()
    {
        LinkedDest closestNode;
        closestNode = nodeCache[0];
        for (int i = 0; i < nodeCache.Length; i++)
        {
            if (Vector3.Distance(monsterPos, nodeCache[i].nodeLocation) < Vector3.Distance(monsterPos, closestNode.nodeLocation))
            {
                closestNode = nodeCache[i];
            }
        }
        return closestNode;
    }

    // enemy eyes
    public void Sight()
    {
        if (player.isAlive)
        {
            RaycastHit rayHit;
            if (Physics.Linecast(eyes.position, player.transform.position, out rayHit))
            {
                if (rayHit.collider.gameObject.name == "Player")
                {
                    currentState = State.Hunt;
                    /*if (currentState != "kill")
                    {
                        state = "hunt";

                        //nav.speed += 3.5f;
                    }*/
                }
            }
        }
    }

    public void Hunt(Vector3 pos)
    {
        // monster goes to player's last seen position
        nav.SetDestination(pos);
    }
}