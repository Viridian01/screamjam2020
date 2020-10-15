using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngineTest : MonoBehaviour
{

    struct Cmd
    {
        public float forward;
        public float side;
    }

    Cmd playerInput;

    CharacterController charControl;
    Camera mainCam;

    private float rotX = 0.0f;
    private float rotY = 0.0f;


    public float mouseSensitivity = 50.0f;
    //public float xMouseSensitivity = 30.0f;
    //public float yMouseSensitivity = 30.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 acceleration = Vector3.zero;

    const float maxAcceleration = 10.0f;
    const float groundAccelerationMultiplier = 10.0f;
    const float surfaceFriction = 0.1f;
    const float slowDownAmount = 3f;

    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        charControl = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity * 0.02f;
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity * 0.02f;

        if (rotX < -90)
        {
            rotX = -90;
        }
        else if(rotX > 90)
        {
            rotX = 90;
        }

        transform.rotation = Quaternion.Euler(0, rotY, 0);
        mainCam.transform.rotation = Quaternion.Euler(rotX, rotY, 0);

        Movement();
        charControl.Move(velocity * Time.deltaTime);
    }

    private void Movement()
    {
        playerInput.forward = Input.GetAxisRaw("Vertical");
        playerInput.side = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = new Vector3(playerInput.side, 0, playerInput.forward);
        inputDir = transform.TransformDirection(inputDir);
        inputDir.Normalize();
        acceleration = inputDir * maxAcceleration;
        acceleration = Vector3.ClampMagnitude(acceleration, maxAcceleration);

        if (acceleration.magnitude > 0.0f)
        {
            Debug.Log(inputDir.x + " " + inputDir.z + " " + inputDir.magnitude);
            acceleration *= groundAccelerationMultiplier * surfaceFriction * Time.deltaTime;
            velocity += acceleration;
        }

        Vector3 deacceleration = charControl.velocity * slowDownAmount * Time.deltaTime;
        if(inputDir != Vector3.zero)
        {
            //deacceleration = Vector3.zero;
        }

        velocity -= deacceleration;
    }

}
