using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyWanderState : EnemyBaseState
{
    private const float SWITCH_PROBABILITY = 0.1f;
    private const bool WAIT_AT_POINTS = true;
    private const float PATROL_WAITING_TIME = 5f;
    private const float DISTRACT_RANGE = 50f;

    EnemyStateManager enemy;

    private bool patrolForward;
    private bool walking;
    private bool waiting;
    private bool isDistracted;
    private float startTime;
    private float startTimeDistract;
    private int currentPatrolIndex = -1;
    private Vector3 distractPos;
    
    private int frameCount;

    public override void EnterState(EnemyStateManager _enemy)
    {
        enemy = _enemy;
        
        EventSystem<Vector3, float>.Subscribe(EventType.DISTRACTION, Distraction);
        EventSystem<GameObject>.Subscribe(EventType.FLASHLIGHT, CheckForFLashLight);
        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
        
        if(currentPatrolIndex == -1)
        {
            currentPatrolIndex = GetNearestPatrolPoint();
        }

        if (!isDistracted)
        {
            SetNewDestination();
        }

        enemy.agent.speed = 2;
        enemy.anim.SetInteger("battle", 0);
        enemy.anim.SetInteger("moving", 1);
    }

    public override void UpdateState()
    {
        CheckForDoor();
        
        if (enemy.fov.canSeeTarget)
        {
            enemy.SwitchState(enemy.chaseState);
        }
        
        // if(frameCount % 30 == 0)
        //     Debug.Log(enemy.agent.remainingDistance);
        
        if (enemy.agent.remainingDistance < 0.01f)
        {
            if (walking)
            {
                Debug.Log("set time");
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
            isDistracted = false;
            
            enemy.anim.SetInteger("moving", 1);

            ChangePatrolPoint();
            SetNewDestination();
        }

        frameCount++;
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

    private int GetNearestPatrolPoint()
    {
        float[] smallestDistance = new float[enemy.patrolPoints.Length];
        int closestPatrolPoint = 0;
        for (int i = 0; i < enemy.patrolPoints.Length; i++)
        {
            smallestDistance[i] = Vector3.Distance(enemy.patrolPoints[i].position, enemy.enemyGameobject.transform.position);
        }
        for (int i = 0; i < enemy.patrolPoints.Length; i++)
        {
            if (Vector3.Distance(enemy.patrolPoints[i].position, enemy.enemyGameobject.transform.position) <= smallestDistance.Min())
            {
                closestPatrolPoint = i;
            }
        }
        return closestPatrolPoint;
    }

    private void CheckForDoor()
    {
        RaycastHit hit;
        float raycastDistance = 2f;
        LayerMask layerMask = 1 << 9;
        
        if (Physics.Raycast(enemy.enemyGameobject.transform.position, enemy.enemyGameobject.transform.forward, out hit, raycastDistance, layerMask))
        {
            if (hit.transform.GetComponent<Animator>())
            {
                Animator anim = hit.transform.GetComponent<Animator>();
                if (AnimatorHasParameter("TriggerDoor", hit.transform.GetComponent<Animator>()))
                {
                    if (!anim.GetBool("TriggerDoor"))
                    {
                        anim.SetBool("TriggerDoor", true);
                        enemy.SwitchState(enemy.interactState);
                    }
                }
            }
        }
    }
    
    private bool AnimatorHasParameter(string _paramName, Animator _animator)
    {
        foreach (AnimatorControllerParameter param in _animator.parameters)
        {
            if (param.name == _paramName)
                return true;
        }
        return false;
    }

    private void CheckForFLashLight(GameObject _enemyHit)
    {
        if (_enemyHit == enemy.head)
        {
            enemy.SwitchState(enemy.stunnedState);
        }
    }

    private void SmoothRotation()
    {
        float rotationSpeed = 30;
        Vector3 newAngle;
        newAngle.x = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.x, enemy.patrolPoints[currentPatrolIndex].eulerAngles.x, (Time.time - startTime) / rotationSpeed);
        newAngle.y = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.y, enemy.patrolPoints[currentPatrolIndex].eulerAngles.y, (Time.time - startTime) / rotationSpeed);
        newAngle.z = Mathf.LerpAngle(enemy.enemyGameobject.transform.eulerAngles.z, enemy.patrolPoints[currentPatrolIndex].eulerAngles.z, (Time.time - startTime) / rotationSpeed);
        enemy.enemyGameobject.transform.eulerAngles = newAngle;
    }

    private void Distraction(Vector3 _distractPos, float _hearingRange)
    {
        Debug.Log("distraction possible");
        distractPos = _distractPos;
        if (Vector3.Distance(distractPos, enemy.enemyGameobject.transform.position) < _hearingRange)
        {
            Debug.Log("distracted");
            startTimeDistract = Time.time;
            enemy.agent.SetDestination(distractPos);
            enemy.anim.SetInteger("moving", 1);
            walking = true;
            waiting = false;
            isDistracted = true;
        }
    }

    private void PlayerAttacked(Transform _transform)
    {
        EventSystem<Vector3, float>.Unsubscribe(EventType.DISTRACTION, Distraction);
        EventSystem<GameObject>.Unsubscribe(EventType.FLASHLIGHT, CheckForFLashLight);
        EventSystem<Transform>.Unsubscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
}


