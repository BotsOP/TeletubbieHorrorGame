using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    [SerializeField] private GameObject playerBodyPrefab;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerLook playerMovement;
    [SerializeField] private PlayerLook playerController;
    [SerializeField] private float playerSensitivity, minAngleY, maxAnglyY, playerSpeed;

    private void Start()
    {
        PlayerLook playerLookScript = new PlayerLook(playerBodyPrefab, Camera.main, playerSensitivity, minAngleY, maxAnglyY);
        PlayerMovement playerMovementScript = new PlayerMovement(playerBodyPrefab, playerSpeed);

        EventSystem.RaiseEvent(EventType.START);
    }

    private void Update()
    {
        EventSystem.RaiseEvent(EventType.UPDATE);
    }

    private void FixedUpdate()
    {
        EventSystem.RaiseEvent(EventType.FIXED_UPDATE);
    }
}
