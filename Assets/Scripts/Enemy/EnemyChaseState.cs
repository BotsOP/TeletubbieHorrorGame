using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    private const float ATTENTION_TIME = 15f;
    private const float RANGE_TILL_ATTACK = 4f;
    
    EnemyStateManager enemy;
    private float lastSeen;
    private float startTime;
    public override void EnterState(EnemyStateManager _enemy)
    {
        enemy = _enemy;
        
        EventSystem<GameObject>.Subscribe(EventType.FLASHLIGHT, CheckForFLashLight);
        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);

        lastSeen = Time.time;

        enemy.agent.speed = 6;
        enemy.anim.SetInteger("battle", 1);
        enemy.anim.SetInteger("moving", 2);
    }
    public override void UpdateState()
    {
        if (enemy.fov.canSeeTarget)
        {
            UpdateTargetPos();
        }

        if (enemy.agent.remainingDistance < 0.2f)
        {
            LookAround();
        }
        else
        {
            KeepMoving();
        }
        
        if (Vector3.Distance(enemy.fov.target.position, enemy.enemyGameobject.transform.position) < RANGE_TILL_ATTACK && enemy.fov.canSeeTarget)
        {
            enemy.SwitchState(enemy.scareState);
        }
        
        if (Time.time - lastSeen > ATTENTION_TIME && !enemy.fov.canSeeTarget)
        {
            enemy.SwitchState(enemy.wanderState);
        }
    }

    private void UpdateTargetPos()
    {
        lastSeen = Time.time;
        enemy.agent.SetDestination(enemy.fov.target.position);
    }

    private void LookAround()
    {
        enemy.anim.SetInteger("moving", 0);
        SmoothRotation();
    }

    private void KeepMoving()
    {
        enemy.anim.SetInteger("moving", 2);
        startTime = Time.time;
    }
    
    private void CheckForFLashLight(GameObject enemyHit)
    {
        if (enemyHit == enemy.head)
        {
            enemy.SwitchState(enemy.stunnedState);
        }
    }
    
    private void SmoothRotation()
    {
        float rotationSpeed = 800;
        Vector3 newAngle;
        newAngle.x = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.x, enemy.enemyGameobject.transform.eulerAngles.x - 180, (Time.time - lastSeen) / rotationSpeed);
        newAngle.y = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.y, enemy.enemyGameobject.transform.eulerAngles.y - 180, 0.001f );
        newAngle.z = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.z, enemy.enemyGameobject.transform.eulerAngles.z - 180, (Time.time - startTime) / rotationSpeed);
        enemy.enemyGameobject.transform.eulerAngles = newAngle;
    }

    private void PlayerAttacked(Transform transform)
    {
        EventSystem<GameObject>.Unsubscribe(EventType.FLASHLIGHT, CheckForFLashLight);
        EventSystem<Transform>.Unsubscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
}
