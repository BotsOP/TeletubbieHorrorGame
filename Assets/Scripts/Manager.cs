using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    [Header("Enemy References")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private GameObject enemyBody;
    [SerializeField] private Transform enemySpawnPos;
    
    
    [Header("Player References")]
    [SerializeField] private GameObject playerBodyPrefab;
    [SerializeField] private PlayerLook playerLook;
    [SerializeField] private PlayerLook playerMovement;
    [SerializeField] private PlayerHeadBob playerHeadBob;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float playerSensitivity, minAngleY, maxAnglyY, playerSpeed, groundDistance = 0.4f;
    [SerializeField] private float bobbingSpeed = 16f, bobbingAmount = 0.05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerRaycastLayer;
    [SerializeField] private LayerMask playerPickUpLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float maxPlayerRayDistance;

    [SerializeField] private GameObject objectHolder;
    [SerializeField] private GameObject[] objectToOpen;
    [SerializeField] private GameObject[] keyToUse;
    [SerializeField] private TextMeshProUGUI textForInteraction;
    [SerializeField] private GameObject flashLight;
    [SerializeField] private float flashLightCoolDown;
    [SerializeField] private float flashLightMaxUsage;

    Dictionary<GameObject, GameObject> objectsToOpenDict = new Dictionary<GameObject, GameObject>();

    private void Start()
    {
        EnemyStateManager enemy = new EnemyStateManager(enemyBody, patrolPoints, enemySpawnPos);
        
        for (int i = 0; i < objectToOpen.Length; i++)
        {
            objectsToOpenDict.Add(objectToOpen[i], keyToUse[i]);
        }

        PlayerController playerController = new PlayerController(objectHolder, flashLight, flashLightCoolDown, flashLightMaxUsage, Camera.main, playerRaycastLayer, playerPickUpLayer, enemyLayer, maxPlayerRayDistance, objectsToOpenDict, textForInteraction);
        PlayerLook playerLookScript = new PlayerLook(playerBodyPrefab, Camera.main, playerSensitivity, minAngleY, maxAnglyY);
        PlayerMovement playerMovementScript = new PlayerMovement(playerBodyPrefab, groundCheck, playerSpeed, groundDistance, groundLayer);
        PlayerHeadBob playerHeadBob = new PlayerHeadBob(playerBodyPrefab, playerMovementScript, bobbingSpeed, bobbingAmount);
        
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
