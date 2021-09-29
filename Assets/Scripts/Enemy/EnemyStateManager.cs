using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager
{
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyWanderState wanderState = new EnemyWanderState();
    public EnemyScareState scareState = new EnemyScareState();
    public GameObject enemyGameobject;
    public Transform[] patrolPoints;
    public FieldOfView fov;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform chaseTarget;

    private EnemyBaseState currentState;


    public EnemyStateManager(GameObject prefab, Transform[] _patrolPoints, Transform spawnPos)
    {
        enemyGameobject = GameObject.Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        patrolPoints = _patrolPoints;

        EventSystem.Subscribe(EventType.START, Start);
        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Start()
    {
        LayerMask targetMask = 1 << 6;
        LayerMask obstructionMask = 1 << 7;
        fov = new FieldOfView(enemyGameobject, targetMask, obstructionMask);
        
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
