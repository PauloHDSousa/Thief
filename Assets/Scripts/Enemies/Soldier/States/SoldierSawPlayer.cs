using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSawPlayer : SoldierBaseState
{
    float elapsedTime;
    float timeLimitToFollowAndAttack = 1.2f;

    public override void EnterState(SoldierStateManager soldier)
    {
        soldier.InstantiateVFX(soldier.vfxSawThePlayer);
        soldier.audioSource.PlayOneShot(soldier.sawThePlayer);
        soldier.soldierAnim.SawPlayer(true);
        soldier.navMeshAgent.isStopped = true;
    }

    public override void UpdateState(SoldierStateManager soldier)
    {
        soldier.soldierAnim.SawPlayer(false);

        //After X seconds, allowToAttackAndFollow
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= timeLimitToFollowAndAttack)
        {
            elapsedTime = 0f;
            soldier.SwitchState(soldier.soldierFollow);
        }

    }

    public override void OnCollisionEnter(SoldierStateManager soldier)
    {
    }
}
