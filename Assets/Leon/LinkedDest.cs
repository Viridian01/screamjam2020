using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedDest : MonoBehaviour
{
    public Transform previous = null;
    public Transform next = null;

    public Transform GetPrev()
    {
        return previous;
    }

    public Transform GetNext()
    {
        return next;
    }
}
