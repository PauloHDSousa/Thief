using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections.Generic;

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

    [Space(10)]
    [Header("SFX")]
    [SerializeField]
    AudioClip[] die_sfxs;

    //Straw
    GameObject strawRef;
    AudioSource audioSource;

    //Whistle
    bool isUsingSkill = false;
    bool isDead = false;


    //Steal
    bool canSteal = false;
    GameObject currentItemToSteal;
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
        playerInput.CharacterController.Steal.performed += onSteal;

        playerInput.CharacterController.TransformIntoBox.started += onTransformIntoBox;
        playerInput.CharacterController.TransformIntoBox.canceled += goBackToHumanForm;
        playerInput.CharacterController.Pause.performed += onPause;

    }

    private void OnEnable()
    {
        playerInput.CharacterController.Enable();
    }

    private void OnDisable()
    {
        playerInput.CharacterController.Disable();
    }

    void onPause(InputAction.CallbackContext ctx)
    {
        GameManager.Instance.ShowMenu();
    }

    void onSteal(InputAction.CallbackContext ctx)
    {
        if (canSteal)
        {
            canSteal = false;
            if (currentItemToSteal != null)
            {
                isUsingSkill = true;
                Vector3 objectPosition = new Vector3(currentItemToSteal.gameObject.transform.position.x, 0, currentItemToSteal.gameObject.transform.position.z);
                transform.LookAt(objectPosition);
                animController.Steal();
            }
        }
    }

    public void StealItemDuringAnimation()
    {
        var item = currentItemToSteal.GetComponent<StealableItem>();
        Destroy(currentItemToSteal);
        GameManager.Instance.AddWeightOnBar(item.Weight);
        GameManager.Instance.AddGoldValue(item.PriceValue);
        isUsingSkill = false;
    }

    void onWhistle(InputAction.CallbackContext ctx)
    {
        isUsingSkill = true;
        playerSkills.OnWhistle();
    }

    void onWhistleFinished()
    {
        isUsingSkill = false;
    }

    void onTransformIntoBox(InputAction.CallbackContext ctx)
    {
        isUsingSkill = true;
        playerSkills.onTransformIntoBox(playerCapsuleCollider, rb);
    }

    void goBackToHumanForm(InputAction.CallbackContext ctx)
    {
        isUsingSkill = false;
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

    private void Update()
    {
        var colliders = Physics.OverlapSphere(transform.position, 1.5f);
        foreach (var hit in colliders)
        {
            if (hit.transform.gameObject.tag == "Stealable")
            {
                currentItemToSteal = hit.transform.gameObject;
                canSteal = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isUsingSkill)
            return;

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
        Gizmos.DrawWireSphere(transform.position, 1.5f);
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
            this.gameObject.tag = "StrawWithPlayer";
            strawRef.tag = "StrawWithPlayer";
            playerSkills.OnInteractWithStraw(strawRef);
        }
        else if (other.CompareTag("Exit"))
        {
            GameManager.Instance.OnExit();
            playerInput.CharacterController.Disable();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("StrawWithPlayer"))
        {
            this.gameObject.tag = "Player";
            other.tag = "Straw";
            playerSkills.OnInteractWithStraw(strawRef);
            strawRef = null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Sword")
            Die();
    }
    public void Die()
    {
        if (isDead)
            return;

        isDead = true;
        AudioClip die_sfx = GetRandomDeathSound();
        audioSource.PlayOneShot(die_sfx);
        animController.Die();
        this.gameObject.tag = "DeadPlayer";
        playerInput.CharacterController.Disable();
        GameManager.Instance.ShowMenu();
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

    public AudioClip GetRandomDeathSound()
    {
        return die_sfxs[UnityEngine.Random.Range(0, die_sfxs.Length)];
    }

}