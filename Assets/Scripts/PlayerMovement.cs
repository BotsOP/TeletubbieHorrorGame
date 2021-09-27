using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private GameObject playerBodyPrefab;

    private float speed;
    private Vector3 movement;
    private Rigidbody rb;

    public PlayerMovement(GameObject _playerBodyPrefab, float _speed)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);

        playerBodyPrefab = _playerBodyPrefab;
        speed = _speed;
        rb = playerBodyPrefab.GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float origMagnitude = movement.magnitude;
        movement.y = 0.0f;
        movement = movement.normalized * origMagnitude;

        movement = playerBodyPrefab.transform.right * horizontal + playerBodyPrefab.transform.forward * vertical;
    }

    private void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    private void MoveCharacter(Vector3 _direction)
    {
        rb.velocity = _direction * speed;
    }
}
