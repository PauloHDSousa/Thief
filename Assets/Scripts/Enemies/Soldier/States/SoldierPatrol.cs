using UnityEngine;
using System.Linq;

public class SoldierPatrol : SoldierBaseState
{

    Vector3? playerLastKnowPosition;
    public override void EnterState(SoldierStateManager soldier)
    {
        playerLastKnowPosition = null;
        soldier.soldierAnim.SawPlayer(false);
        soldier.soldierAnim.SetIsWalking(true);
        soldier.soldierAnim.SetIsRunning(false);
        soldier.navMeshAgent.isStopped = false;
    }
    public override void UpdateState(SoldierStateManager soldier)
    {

        //If didn't heard anythign, Keey hearing
        Hear(soldier);
        
        //Keep looking
        soldier.CheckPlayerAhead(soldier.rangeOfView, soldier.soldierSawPlayer);

        if (playerLastKnowPosition != null)
        {
            if (Vector3.Distance(soldier.transform.position, playerLastKnowPosition.Value) < 1)
            {
                playerLastKnowPosition = null;
                soldier.SwitchState(soldier.soldierPlayerMissing);
            }
        }
        else
            GoToPatrolPoints(soldier);

    }

    public override void OnCollisionEnter(SoldierStateManager soldier)
    {

    }

    void GoToPatrolPoints(SoldierStateManager soldier)
    {

        Transform targetPatrolToGo = soldier.patrolPoints[soldier.currentPoint];
        Transform transform = soldier.transform;

        //If reach the position, go to IDLE then Next position
        if (Vector3.Distance(transform.position, targetPatrolToGo.position) < 1)
        {
            if (soldier.currentPoint == soldier.patrolPoints.Length - 1)
                soldier.currentPoint = 0;
            else
                soldier.currentPoint += 1;

            soldier.SwitchState(soldier.soldierIdle);
        }
        else
            soldier.navMeshAgent.SetDestination(targetPatrolToGo.position);

    }


    void Hear(SoldierStateManager soldier)
    {
       if (playerLastKnowPosition == null)
        {
            var colliders = Physics.OverlapSphere(soldier.transform.position, soldier.rangeOfSoundHeard);
            var collider = colliders.Where(c => c.tag == "PlayerLastKnowPosition").FirstOrDefault();

            if (collider != null)
            {
                playerLastKnowPosition = collider.transform.position;
                soldier.navMeshAgent.SetDestination(playerLastKnowPosition.Value);
            }
        }
    }
}
