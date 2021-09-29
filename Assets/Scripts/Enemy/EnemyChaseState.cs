using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private const float ATTENTION_TIME = 10f;
    
    EnemyStateManager enemy;
    private float lastSeen;
    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;

        lastSeen = Time.time;

        enemy.agent.speed = 6;
        enemy.anim.SetInteger("battle", 1);
        enemy.anim.SetInteger("moving", 2);
    }
    public override void UpdateState()
    {
        if (enemy.fov.canSeeTarget)
        {
            lastSeen = Time.time;
        }
        
        if (Time.time - lastSeen > ATTENTION_TIME && !enemy.fov.canSeeTarget)
        {
            enemy.SwitchState(enemy.wanderState);
        }
        enemy.agent.SetDestination(enemy.fov.target.position);
    }
}
