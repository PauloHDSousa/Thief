using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFollow : SoldierBaseState
{

    public override void EnterState(SoldierStateManager soldier)
    {
        soldier.navMeshAgent.isStopped = false;
        soldier.soldierAnim.SetIsRunning(true);
        soldier.soldierAnim.SetPlayerIsMissing(false);
        soldier.navMeshAgent.speed = soldier.runSpeed;
    }
    public override void UpdateState(SoldierStateManager soldier)
    {
        if (soldier.CheckIfLayerAhead(soldier.rangeOfViewWhileFollowing, new string[] { "PlayerBox", "StrawWithPlayer" }))
            soldier.SwitchState(soldier.soldierPlayerMissing);
        else
        {
            soldier.navMeshAgent.SetDestination(soldier.playerObject.transform.position);
            float dist = Vector3.Distance(soldier.playerObject.transform.position, soldier.transform.position);
          
            if(dist < 2)
            {
                Debug.Log("Hit player");
            }
        }
    }


    public override void OnCollisionEnter(SoldierStateManager soldier)
    {

    }
}
