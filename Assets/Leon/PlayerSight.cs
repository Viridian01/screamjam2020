using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public Camera cam;
    public Transform monsterDir;
    public MonsterAI monster;
    public float fieldOfView = 60f;

    public bool alive = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "EnemyEyes")
        {
            other.transform.parent.GetComponent<MonsterAI>().Sight();
        }
    }

    // FOV = cone or whatever
    // Line of Sight = raycast and there's something in the way
    private void Update()
    {
        // CHECK IF WE're closing our eyes...

        print("check: " + CheckMonster());
        if(CheckMonster())
        {
            print("hunting");
            monster.Hunt();
        }
    }

    /// <summary>
    /// Checks if monster is in FOV
    /// </summary>
    /// <returns>True if yes.</returns>
    bool CheckMonster()
    {
        Vector3 dir = monsterDir.position - transform.position;
        float angle = Vector3.Angle(cam.transform.forward, dir);
        bool isInFov = false;   // FOV check

        if(angle <= fieldOfView)
        {
            isInFov = true;
        }

        bool isInLos = false;   // Line of Sight check
        RaycastHit hit;
        if(isInFov && Physics.Raycast(cam.transform.position, dir, out hit))
        {
            if(hit.transform == monsterDir)
            {
                isInLos = true;
            }
        }

        return isInFov && isInLos;
    }
}
