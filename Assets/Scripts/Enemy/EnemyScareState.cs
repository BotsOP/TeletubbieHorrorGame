using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScareState : EnemyBaseState
{
    EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
    }
    public override void UpdateState()
    {

    }
}
