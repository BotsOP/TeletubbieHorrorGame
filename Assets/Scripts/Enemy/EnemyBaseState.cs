using UnityEngine;

public abstract class EnemyBaseState
{
    public abstract void EnterState(EnemyStateManager _enemy);
    public abstract void UpdateState();
}