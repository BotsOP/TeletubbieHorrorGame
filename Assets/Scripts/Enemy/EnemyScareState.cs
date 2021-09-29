using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScareState : EnemyBaseState
{
    EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
        Debug.Log("attack");
        enemy.agent.isStopped = true;
        enemy.anim.SetInteger("moving", 6);
    }
    public override void UpdateState()
    {

    }
}
