using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
    public TextMeshProUGUI interactText;
    private AudioSource footstepSoundSource;

    private float rotX = 0.0f;
    private float rotY = 0.0f;
    public float mouseSensitivity = 50.0f;

    public float interactDistance = 2.0f;

    public AudioClip[] footstepSounds;
    private int footstepSoundIndex;
    private int previousFootstepSoundIndex;
    bool playedFootsteps;
    public float footstepDelay = 0.3f;

    private Vector3 velocity = Vector3.zero;
    private Vector3 wishDir = Vector3.zero;
    private float wishSpeed = 0.0f;

    const float brakeSpeed = 8.0f;
    const float maxAcceleration = 20.0f;
    //const float maxAcceleration = 857.25f;
    const float maxSpeed = 3.0f;
    //const float maxSpeed = 285.75f;
    const float gravAcceleration = 50.0f;
    const float groundAccelerationMultiplier = 10.0f;
    const float brakingDeceleration = 10.0f;
    //const float brakingDeceleration = 190.5f;
    const float surfaceFriction = 1.0f;

    public AnimationCurve fadeCurve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 0));
    public Image blinkSheet;
    public bool eyesClosed = false;
    bool eyesToggledClosed = false;

    public bool isAlive = true;


    // Start is called before the first frame update
    void Start()
    {
        charControl = gameObject.GetComponent<CharacterController>();
        footstepSoundSource = gameObject.GetComponent<AudioSource>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playedFootsteps = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                InteractPhysical();
            }

            ShowInteractText();

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

            // Debug Information
            /*groundedDisplay.text = "Grounded: " + charControl.isGrounded;
            speedDisplay.text = "Speed: " + velocity.magnitude;
            accelDisplay.text = "Accel: " + wishDir.magnitude;*/
        }
    }

    void Movement()
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

        if (!charControl.isGrounded)
        {
            StopCoroutine(PlayFootstep());
        }

        if (wishSpeed > 0 && charControl.isGrounded && !playedFootsteps)
        {
            playedFootsteps = true;
            StartCoroutine(PlayFootstep());
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

    IEnumerator PlayFootstep()
    {
        footstepSoundIndex = Random.Range(0, footstepSounds.Length);
        while (footstepSoundIndex == previousFootstepSoundIndex)
        {
            footstepSoundIndex = Random.Range(0, footstepSounds.Length);
        }
        AudioClip randomFootstepSound = footstepSounds[footstepSoundIndex];
        footstepSoundSource.PlayOneShot(randomFootstepSound);
        previousFootstepSoundIndex = footstepSoundIndex;
        yield return new WaitForSeconds(footstepDelay);
        playedFootsteps = false;
    }

    void InteractPhysical()
    {
        InteractableObject objectInFront = ObjectInView();
        if (objectInFront != null)
        {
            objectInFront.Interact(gameObject);
        }
        else
        {
            return;
        }
    }

    void ShowInteractText()
    {
        InteractableObject objectInFront = ObjectInView();
        if (objectInFront != null)
        {
            interactText.enabled = true;
            interactText.text = objectInFront.interactText;
        }
        else
        {
            interactText.text = "";
            interactText.enabled = false;
        }
    }

    public InteractableObject ObjectInView()
    {
        RaycastHit hitObject;
        Ray interactRay = new Ray(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), playerCam.transform.forward);
        if (Physics.Raycast(interactRay, out hitObject, interactDistance))
        {
            if (hitObject.collider.GetComponent<InteractableObject>() == null)
            {
                return null;
            }
            InteractableObject interactable = hitObject.collider.GetComponent<InteractableObject>();
            return interactable;
        }
        else
        {
            return null;
        }
    }

    public Vector3 RaycastView()
    {
        RaycastHit hitObject;
        Ray interactRay = new Ray(playerCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f)), playerCam.transform.forward);
        if (Physics.Raycast(interactRay, out hitObject, interactDistance))
        {
            return hitObject.point;
        }
        else
        {
            Debug.Log("No valid point, returning player position.");
            return Vector3.zero;
        }
    }

    public void ToggleEyesClosed()
    {

    }

    public void OpenEyes()
    {

    }

    /*private void Movement()
    {
        ApplyFriction();

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
    }*/

    /*private void ApplyFriction()
    {
        //Friction

        float relSpeed = velocity.magnitude;
        if (relSpeed <= 0.1f)
        {
            return;
        }

        float localDeceleration = Mathf.Max(brakingDeceleration, relSpeed);

        Vector3 oldVel = velocity;

        Vector3 revAccel = localDeceleration * velocity.normalized;
        velocity -= revAccel * Time.deltaTime;

        if (Vector3.Dot(velocity, oldVel) <= 0.0f)
        {
            velocity = Vector3.zero;
            return;
        }
    }*/

}
