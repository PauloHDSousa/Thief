using UnityEngine;

public class SoldierFollow : SoldierBaseState
{

    public override void EnterState(SoldierStateManager soldier)
    {
        GameManager.Instance.timesPLayerSeenByGuards++;
        soldier.navMeshAgent.isStopped = false;
        soldier.soldierAnim.SetIsRunning(true);
        soldier.soldierAnim.SetPlayerIsMissing(false);
        soldier.navMeshAgent.speed = soldier.runSpeed;
        GameManager.Instance.PlayRunSong();
    }
    public override void UpdateState(SoldierStateManager soldier)
    {
        if (soldier.CheckIfLayerAhead(soldier.rangeOfViewWhileFollowing, new string[] { "PlayerBox", "StrawWithPlayer", "DeadPlayer" }))
            soldier.SwitchState(soldier.soldierPlayerMissing);
        else
        {
            soldier.navMeshAgent.SetDestination(soldier.playerObject.transform.position);
            float dist = Vector3.Distance(soldier.playerObject.transform.position, soldier.transform.position);
          
            if(dist < 2)
                soldier.soldierAnim.SetAttack(true);
            else
                soldier.soldierAnim.SetAttack(false);
        }
    }


    public override void OnCollisionEnter(SoldierStateManager soldier)
    {

    }
}
