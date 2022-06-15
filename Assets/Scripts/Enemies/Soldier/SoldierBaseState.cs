using UnityEngine;

public abstract class SoldierBaseState
{
  public abstract void EnterState(SoldierStateManager soldier);
  public abstract void UpdateState(SoldierStateManager soldier);
  public abstract void OnCollisionEnter(SoldierStateManager soldier);
}