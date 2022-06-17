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

    public void SetIsWalking(bool isWalking)
    {
        animator.SetBool(isWalkingHash, isWalking);
    }

    public void Whistle()
    {
        animator.SetTrigger("Whistle");
    }

    public void Die()
    {
        animator.SetTrigger("Die");
    }
    public void Steal()
    {
        animator.SetTrigger("PickUp");
    }
}
