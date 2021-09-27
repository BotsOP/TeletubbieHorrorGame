using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager
{
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyWanderState wanderState = new EnemyWanderState();
    public EnemyScareState attackState = new EnemyScareState();
    public GameObject enemyGameobject;
    public Transform[] patrolPoints;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform chaseTarget;

    private EnemyBaseState currentState;


    public EnemyStateManager(GameObject prefab, Transform[] _patrolPoints)
    {
        enemyGameobject = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        patrolPoints = _patrolPoints;

        EventSystem.Subscribe(EventType.START, Start);
        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Start()
    {
        agent = enemyGameobject.GetComponent<NavMeshAgent>();
        anim = enemyGameobject.GetComponent<Animator>();

        agent.updatePosition = true;
        agent.updateRotation = true;

        currentState = wanderState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState();
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
