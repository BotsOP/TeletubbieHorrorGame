using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
        SetDestination();
        enemy.anim.SetInteger("battle", 1);
        enemy.anim.SetInteger("moving", 1);
    }
    public override void UpdateState()
    {
        if(enemy.agent.remainingDistance < 1f)
        {
            SetDestination();
        }
    }

    private void SetDestination()
    {
        Vector3 randomPos = new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        enemy.agent.SetDestination(randomPos);
    }
}
