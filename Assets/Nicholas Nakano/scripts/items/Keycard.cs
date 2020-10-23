using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : Item
{
    public string keyCardName;
    void Awake()
    {
        itemName = keyCardName;
        interactText = "Pick up the " + itemName;
    }

    public override void Interact()
    {
        base.Interact();
    }
}
