using UnityEngine;
using System.Linq;

public class SoldierIdle : SoldierBaseState
{
    float elapsedTime;
    float timeLimitToChangeState = 1.5f;
    Vector3? playerLastKnowPosition = null;

    public override void EnterState(SoldierStateManager soldier)
    {
        playerLastKnowPosition = null;
        soldier.soldierAnim.SetPlayerIsMissing(false);
        soldier.soldierAnim.SetIsWalking(false);
        soldier.navMeshAgent.isStopped = true;
        soldier.navMeshAgent.speed = soldier.moveSpeed;
    }

    public override void UpdateState(SoldierStateManager soldier)
    {

        //If didn't heard anythign, Keey hearing
        Hear(soldier);

        //Always looking for the player
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
        {
            //After X seconds, Changes to Follow STATE
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeLimitToChangeState)
            {
                soldier.SwitchState(soldier.soldierPatrol);
                elapsedTime = 0f;
            }
        }
    }

    public override void OnCollisionEnter(SoldierStateManager stateManager)
    {

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
                soldier.soldierAnim.SetIsWalking(true);
                soldier.navMeshAgent.isStopped = false;
            }
        }
    }
}
