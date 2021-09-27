using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy
{
    public GameObject enemyBodyBlueprint;

    private GameObject enemyBody;
    private NavMeshAgent agent;
    private float startTime;

    public void Start()
    {
        enemyBody = GameObject.Instantiate(enemyBodyBlueprint);
        agent = enemyBody.GetComponent<NavMeshAgent>();
        SetRandomDestination();

    }

    public void Update()
    {
        if (agent.remainingDistance < 0.2f)
        {
            SetRandomDestination();
        }
    }

    private void SetRandomDestination()
    {
        Vector3 randomPos = new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
        agent.SetDestination(randomPos);
    }
}
