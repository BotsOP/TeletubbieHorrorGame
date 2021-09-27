using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public System.Action UPDATE;

    private void Update()
    {
        UPDATE?.Invoke();
    }
}
