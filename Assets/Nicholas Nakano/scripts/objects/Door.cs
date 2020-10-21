using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorSides
{
    Outwards,
    Inwards
}
public class Door : InteractableObject
{
    private Animator doorAnimator;

    void Awake()
    {
        objName = "Door";
        interactText = "Use the door.";
        doorAnimator = gameObject.GetComponent<Animator>();
    }

    public override void Interact(GameObject user)
    {
        Debug.Log("Used Door");

        switch (CalculateSide(user.transform.position))
        {
            case DoorSides.Inwards:
                doorAnimator.SetBool("openOut", false);
                break;
            case DoorSides.Outwards:
                doorAnimator.SetBool("openOut", true);
                break;
        }
        doorAnimator.SetTrigger("OpenDoor");
    }

    DoorSides CalculateSide(Vector3 interactPos)
    {
        Vector3 relativePos = gameObject.transform.InverseTransformPoint(interactPos);
        if (relativePos.x > 0)
        {
            Debug.Log("Open Inwards");
            return DoorSides.Inwards;
        }
        else
        {
            Debug.Log("Open Outwards");
            return DoorSides.Outwards;
        }
    }
}
