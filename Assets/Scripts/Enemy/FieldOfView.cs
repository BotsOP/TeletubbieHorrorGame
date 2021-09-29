using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView
{
    private const float RADIUS = 100f;
    private const float ANGLE = 180;
    
    public bool canSeeTarget;
    public Transform target;

    private GameObject gameObject;
    private int frameCount;
    private LayerMask targetMask;
    private LayerMask obstructionMask;

    public FieldOfView(GameObject _gameObject, LayerMask _targetMask, LayerMask _obstructionMask)
    {
        gameObject = _gameObject;

        targetMask = _targetMask;
        obstructionMask = _obstructionMask;

        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);
    }

    private void FixedUpdate()
    {
        if(frameCount % 20 == 0)
        {
            Debug.Log("Yo");
            FieldofViewCheck();
        }
        frameCount++;
    }

    private void FieldofViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(gameObject.transform.position, RADIUS, targetMask);

        //If there are multiple targets change this into a foreach loop
        if (rangeChecks.Length != 0)
        {
            target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - gameObject.transform.position).normalized;

            if (Vector3.Angle(gameObject.transform.forward, directionToTarget) < ANGLE / 2)
            {
                float distanceToTarget = Vector3.Distance(gameObject.transform.position, target.position);

                if (!Physics.Raycast(gameObject.transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeeTarget = true;
                }
                else
                {
                    canSeeTarget = false;
                }
            }
            else
            {
                canSeeTarget = false;
            }
        }
        else if (canSeeTarget)
        {
            canSeeTarget = false;
        }
    }
}
