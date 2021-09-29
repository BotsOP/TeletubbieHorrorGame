using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    private const float SWITCH_PROBABILITY = 0.1f;
    private const bool WAIT_AT_POINTS = true;
    private const float PATROL_WAITING_TIME = 5f;

    EnemyStateManager enemy;

    private bool patrolForward;
    private bool walking;
    private bool waiting;
    private bool isDistracted;
    private float startTime;
    private int currentPatrolIndex;
    private Vector3 distractPos;

    public override void EnterState(EnemyStateManager enemy)
    {
        this.enemy = enemy;
        
        EventSystem<Vector3>.Subscribe(EventType.DISTRACTION, Distraction);
        
        SetNewDestination();

        enemy.agent.speed = 2;
        enemy.anim.SetInteger("battle", 0);
        enemy.anim.SetInteger("moving", 1);
    }

    public override void UpdateState()
    {
        if (enemy.fov.canSeeTarget)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        
        if (isDistracted && Vector3.Distance(distractPos, enemy.enemyGameobject.transform.position) < 80)
        {
            enemy.agent.SetDestination(distractPos);
            isDistracted = false;
        }
        
        if (enemy.agent.remainingDistance < 0.01f)
        {
            if (walking)
            {
                walking = false;
                

                if (WAIT_AT_POINTS)
                {
                    enemy.anim.SetInteger("moving", 0);
                    waiting = true;
                    startTime = Time.time;
                }
                else
                {
                    ChangePatrolPoint();
                    SetNewDestination();
                }
            }

            SmoothRotation();
        }

        if (waiting && Time.time - startTime > PATROL_WAITING_TIME)
        {
            waiting = false;

            ChangePatrolPoint();
            SetNewDestination();
        }
    }

    private void SetNewDestination()
    {
        Vector3 walkTo = enemy.patrolPoints[currentPatrolIndex].position;
        enemy.agent.SetDestination(walkTo);
        walking = true;
        enemy.anim.SetInteger("moving", 1);
    }

    private void ChangePatrolPoint()
    {
        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber <= SWITCH_PROBABILITY)
        {
            patrolForward = !patrolForward;
        }

        if (patrolForward)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % enemy.patrolPoints.Length;
        }
        else
        {
            currentPatrolIndex--;
            if (currentPatrolIndex < 0)
            {
                currentPatrolIndex = enemy.patrolPoints.Length - 1;
            }
        }
    }

    private void SmoothRotation()
    {
        float rotationSpeed = 80;
        Vector3 newAngle;
        newAngle.x = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.x, enemy.patrolPoints[currentPatrolIndex].eulerAngles.x, (Time.time - startTime) / rotationSpeed);
        newAngle.y = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.y, enemy.patrolPoints[currentPatrolIndex].eulerAngles.y, (Time.time - startTime) / rotationSpeed);
        newAngle.z = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.z, enemy.patrolPoints[currentPatrolIndex].eulerAngles.z, (Time.time - startTime) / rotationSpeed);
        enemy.enemyGameobject.transform.eulerAngles = newAngle;
    }

    private void Distraction(Vector3 _distractPos)
    {
        distractPos = _distractPos;
        isDistracted = true;
    }
}


