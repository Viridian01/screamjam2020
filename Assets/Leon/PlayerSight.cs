using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public PlayerEngine player;
    public Camera cam;
    //public Transform monsterDir;
    //public MonsterAI monster;
    public float fieldOfView = 60f;
    public CloseEyes eyes;

    public SanityBar sanityBar;
    public int maxSanity = 100;
    public int currentSanity;
    private int dec = 1;
    public float wait = 0f;

    public bool sanModifier;
    private int decMod = 10;

    MonsterAI[] monsters;

    private void Start()
    {
        monsters = FindObjectsOfType<MonsterAI>();

        player = FindObjectOfType<PlayerEngine>();
        currentSanity = maxSanity + 1;
        sanityBar.SetMaxSanity(maxSanity);
        sanModifier = false;
    }

    // FOV = cone or whatever
    // Line of Sight = raycast and there's something in the way
    private void Update()
    {
        if (!player.isAlive)
        {
            sanityBar.SetSanity(0);
        }
        else
        {
            // check sanity meter alive state
            if (currentSanity == 0)
            {
                player.isAlive = false;
            }

            // if eyes are opened decrease sanity meter
            if (!player.eyesClosed)
            {
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                }
                else
                {
                    decreaseSanity(dec, sanModifier);
                }

                // check if player is looking at the monster when eyes are opened
                MonsterAI monster = CheckMonster();
                if (monster)
                {
                    monster.HuntEyes(transform.position);
                    sanModifier = true;
                }
                else
                {
                    sanModifier = false;
                }
            }
            // if eyes are closed regenerate sanity meter
            else if (player.eyesClosed)
            {
                if (wait > 0f)
                {
                    wait -= Time.deltaTime;
                }
                else
                {
                    increaseSanity(dec);
                }
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "EnemyEyes")
        {
            other.transform.parent.GetComponent<MonsterAI>().Sight();
        }
    }

    private void decreaseSanity(int dec, bool monsterLook)
    {
        if (!monsterLook)
        {
            int check = currentSanity - dec;
            if (check >= 0)
            {
                currentSanity -= dec;
                sanityBar.SetSanity(currentSanity);
                wait = 0.2f;
            }
        }
        else if (monsterLook)
        {
            dec = decMod;
            int check = currentSanity - dec;
            if (check > 0)
            {
                currentSanity -= dec;
                sanityBar.SetSanity(currentSanity);
                wait = 0.2f;
            }
            if (check <= 0)
            {
                currentSanity = 0;
                sanityBar.SetSanity(currentSanity);
            }
        }
    }

    private void increaseSanity(int dec)
    {
        int check = currentSanity + dec;
        if (check <= maxSanity)
        {
            currentSanity += dec;
            sanityBar.SetSanity(currentSanity);
            wait = 0.2f;
        }
    }

    // Checks if monster is in FOV
    public MonsterAI CheckMonster()
    {
        foreach(MonsterAI m in monsters)
        {
            Transform monsterDir = m.transform;

            Vector3 dir = (monsterDir.position - transform.position).normalized;
            float angle = Vector3.Angle(cam.transform.forward, dir);
            bool isInFov = false;   // FOV check

            if (angle <= fieldOfView)
            {
                isInFov = true;
            }

            if (isInFov && CheckLoS(m))
            {
                return m;
            }
        }

        return null;
        
    }

    public MonsterAI ClosestMonster()
    {
        MonsterAI closest = null;
        float closestDist = 50f;

        foreach (MonsterAI m in monsters)
        {
            Transform monsterDir = m.transform;

            Vector3 dir = (monsterDir.position - transform.position).normalized;
            float dist = Vector3.Angle(monsterDir.position, transform.position);
            bool inRange = false;

            if (dist <= 50f)
            {
                inRange = true;
            }

            float angle = Vector3.Angle(cam.transform.forward, dir);
            bool isInFov = false;   // FOV check

            if (angle <= fieldOfView)
            {
                isInFov = true;
            }

            if (isInFov || inRange)
            {
                if(dist < closestDist)
                {
                    closest = m;
                    closestDist = dist;
                }
            }
        }

        return closest;

    }

    public bool CheckLoS(MonsterAI m)
    {
        if (!m) return false;

        Transform monsterDir = m.transform;

        Vector3 dir = (monsterDir.position - transform.position).normalized;

        bool isInLos = false;   // Line of Sight check
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, dir, out hit))
        {
            Debug.DrawLine(cam.transform.position, hit.point, Color.green, 2.0f);
            if (hit.transform == monsterDir)
            {
                isInLos = true;
            }
        }
        return isInLos;
    }
}
