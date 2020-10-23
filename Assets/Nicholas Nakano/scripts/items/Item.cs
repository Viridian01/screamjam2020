using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : InteractableObject
{
    protected string itemName;
    Image itemIcon;

    void Awake()
    {
        itemName = "Default Item";
        interactText = "Pick up the " + itemName;
    }

    public string Name
    {
        get
        {
            return itemName;
        }
        set
        {
            itemName = value;
        }
    }

    public override void Interact(GameObject obj)
    {
        Debug.Log("Picked up: " + itemName);
        InventoryManager inventory = obj.GetComponent<InventoryManager>();
        if (inventory != null)
        {
            inventory.AddItem(this);
        }
        gameObject.SetActive(false);

        base.Interact(obj);
    }
}
