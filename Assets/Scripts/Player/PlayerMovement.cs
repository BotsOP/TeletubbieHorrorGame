using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement
{
    private GameObject playerBodyPrefab;

    private float walkSpeed;
    private float sneakSpeed;
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

    private float sneakLoudness = 2f;
    private float walkLoudness = 5f;

    private bool isSneaking = false;

    public float horizontal { get; private set; }
    public float vertical { get; private set; }

    public PlayerMovement(GameObject _playerBodyPrefab, Transform _groundCheck, float _walkSpeed, float _sneakSpeed, float _groundDistance, LayerMask _groundLayer, float _distanceToTravelPerStep, AudioClip[] _footStepSounds)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);

        groundCheck = _groundCheck;
        groundDistance = _groundDistance;
        groundLayer = _groundLayer;

        playerBodyPrefab = _playerBodyPrefab;
        walkSpeed = _walkSpeed;
        sneakSpeed = _sneakSpeed;
        rb = playerBodyPrefab.GetComponent<Rigidbody>();
        playerAudioSource = playerBodyPrefab.GetComponent<AudioSource>();
        footStepSounds = _footStepSounds;
        distanceToTravelPerStep = _distanceToTravelPerStep;

        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
    
    private void Update()
    {
        if (canMove)
        {
            CheckSneaking();

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            CheckFootsteps();

            float origMagnitude = movement.magnitude;
            movement.y = 0.0f;
            movement = movement.normalized * origMagnitude;

            movement = playerBodyPrefab.transform.right * horizontal + playerBodyPrefab.transform.forward * vertical;
        }
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
        if (!isSneaking)
        {
            rb.velocity = _direction * walkSpeed;
        }
        else
        {
            rb.velocity = _direction * sneakSpeed;
        }
    }

    private void PlayerAttacked(Transform _enemyTransform)
    {
        canMove = false;
    }

    private void CheckFootsteps()
    {
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f && isGrounded)
        {
            EventSystem<bool>.RaiseEvent(EventType.PLAYER_MOVEMENT, true);
            EventSystem<bool>.RaiseEvent(EventType.PLAYER_GROUNDED, true);

            distanceTravelled += Time.deltaTime;

            if (distanceTravelled >= distanceToTravelPerStep)
            {
                if (isSneaking)
                {
                    playerAudioSource.volume = Random.Range(0.1f, 0.15f);
                    EventSystem<Vector3, float>.RaiseEvent(EventType.DISTRACTION, playerBodyPrefab.transform.position, sneakLoudness);
                }
                else if (!isSneaking)
                {
                    playerAudioSource.volume = Random.Range(0.25f, 0.35f);
                    EventSystem<Vector3, float>.RaiseEvent(EventType.DISTRACTION, playerBodyPrefab.transform.position, walkLoudness);
                }

                playerAudioSource.clip = footStepSounds[Random.Range(0, footStepSounds.Length)];
                playerAudioSource.pitch = Random.Range(0.8f, 1.1f);
                playerAudioSource.Play();
                distanceTravelled = 0;
            }
        }
        else if (!isGrounded)
        {
            EventSystem<bool>.RaiseEvent(EventType.PLAYER_GROUNDED, true);
        }
        else if (Mathf.Abs(horizontal) < 0.1f || Mathf.Abs(vertical) < 0.1f)
        {
            EventSystem<bool>.RaiseEvent(EventType.PLAYER_MOVEMENT, true);
        }
    }

    private void CheckSneaking()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isSneaking)
        {
            isSneaking = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && isSneaking)
        {
            isSneaking = false;
        }
    }
}
