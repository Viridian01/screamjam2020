using TMPro;
using UnityEngine;

public class PlayerEngine : MonoBehaviour
{

    struct Cmd
    {
        public float forward;
        public float side;
    }

    Cmd playerInput;

    CharacterController charControl;
    public Camera playerCam;
    public TextMeshProUGUI speedDisplay;
    public TextMeshProUGUI accelDisplay;
    public TextMeshProUGUI groundedDisplay;

    private float rotX = 0.0f;
    private float rotY = 0.0f;


    public float mouseSensitivity = 50.0f;
    //public float xMouseSensitivity = 30.0f;
    //public float yMouseSensitivity = 30.0f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 wishDir = Vector3.zero;
    private float wishSpeed = 0.0f;

    const float brakeSpeed = 8.0f;
    const float maxAcceleration = 10.0f;
    const float maxSpeed = 3.0f;
    const float gravAcceleration = 20.0f;
    //const float groundAccelerationMultiplier = 10.0f;
    //const float surfaceFriction = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        charControl = gameObject.GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        rotX -= Input.GetAxis("Mouse Y") * mouseSensitivity * 0.02f;
        rotY += Input.GetAxis("Mouse X") * mouseSensitivity * 0.02f;

        if (rotX < -90)
        {
            rotX = -90;
        }
        else if (rotX > 90)
        {
            rotX = 90;
        }

        transform.rotation = Quaternion.Euler(0, rotY, 0);
        playerCam.transform.rotation = Quaternion.Euler(rotX, rotY, 0);

        if (velocity.magnitude < 0.01f)
        {
            velocity = Vector3.zero;
        }

        Movement();
        charControl.Move(velocity * Time.deltaTime);

        /* Debug Information
        groundedDisplay.text = "Grounded: " + charControl.isGrounded;
        speedDisplay.text = "Speed: " + velocity.y;
        accelDisplay.text = "Accel: " + wishDir.magnitude;
        */
    }

    private void Movement()
    {
        playerInput.forward = Input.GetAxisRaw("Vertical");
        playerInput.side = Input.GetAxisRaw("Horizontal");

        wishDir = new Vector3(playerInput.side, 0, playerInput.forward);
        wishDir = transform.TransformDirection(wishDir);
        wishDir.Normalize();

        wishSpeed = wishDir.magnitude * maxAcceleration;

        if (!(wishSpeed > 0))
        {
            Vector3 currentDir = velocity.normalized;
            Vector3 brakeVec = currentDir * -brakeSpeed * Time.deltaTime;

            velocity += brakeVec;
        }

        Vector3 accelVec = wishDir * wishSpeed * Time.deltaTime;

        velocity += accelVec;
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        if (charControl.isGrounded)
        {
            velocity.y = -gravAcceleration * Time.deltaTime;
        }
        else
        {
            velocity.y -= gravAcceleration * Time.deltaTime;
        }
    }

    /*private void Movement()
    {
        applyFriction();

        playerInput.forward = Input.GetAxisRaw("Vertical");
        playerInput.side = Input.GetAxisRaw("Horizontal");

        Vector3 inputDir = new Vector3(playerInput.side, 0, playerInput.forward);
        inputDir = transform.TransformDirection(inputDir);
        inputDir.Normalize();
        acceleration = inputDir * maxAcceleration;
        acceleration = Vector3.ClampMagnitude(acceleration, maxSpeed);

        Vector3 accelDir = acceleration.normalized;

        float veer = velocity.x * accelDir.x + velocity.y * accelDir.y;
        float addSpeed = acceleration.magnitude - veer;

        if (addSpeed > 0.0f)
        {
            //Debug.Log(inputDir.x + " " + inputDir.z + " " + acceleration.magnitude);
            acceleration *= groundAccelerationMultiplier * surfaceFriction * Time.deltaTime;
            acceleration = Vector3.ClampMagnitude(acceleration, addSpeed);
            velocity += acceleration;
        }
    }

    private void applyFriction()
    {
        //Friction

        float relSpeed = velocity.magnitude;
        if (relSpeed <= 0.1f)
        {
            return;
        }

        Vector3 oldVel = velocity;

        Vector3 revAccel = brakingDeceleration * velocity.normalized;
        velocity -= revAccel * Time.deltaTime;

        if (Vector3.Dot(velocity, oldVel) <= 0.0f)
        {
            velocity = Vector3.zero;
            return;
        }
    }*/

}
