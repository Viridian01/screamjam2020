using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keycard : Item
{

    void Awake()
    {
        itemName = "Keycard";
        interactText = "Pick up the " + itemName;
    }

    public override void Interact()
    {
        
    }
}
