using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item
{
    protected string itemName;
    protected Image itemIcon;

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

    public Image Icon
    {
        get
        {
            return itemIcon;
        }
    }

    public virtual void Use()
    {
        Debug.Log("Used: " + itemName + ".");
    }
}
