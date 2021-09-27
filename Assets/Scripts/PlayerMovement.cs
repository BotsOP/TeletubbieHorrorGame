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
        playerBodyPrefab = _playerBodyPrefab;
        speed = _speed;
        rb = playerBodyPrefab.GetComponent<Rigidbody>();
    }
    
    public void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
    }

    public void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    public void MoveCharacter(Vector3 _direction)
    {
        rb.velocity = _direction * speed;
    }
}
