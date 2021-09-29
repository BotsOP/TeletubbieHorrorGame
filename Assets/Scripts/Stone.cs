using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Distraction());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Distraction()
    {
        yield return new WaitForSeconds(1f);
        EventSystem<Vector3>.RaiseEvent(EventType.DISTRACTION, transform.position);
    }
}
