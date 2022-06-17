using UnityEngine;

public class SoldierPlayerMissing : SoldierBaseState
{
    float elapsedTime;
    float TimeToPatrolAgain = 2f;

    public override void EnterState(SoldierStateManager soldier)
    {
        soldier.soldierAnim.SetIsWalking(false);
        soldier.soldierAnim.SetPlayerIsMissing(true);
        soldier.soldierAnim.SetIsRunning(false);
        soldier.navMeshAgent.isStopped = true;
        GameManager.Instance.StopRunSong();
    }
    public override void UpdateState(SoldierStateManager soldier)
    {
        soldier.CheckPlayerAhead(soldier.rangeOfView, soldier.soldierSawPlayer);

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= TimeToPatrolAgain)
        {
            elapsedTime = 0f;
            soldier.soldierAnim.SetPlayerIsMissing(false);
            soldier.SwitchState(soldier.soldierIdle);
        }
    }

    public override void OnCollisionEnter(SoldierStateManager soldier)
    {

    }
}
