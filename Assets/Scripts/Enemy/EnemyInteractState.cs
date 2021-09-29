using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteractState : EnemyBaseState
{
    private const float WAIT_TIME = 1f;
    EnemyStateManager enemy;
    private float startTime;
    public override void EnterState(EnemyStateManager _enemy)
    {
        enemy = _enemy;
        
        Debug.Log("interacting");
        startTime = Time.time;
        enemy.agent.isStopped = true;
        enemy.anim.SetInteger("moving", 4);
    }

    public override void UpdateState()
    {
        if (Time.time - startTime > WAIT_TIME)
        {
            Debug.Log("going back to wander");
            enemy.agent.isStopped = false;
            enemy.SwitchState(enemy.wanderState);
        }
    }
}
