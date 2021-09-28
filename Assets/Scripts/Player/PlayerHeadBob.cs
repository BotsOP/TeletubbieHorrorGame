using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBob
{
    private GameObject playerBodyPrefab;

    private float bobbingSpeed = 16f;
    private float bobbingAmount = 0.05f;
    private PlayerMovement controller;
    private bool isGrounded = true;

    float defaultPosY = 0;
    float timer = 0;

    public PlayerHeadBob(GameObject _playerBodyPrefab, PlayerMovement _controller, float _bobbingSpeed, float _bobbingAmount)
    {
        playerBodyPrefab = _playerBodyPrefab;
        controller = _controller;
        bobbingSpeed = _bobbingSpeed;
        bobbingAmount = _bobbingAmount;

        defaultPosY = playerBodyPrefab.transform.localPosition.y;

        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    // Update is called once per frame
    private void Update()
    {
        isGrounded = controller.isGrounded;

        if (Mathf.Abs(controller.horizontal) > 0.1f || Mathf.Abs(controller.vertical) > 0.1f && isGrounded)
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
}
