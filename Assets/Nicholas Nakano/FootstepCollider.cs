using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootstepCollider : MonoBehaviour
{
    public GameObject playerObject;
    private PlayerEngine player;
    public enum FloorTypes {Concrete, Wood, Metal, Tile}

    public FloorTypes floor;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<PlayerEngine>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerObject)
        {
            player.SetFloorType((int)floor);
        }
    }
}
