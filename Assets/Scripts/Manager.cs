using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform[] patrolPoints;
    public GameObject enemyBody;

    void Start()
    {
        EnemyStateManager enemy = new EnemyStateManager(enemyBody, patrolPoints);
        EventSystem.RaiseEvent(EventType.START);
    }

    void Update()
    {
        EventSystem.RaiseEvent(EventType.UPDATE);
    }
}
