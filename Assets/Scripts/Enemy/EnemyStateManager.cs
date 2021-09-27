using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager
{
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyWanderState wanderState = new EnemyWanderState();
    public EnemyScareState attackState = new EnemyScareState();

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform chaseTarget;

    public GameObject enemyBodyBlueprint;

    private GameObject enemyGameobject;

    private EnemyBaseState currentState;

    public EnemyStateManager(GameObject prefab)
    {
        enemyGameobject = GameObject.Instantiate(prefab);
        EventSystem.Subscribe(EventType.START, Start);
        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    public void Start()
    {
        Debug.Log("Start");
        agent = enemyGameobject.GetComponent<NavMeshAgent>();
        anim = enemyGameobject.GetComponent<Animator>();

        agent.updatePosition = true;
        agent.updateRotation = true;

        currentState = wanderState;
        currentState.EnterState(this);
    }

    public void Update()
    {
        Debug.Log("Update");
        currentState.UpdateState();
    }

    public void SwitchState(EnemyBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
