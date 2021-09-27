using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook
{
    private GameObject playerBodyPrefab;

    private GameObject cam;

    private float mouseSensitivityX, mouseSensitivityY;
    private float maxAngleY, minAngleY;
    private float angleX, angleY;

    public PlayerLook(GameObject _playerBodyPrefab, GameObject _cam, float _sensitivity, float _minAngleY, float _maxAngleY)
    {
        playerBodyPrefab = _playerBodyPrefab;
        cam = _cam;
        mouseSensitivityX = _sensitivity;
        mouseSensitivityY = _sensitivity;
        minAngleY = _minAngleY;
        maxAngleY = _maxAngleY;
    }

    void Update()
    {
        Look();
    }

    public void Look()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        angleX += mouseX * Time.deltaTime * mouseSensitivityX;
        angleY += mouseY * Time.deltaTime * mouseSensitivityY;
        angleY = Mathf.Clamp(angleY, -minAngleY, maxAngleY);

        playerBodyPrefab.transform.rotation = Quaternion.Euler(0, angleX, 0);
        cam.transform.localRotation = Quaternion.Euler(-angleY, 0, 0);
    }
}
