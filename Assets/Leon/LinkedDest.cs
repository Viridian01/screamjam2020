using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedDest : MonoBehaviour
{
    public LinkedDest[] next = new LinkedDest[1];
    public Vector3 nodeLocation;

    void Start()
    {
        nodeLocation = gameObject.transform.position;
    }

    public LinkedDest GetNext()
    {
        if (next.Length < 2)
        {
            return next[0];
        }
        else
        {
            return next[Random.Range(0, next.Length)];
        }
    }
}
