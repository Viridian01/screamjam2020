using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSight : MonoBehaviour
{
    public bool alive = true;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "EnemyEyes")
        {
            other.transform.parent.GetComponent<MonsterAI>().Sight();
        }
    }
}
