using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierAnimController : MonoBehaviour
{
    Animator animator;

    int isWalkingHash;
    int IsRunningHash;
    int SawPlayerHash;
    int MissingPlayerHash;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("IsWalking");
        IsRunningHash = Animator.StringToHash("IsRunning");
        MissingPlayerHash = Animator.StringToHash("IsMissing");
        SawPlayerHash = Animator.StringToHash("SawPlayer");
    }

    public void SetIsWalking(bool isWalking)
    {  
        animator.SetBool(isWalkingHash, isWalking);
    }

    public void SetIsRunning(bool IsRunning)
    {
        animator.SetBool(IsRunningHash, IsRunning);
    }

    public void SawPlayer(bool sawPlayer)
    {
        animator.SetBool(SawPlayerHash, sawPlayer);
    }

    public void SetPlayerIsMissing(bool isMissing)
    {
        animator.SetBool(MissingPlayerHash, isMissing);
    }

}
