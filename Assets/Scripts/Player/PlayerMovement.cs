using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private GameObject playerBodyPrefab;

    private float speed;
    private Vector3 movement;
    private Rigidbody rb;

    public bool isGrounded;
    private Transform groundCheck;

    private LayerMask groundLayer;
    private float groundDistance = 0.4f;
    private bool canMove = true;

    public float horizontal { get; private set; }
    public float vertical { get; private set; }

    public PlayerMovement(GameObject _playerBodyPrefab, Transform _groundCheck, float _speed, float _groundDistance, LayerMask _groundLayer)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);

        groundCheck = _groundCheck;
        groundDistance = _groundDistance;
        groundLayer = _groundLayer;

        playerBodyPrefab = _playerBodyPrefab;
        speed = _speed;
        rb = playerBodyPrefab.GetComponent<Rigidbody>();

        EventSystem.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        float origMagnitude = movement.magnitude;
        movement.y = 0.0f;
        movement = movement.normalized * origMagnitude;

        movement = playerBodyPrefab.transform.right * horizontal + playerBodyPrefab.transform.forward * vertical;
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            MoveCharacter(movement);
        }
    }

    private void MoveCharacter(Vector3 _direction)
    {
        rb.velocity = _direction * speed;
    }

    private void PlayerAttacked()
    {
        canMove = false;
    }
}
