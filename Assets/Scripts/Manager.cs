using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public Transform[] patrolPoints;
    public GameObject enemyBody;

    private void Start()
    {
        EnemyStateManager enemy = new EnemyStateManager(enemyBody, patrolPoints);
        EventSystem.RaiseEvent(EventType.START);
    }

    private void Update()
    {
        EventSystem.RaiseEvent(EventType.UPDATE);
    }

    private void FixedUpdate()
    {
        EventSystem.RaiseEvent(EventType.FIXED_UPDATE);
    }
}
