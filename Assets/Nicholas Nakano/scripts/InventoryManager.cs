using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    int inventorySlots = 4;

    Item[] itemArray;

    PlayerEngine player;

    // Start is called before the first frame update
    void Start()
    {
        itemArray = new Item[inventorySlots];
        player = gameObject.GetComponent<PlayerEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DropItem(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DropItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DropItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DropItem(3);
        }
    }

    public void AddItem(Item addedItem)
    {
        for (int i = 0; i < itemArray.Length; i++)
        {
            if (itemArray[i] == null)
            {
                itemArray[i] = addedItem;
                break;
            }
        }
    }

    public void DropItem(int itemSlot)
    {
        Item itemToDrop = itemArray[itemSlot];
        if (itemToDrop != null)
        {
            Vector3 pointToDrop = player.PosInView();
            if (pointToDrop != Vector3.zero)
            {
                itemToDrop.gameObject.SetActive(true);
                itemToDrop.transform.position = pointToDrop;
                itemArray[itemSlot] = null;
            }
        }
    }
}
