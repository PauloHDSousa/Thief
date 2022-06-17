using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    PlayerSkills playerSkills;
    PlayerAnimController animController;
    PlayerInput playerInput;
    bool isMovementPressed;

    //Player movement
    Vector2 currentMovementInput;
    Rigidbody rb;
    CapsuleCollider playerCapsuleCollider;

    [Space(10)]
    [Header("Movement")]
    [SerializeField]
    float speed = 3f;

    [SerializeField]
    AudioClip[] steps;

    //Straw
    GameObject strawRef;
    AudioSource audioSource;
    #region Input Actions

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), ((int)(1.0f / Time.smoothDeltaTime)).ToString());
    }

    private void Awake()
    {
        playerSkills = GetComponent<PlayerSkills>();
        animController = GetComponent<PlayerAnimController>();
        playerInput = new PlayerInput();

        playerInput.CharacterController.Movement.started += onMovementInput;
        playerInput.CharacterController.Movement.canceled += onMovementInput;
        playerInput.CharacterController.Movement.performed += onMovementInput;

        playerInput.CharacterController.ShowBehindWalls.performed += onShowBehindWalls;
        playerInput.CharacterController.Whistle.performed += onWhistle;


        playerInput.CharacterController.TransformIntoBox.started += onTransformIntoBox;
        playerInput.CharacterController.TransformIntoBox.canceled += goBackToHumanForm;

    }
    private void OnEnable()
    {
        playerInput.CharacterController.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterController.Disable();
    }

    void onWhistle(InputAction.CallbackContext ctx)
    {
        playerSkills.OnWhistle();
    }

    void onTransformIntoBox(InputAction.CallbackContext ctx)
    {
        playerSkills.onTransformIntoBox(playerCapsuleCollider, rb);
    }

    void goBackToHumanForm(InputAction.CallbackContext ctx)
    {
        playerSkills.GoBackToHumanForm(playerCapsuleCollider, rb);
    }


    void onShowBehindWalls(InputAction.CallbackContext ctx)
    {
        playerSkills.ShowBehindWalls();
    }

    void onMovementInput(InputAction.CallbackContext ctx)
    {
        currentMovementInput = ctx.ReadValue<Vector2>();
        isMovementPressed = currentMovementInput.x != 0 || currentMovementInput.y != 0;
    }
    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isMovementPressed)
        {
            var direction = new Vector3(currentMovementInput.x, 0, currentMovementInput.y);

            //Rotation
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.fixedDeltaTime);

            rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
            rb.MoveRotation(targetRotation);
        }
        HandleAnimations();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }

    void HandleAnimations()
    {
        animController.SetIsWalking(isMovementPressed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Straw"))
        {
            strawRef = other.gameObject;
            strawRef.tag = "StrawWithPlayer";
            playerSkills.OnInteractWithStraw(strawRef);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StrawWithPlayer"))
        {
            other.tag = "Straw";
            playerSkills.OnInteractWithStraw(strawRef);
            strawRef = null;
        }
    }


    public void Step()
    {
        AudioClip step = GetRandomStep();
        audioSource.PlayOneShot(step);
    }

    public AudioClip GetRandomStep()
    {
        return steps[UnityEngine.Random.Range(0, steps.Length)];
    }

}