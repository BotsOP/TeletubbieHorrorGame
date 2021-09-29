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
    private float distanceTravelled;
    private float distanceToTravelPerStep;

    private AudioSource playerAudioSource;
    private AudioClip[] footStepSounds;

    public float horizontal { get; private set; }
    public float vertical { get; private set; }

    public PlayerMovement(GameObject _playerBodyPrefab, Transform _groundCheck, float _speed, float _groundDistance, LayerMask _groundLayer, float _distanceToTravelPerStep, AudioClip[] _footStepSounds)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);

        groundCheck = _groundCheck;
        groundDistance = _groundDistance;
        groundLayer = _groundLayer;

        playerBodyPrefab = _playerBodyPrefab;
        speed = _speed;
        rb = playerBodyPrefab.GetComponent<Rigidbody>();
        playerAudioSource = playerBodyPrefab.GetComponent<AudioSource>();
        footStepSounds = _footStepSounds;
        distanceToTravelPerStep = _distanceToTravelPerStep;

        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
    
    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        CheckFootsteps();

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

    private void PlayerAttacked(Transform _enemyTransform)
    {
        Debug.Log("attacked!!!!!!!!");
        canMove = false;
    }

    private void CheckFootsteps()
    {
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f && isGrounded)
        {
            distanceTravelled += Time.deltaTime;

            if (distanceTravelled >= distanceToTravelPerStep)
            {
                // Play Step Sound
                playerAudioSource.clip = footStepSounds[Random.Range(0, footStepSounds.Length)];
                playerAudioSource.volume = Random.Range(0.25f, 0.35f);
                playerAudioSource.pitch = Random.Range(0.8f, 1.1f);
                playerAudioSource.Play();
                distanceTravelled = 0;
            }
        }
    }
}
