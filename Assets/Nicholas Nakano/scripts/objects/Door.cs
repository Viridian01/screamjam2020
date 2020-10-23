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

    public bool isLocked;
    public string correspondingCard;

    void Awake()
    {
        objName = "Door";
        interactText = "Use the door.";
        doorAnimator = gameObject.GetComponent<Animator>();
        if (isLocked)
        {
            interactText = "This door requires the " + correspondingCard + " to open.";
        }
    }

    public override void Interact(GameObject user)
    {
        if (!isLocked)
        {
            OpenDoor(user);
        }
        else
        {
            InventoryManager playerInventory = user.GetComponent<InventoryManager>();
            if (playerInventory != null)
            {
                if (playerInventory.CheckForItem(correspondingCard))
                {
                    OpenDoor(user);
                }
            }
        }
    }

    DoorSides CalculateSide(Vector3 interactPos)
    {
        Vector3 relativePos = gameObject.transform.InverseTransformPoint(interactPos);
        if (relativePos.x > 0)
        {
            return DoorSides.Inwards;
        }
        else
        {
            return DoorSides.Outwards;
        }
    }

    void OpenDoor(GameObject user)
    {
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

        base.Interact(user);
    }
}
