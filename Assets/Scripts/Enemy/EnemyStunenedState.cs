using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStunenedState : EnemyBaseState
{
    private const float STUNNED_TIME = 1.5f;
    
    EnemyStateManager enemy;
    private float startTime;
    public override void EnterState(EnemyStateManager _enemy)
    {
        enemy = _enemy;
        
        Debug.Log("stunned");
        startTime = Time.time;
        enemy.agent.isStopped = true;
        enemy.anim.SetInteger("moving", 5);
    }

    public override void UpdateState()
    {
        if (Time.time - startTime > STUNNED_TIME)
        {
            Debug.Log("going back to wander");
            enemy.agent.isStopped = false;
            enemy.SwitchState(enemy.wanderState);
        }
    }
}
