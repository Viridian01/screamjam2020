﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public PlayerEngine player;
    public Camera cam;
    public Transform monsterDir;
    public MonsterAI monster;
    public float fieldOfView = 60f;
    public CloseEyes eyes;

    public SanityBar sanityBar;
    public int maxSanity = 100;
    public int currentSanity;
    private int dec = 1;
    public float wait = 0f;

    private void Start()
    {
        player = FindObjectOfType<PlayerEngine>();
        currentSanity = maxSanity + 1;
        sanityBar.SetMaxSanity(maxSanity);
    }

    // FOV = cone or whatever
    // Line of Sight = raycast and there's something in the way
    private void Update()
    {
        // check sanity meter alive state
        if (currentSanity == 0)
        {
            print("player is dead");
            player.isAlive = false;
        }

        // if eyes are opened decrease sanity meter
        if (!player.eyesClosed)
        {
            if (wait > 0f)
            {
                print("delay");
                wait -= Time.deltaTime;
            }
            else
            {
                decreaseSanity(dec);
            }
        }
        // if eyes are closed regenerate sanity meter
        else if (player.eyesClosed)
        {
            if (wait > 0f)
            {
                print("delay");
                wait -= Time.deltaTime;
            }
            else
            {
                increaseSanity(dec);
            }
        }

        // check if player is looking at the monster
        if (CheckMonster())
        {
            monster.HuntEyes(transform.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyEyes")
        {
            other.transform.parent.GetComponent<MonsterAI>().Sight();
        }
    }

    private void decreaseSanity(int dec)
    {
        int check = currentSanity - dec;
        if (check >= 0)
        {
            print("decreasing sanity");
            currentSanity -= dec;
            sanityBar.SetSanity(currentSanity);
            wait = 0.5f;
        }
    }

    private void increaseSanity(int dec)
    {
        int check = currentSanity + dec;
        if (check <= maxSanity)
        {
            print("increasing sanity");
            currentSanity += dec;
            sanityBar.SetSanity(currentSanity);
            wait = 0.5f;
        }
    }

    // Checks if monster is in FOV
    bool CheckMonster()
    {
        Vector3 dir = monsterDir.position - transform.position;
        float angle = Vector3.Angle(cam.transform.forward, dir);
        bool isInFov = false;   // FOV check

        if (angle <= fieldOfView)
        {
            isInFov = true;
        }

        return isInFov && CheckLoS();
    }

    public bool CheckLoS()
    {
        Vector3 dir = monsterDir.position - transform.position;

        bool isInLos = false;   // Line of Sight check
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, dir, out hit))
        {
            if (hit.transform == monsterDir)
            {
                isInLos = true;
            }
        }

        return isInLos;
    }
}
