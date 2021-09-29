using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScareState : EnemyBaseState
{
    EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;

        enemy.agent.isStopped = true;
        
        enemy.anim.SetInteger("moving", 6);
        
        EventSystem<Transform>.RaiseEvent(EventType.PLAYER_ATTACKED, enemy.enemyGameobject.transform);
    }
    public override void UpdateState()
    {
        
    }
}
