using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook
{
    private GameObject playerBodyPrefab;

    private Camera cam;

    private float mouseSensitivity;
    private float maxAngleY;
    private float angleX, angleY;
    private bool canLook = true;

    public PlayerLook(GameObject _playerBodyPrefab, float _sensitivity, float _maxAngleY)
    {
        Cursor.lockState = CursorLockMode.Locked;

        playerBodyPrefab = _playerBodyPrefab;
        cam = Camera.main;
        mouseSensitivity = _sensitivity;
        maxAngleY = _maxAngleY;
        angleY = 0;

        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }

    private void Update()
    {
        if (canLook)
        {
            Look();
        }
    }

    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        angleX += mouseX * Time.deltaTime * mouseSensitivity;
        angleY += mouseY * Time.deltaTime * mouseSensitivity;
        angleY = Mathf.Clamp(angleY, -maxAngleY, maxAngleY);

        playerBodyPrefab.transform.rotation = Quaternion.Euler(0, angleX, 0);
        cam.transform.localRotation = Quaternion.Euler(-angleY, 0, 0);
    }

    private void PlayerAttacked(Transform _enemyTransform)
    {
        canLook = false;
        cam.transform.LookAt(_enemyTransform);
    }
}
