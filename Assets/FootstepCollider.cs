using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootstepCollider : MonoBehaviour
{
    public GameObject playerObject;
    private PlayerEngine player;
    public enum floorTypes {Concrete, Wood, Metal, Tile}

    public floorTypes floor;

    // Start is called before the first frame update
    void Start()
    {
        player=playerObject.GetComponent<PlayerEngine>();
    }

    void OnCollisionEnter(Collision collision)
    {
        player.setFloorType((int)floor);
    }
}
