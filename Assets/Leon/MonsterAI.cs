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
        Hunt,
        Kill
    }

    private State currentState = State.Walk;
    public Transform eyes;
    public Vector3 monsterPos;
    public float wait = 0f;
    private LinkedDest currentNode = null;
    static LinkedDest[] nodeCache;
    public MonsterAnimate animator;

    bool wasLookedAt = false;

    void Start()
    {
        player = FindObjectOfType<PlayerEngine>();
        monsterPos = gameObject.transform.position;
        nav = GetComponent<NavMeshAgent>();
        InitializeNodeCache();
        // GoToNode(ClosestNode());
        currentNode = ClosestNode();
        GoToNode(currentNode);
    }

    void Update()
    {
        if (player.isAlive)
        {
            if (currentState == State.Walk)
            {
                // print("WALK");
                // print(nav.remainingDistance + " " + nav.stoppingDistance);
                if (nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
                {
                    currentState = State.Wait;
                    wait = 3f;
                    wasLookedAt = false;
                }
            }
            if (currentState == State.Wait)
            {
                // print("WAIT");
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
                // print("HUNT");
                // when hunting time is over, return to default state
                if (wait > 0f)
                {
                    // print("hunt wait time: " + wait);
                    nav.destination = player.transform.position;
                    wait -= Time.deltaTime;
                }
                else
                {
                    currentState = State.Walk;
                    GoToNode(currentNode.GetNext());
                    wasLookedAt = false;
                }
            }
            if (currentState == State.Kill)
            {
                // on collision between monster and player, game over screen?
                // print("Player Dead");
                animator.OpenEyesDie();
                player.isAlive = false;
                wasLookedAt = false;
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
                    // time monster will chase the player
                    wait = 3f;
                }
            }
        }
    }

    public bool IsHunting()
    {
        return currentState == State.Hunt;
    }
    public bool WasLookedAt()
    {
        return wasLookedAt;
    }

    public void HuntEyes(Vector3 pos)
    {
        wasLookedAt = true;
        if (player.isAlive)
        {
            // print("path status: " + nav.pathStatus);
            // monster goes to player's last seen position
            nav.SetDestination(pos);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            currentState = State.Kill;
        }

        if (other.gameObject.tag == "Door")
        {
            currentNode = ClosestNode();
            GoToNode(currentNode);
            currentState = State.Walk;
        }
    }
}