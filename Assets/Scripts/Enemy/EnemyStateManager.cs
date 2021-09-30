using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager
{
    public EnemyChaseState chaseState = new EnemyChaseState();
    public EnemyWanderState wanderState = new EnemyWanderState();
    public EnemyScareState scareState = new EnemyScareState();
    public EnemyInteractState interactState = new EnemyInteractState();
    public EnemyStunenedState stunnedState = new EnemyStunenedState();
    public GameObject enemyGameobject;
    public GameObject head;
    public Transform[] patrolPoints;
    public FieldOfView fov;
    public AudioSource enemyAudioSource;
    public AudioClip enemyRoarSound;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform chaseTarget;

    private EnemyBaseState currentState;


    public EnemyStateManager(GameObject prefab, Transform[] _patrolPoints, Transform spawnPos, AudioClip _enemyRoarSound)
    {
        enemyGameobject = Object.Instantiate(prefab, spawnPos.position, spawnPos.rotation);
        enemyAudioSource = enemyGameobject.GetComponent<AudioSource>();
        enemyRoarSound = _enemyRoarSound;
        
        patrolPoints = _patrolPoints;

        EventSystem.Subscribe(EventType.START, Start);
        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Start()
    {
        head = enemyGameobject.transform.Find("Creature_armature1/Base_bone/Hips_ctrl/spine_1/spine_2/spine_3/spine_5/Neck/Head/Jaw/LookTarget").gameObject;
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
