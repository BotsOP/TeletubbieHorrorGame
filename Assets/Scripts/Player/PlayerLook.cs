using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook
{
    private GameObject playerBodyPrefab;

    private Camera cam;

    private float mouseSensitivityX, mouseSensitivityY;
    private float maxAngleY, minAngleY;
    private float angleX, angleY;
    private bool canLook = true;

    public PlayerLook(GameObject _playerBodyPrefab, Camera _cam, float _sensitivity, float _minAngleY, float _maxAngleY)
    {
        Cursor.lockState = CursorLockMode.Locked;

        EventSystem.Subscribe(EventType.UPDATE, Update);

        playerBodyPrefab = _playerBodyPrefab;
        cam = _cam;
        mouseSensitivityX = _sensitivity;
        mouseSensitivityY = _sensitivity;
        minAngleY = _minAngleY;
        maxAngleY = _maxAngleY;
        angleY = 0;

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

        angleX += mouseX * Time.deltaTime * mouseSensitivityX;
        angleY += mouseY * Time.deltaTime * mouseSensitivityY;
        angleY = Mathf.Clamp(angleY, -minAngleY, maxAngleY);

        playerBodyPrefab.transform.rotation = Quaternion.Euler(0, angleX, 0);
        cam.transform.localRotation = Quaternion.Euler(-angleY, 0, 0);
    }

    private void PlayerAttacked(Transform _enemyTransform)
    {
        canLook = false;
        cam.transform.LookAt(_enemyTransform);
    }
}
