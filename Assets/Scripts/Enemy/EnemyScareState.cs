using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScareState : EnemyBaseState
{
    EnemyStateManager enemy;
    public override void EnterState(EnemyStateManager _enemy)
    {
        enemy = _enemy;

        enemy.agent.isStopped = true;
        
        Debug.Log("attacking");
        
        enemy.anim.SetInteger("moving", 6);
        
        EventSystem<Transform>.RaiseEvent(EventType.PLAYER_ATTACKED, enemy.head.transform);
    }
    public override void UpdateState()
    {
        Vector3 lookAt = new Vector3(enemy.fov.target.position.x, enemy.enemyGameobject.transform.position.y, enemy.fov.target.position.z);
        enemy.enemyGameobject.transform.LookAt(lookAt);
        EventSystem<Transform>.RaiseEvent(EventType.PLAYER_ATTACKED, enemy.head.transform);
    }
}
