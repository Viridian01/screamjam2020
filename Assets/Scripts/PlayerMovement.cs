using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 10f;
    public float gravity = 9.81f;

    Vector3 velocity;

    // Update is called once per frame
    void Update()
    {


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move *= speed;

        move -= Vector3.up * gravity;

        controller.Move(move * Time.deltaTime);

    }
}
