using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;

public class SoldierStateManager : MonoBehaviour
{
    SoldierBaseState currentState;

    //States
    public SoldierIdle soldierIdle = new SoldierIdle();
    public SoldierPatrol soldierPatrol = new SoldierPatrol();
    public SoldierFollow soldierFollow = new SoldierFollow();
    public SoldierSawPlayer soldierSawPlayer = new SoldierSawPlayer();
    public SoldierPlayerMissing soldierPlayerMissing = new SoldierPlayerMissing();

    public SoldierAnimController soldierAnim { get; private set; }
    public NavMeshAgent navMeshAgent { get; private set; }

    public AudioSource audioSource { get; private set; }


    [Space(10)]
    [Header("SFX")]
    [SerializeField]
    public AudioClip sawThePlayer;

    [SerializeField]
    public AudioClip[] footsteps;

    [SerializeField]
    public AudioClip attack;

    [Space(10)]
    [Header("VFX")]
    [SerializeField]
    public ParticleSystem vfxSawThePlayer;

    [Space(10)]
    [Header("General")]
    [SerializeField]
    public LayerMask playerMask;

    [SerializeField]
    public LayerMask boxMask;

    [SerializeField]
    float radiusOfSenseAura = 1.6f;

    [SerializeField]
    public float numberOfAdditionalRC = 6f;

    [Space(10)]
    [Header("Patrol")]
    [SerializeField]
    public Transform[] patrolPoints;

    [SerializeField]
    public float moveSpeed = 1.5f;

    [SerializeField]
    public float rangeOfView = 5f;

    [SerializeField]
    public float rangeOfViewBehind = 3f;

    [SerializeField]
    public float rangeOfViewSideways = 2f;


    [SerializeField]
    public float rangeOfSoundHeard = 5f;

    [Space(10)]
    [Header("Follow")]
    [SerializeField]
    public float runSpeed = 3f;

    [SerializeField]
    public float rangeOfViewWhileFollowing = 2f;

    [SerializeField]
    public GameObject playerObject;

    public int currentPoint;
    public Rigidbody rb { get; private set; }

    public bool isPlayedTurnedIntoABox;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        soldierAnim = GetComponent<SoldierAnimController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        currentState = soldierIdle;
        currentState.EnterState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(SoldierBaseState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, rangeOfSoundHeard);
    }
    public bool CheckIfLayerAhead(float range, string[] tag)
    {

        Vector3 posA = transform.position;
        Vector3 posB = navMeshAgent.destination;
        Vector3 dir = (posB - posA).normalized;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 forwardAnglesUp = forward;
        Vector3 forwardAnglesDown = forward;

        Vector3 backwardsdAnglesUp = -forward;
        Vector3 backwardsdAnglesDown = -forward;

        RaycastHit hit;
        Debug.DrawRay(transform.position, forward * range, Color.yellow);

        if (Physics.Raycast(transform.position, forward, out hit, range))
            if (tag.Contains(hit.transform.gameObject.tag))
                return true;

        int i = 0;
        double z = Math.Round(dir.z);

        while (i < numberOfAdditionalRC)
        {

            //Going Down
            if (z == 1 || z == -1)
            {
                forwardAnglesUp.x += 0.1f;
                forwardAnglesDown.x -= 0.1f;

                backwardsdAnglesUp.x += 0.1f;
                backwardsdAnglesDown.x -= 0.1f;
            }
            else
            {
                forwardAnglesUp.z += 0.1f;
                forwardAnglesDown.z -= 0.1f;

                backwardsdAnglesUp.z += 0.1f;
                backwardsdAnglesDown.z -= 0.1f;
            }

            //Forward
            if (AnyHit(forwardAnglesUp, tag, range))
                return true;
            if (AnyHit(forwardAnglesDown, tag, range))
                return true;

            //BACK
            if (AnyHit(backwardsdAnglesUp, tag, rangeOfViewBehind))
                return true;

            if (AnyHit(backwardsdAnglesDown, tag, rangeOfViewBehind))
                return true;
            i++;
        }

        //Left - Right
        var right = (transform.forward + transform.right);
        var left = (transform.forward - transform.right);

        var middleRight = (transform.right);
        var middleLeft = (-transform.right);

        if (AnyHit(right, tag, rangeOfViewSideways))
            return true;

        if (AnyHit(left, tag, rangeOfViewSideways))
            return true;

        if (AnyHit(middleRight, tag, rangeOfViewSideways))
            return true;

        if (AnyHit(middleLeft, tag, rangeOfViewSideways))
            return true;

        return false;
    }

    bool AnyHit(Vector3 dir, string[] tag, float distance)
    {

        RaycastHit hit;
        Debug.DrawRay(transform.position, dir * distance, Color.red);
        if (Physics.Raycast(transform.position, dir, out hit, distance))
            if (tag.Contains(hit.transform.gameObject.tag))
                return true;

        return false;
    }


    public void CheckPlayerAhead(float range, SoldierBaseState state)
    {
        if (CheckIfLayerAhead(range, new string[] { "Player" }))
            OnRCHitPlayer(state);
    }

    void OnRCHitPlayer(SoldierBaseState state)
    {
        SwitchState(state);
    }

    public void InstantiateVFX(ParticleSystem particle)
    {
        Instantiate(particle, transform);
    }

    public void Destroy(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void Step()
    {
        AudioClip step = GetRandomStep();
        audioSource.PlayOneShot(step);
    }

    public AudioClip GetRandomStep()
    {
        return footsteps[UnityEngine.Random.Range(0, footsteps.Length)];
    }

    public void Attack()
    {
        audioSource.PlayOneShot(attack);
    }

}
