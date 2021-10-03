using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBob
{
    private GameObject playerBodyPrefab;

    private float bobbingSpeed = 16f;
    private float bobbingAmount = 0.05f;
    private bool isPlayerMoving = false;
    private bool isPlayerGrounded = true;

    float defaultPosY = 0;
    float timer = 0;

    public PlayerHeadBob(GameObject _playerBodyPrefab, float _bobbingSpeed, float _bobbingAmount)
    {
        playerBodyPrefab = _playerBodyPrefab;
        bobbingSpeed = _bobbingSpeed;
        bobbingAmount = _bobbingAmount;

        defaultPosY = playerBodyPrefab.transform.localPosition.y;

        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem<bool>.Subscribe(EventType.PLAYER_MOVEMENT, CheckPlayerMovement);
        EventSystem<bool>.Subscribe(EventType.PLAYER_GROUNDED, CheckPlayerGrounded);
    }

    // Update is called once per frame
    private void Update()
    {
        if (isPlayerMoving && isPlayerGrounded)
        {
            timer += Time.deltaTime * bobbingSpeed;
            playerBodyPrefab.transform.localPosition = new Vector3(playerBodyPrefab.transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, playerBodyPrefab.transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            playerBodyPrefab.transform.localPosition = new Vector3(playerBodyPrefab.transform.localPosition.x, Mathf.Lerp(playerBodyPrefab.transform.localPosition.y, defaultPosY, Time.deltaTime * bobbingSpeed), playerBodyPrefab.transform.localPosition.z);
        }
    }

    private void CheckPlayerMovement(bool _isMoving)
    {
        isPlayerMoving = _isMoving;
    }

    private void CheckPlayerGrounded(bool _isGrounded)
    {
        isPlayerGrounded = _isGrounded;
    }
}
