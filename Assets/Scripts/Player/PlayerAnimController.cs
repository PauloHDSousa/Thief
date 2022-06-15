using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{

    Animator animator;

    int isWalkingHash;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsWalking");
    }

    void Start()
    {

    }

    public void SetIsWalking(bool isWalking)
    {
        animator.SetBool(isWalkingHash, isWalking);
    }

    public void Whistle()
    {
        animator.SetTrigger("Whistle");
    }
}
