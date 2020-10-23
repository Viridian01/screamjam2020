using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalDoor : InteractableObject
{
    private NarrationText narrationText;

    void Awake()
    {
        objName = "Door";
        interactText = "Escape this nightmare.";
        narrationText = FindObjectOfType<NarrationText>();
    }

    public override void Interact(GameObject user)
    {
        base.Interact(user);
        narrationText.DisplayText("Office Hours are over.", 5);
    }
}
