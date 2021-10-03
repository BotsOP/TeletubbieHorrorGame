using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    [Header("Enemy References")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private GameObject enemyBody;
    [SerializeField] private Transform enemySpawnPos;
    [SerializeField] private AudioClip enemyRoarSound;


    [Header("Player References")]
    private PlayerController playerController;
    private PlayerLook playerLook;
    private PlayerMovement playerMovement;
    private PlayerHeadBob playerHeadBob;

    [SerializeField] private GameObject playerBodyPrefab;
    [SerializeField] private float playerSensitivity, maxAnglyY, playerWalkSpeed, playerSneakSpeed, groundDistance = 0.4f, distanceToTravelPerStep = 1f;
    [SerializeField] private float bobbingSpeed = 16f, bobbingAmount = 0.05f;
    [Space(15)]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask playerRaycastLayer;
    [SerializeField] private LayerMask playerPickUpLayer;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float maxPlayerRayDistance;
    [SerializeField] private AudioClip[] footStepSounds;
    [Space(15)]
    [SerializeField] private GameObject objectHolder;
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private TextMeshProUGUI textForInteraction;
    [SerializeField] private GameObject[] objectToOpen;
    [SerializeField] private GameObject[] keyToUse;
    [SerializeField] private AudioClip openDoorSound;
    [Space(15)]
    [SerializeField] private GameObject flashLight;
    [SerializeField] private float flashLightCoolDown;
    [SerializeField] private float flashLightMaxUsage;
    [SerializeField] private float flashLightDistance = 3f;

    [SerializeField] private GameObject gameWonPanel;
    [SerializeField] private GameObject gameLostPanel;
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject howToUsePanel;
    [Space(15)]
    [SerializeField] private GameObject[] objectsToCollect;
    [SerializeField] private TextMeshProUGUI[] objectsToCollectTexts;
    [Space(15)]
    [SerializeField] private AudioSource screamSource;
    [SerializeField] private AudioClip screamClip;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject flashingImage;
    [SerializeField] private GameObject[] jumpscareTriggers;
    private List<Jumpscare> jumpscareScripts = new List<Jumpscare>();

    Dictionary<GameObject, GameObject> objectsToOpenDict = new Dictionary<GameObject, GameObject>();
    private bool gameOver = false;

    private void Start()
    {
            EnemyStateManager enemy = new EnemyStateManager(enemyBody, patrolPoints, enemySpawnPos, enemyRoarSound);

            for (int i = 0; i < objectToOpen.Length; i++)
            {
                objectsToOpenDict.Add(objectToOpen[i], keyToUse[i]);
            }

            for (int i = 0; i < jumpscareTriggers.Length; i++)
            {
                Jumpscare jumpscareScript = new Jumpscare(playerBodyPrefab, jumpscareTriggers[i].transform, jumpscareCam, flashingImage, screamSource, screamClip);
                jumpscareScripts.Add(jumpscareScript);
            }

            gameOver = false;
            gameWonPanel.SetActive(false);
            gameLostPanel.SetActive(false);
            howToUsePanel.SetActive(true);

            playerController = new PlayerController(playerBodyPrefab, objectHolder, flashLight, flashLightCoolDown, flashLightMaxUsage, flashLightDistance, playerRaycastLayer, playerPickUpLayer, enemyLayer, maxPlayerRayDistance, objectsToOpenDict, textForInteraction, throwForce, openDoorSound, objectsToCollect, objectsToCollectTexts);
            playerLook = new PlayerLook(playerBodyPrefab, playerSensitivity, maxAnglyY);
            playerMovement = new PlayerMovement(playerBodyPrefab, groundCheck, playerWalkSpeed, playerSneakSpeed, groundDistance, groundLayer, distanceToTravelPerStep, footStepSounds);
            playerHeadBob = new PlayerHeadBob(playerBodyPrefab, bobbingSpeed, bobbingAmount);

            EventSystem.RaiseEvent(EventType.START);
            EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
            EventSystem.Subscribe(EventType.GAME_WON, GameWon);
    }

    private void Update()
    {
        EventSystem.RaiseEvent(EventType.UPDATE);

        if (Input.GetKeyDown(KeyCode.H))
        {
            howToUsePanel.SetActive(!howToUsePanel.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.R) && gameOver)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void FixedUpdate()
    {
        EventSystem.RaiseEvent(EventType.FIXED_UPDATE);
    }

    void PlayerAttacked(Transform _transform)
    {
        gameOver = true;
        Destroy(playerBodyPrefab.GetComponent<Rigidbody>());
        gameLostPanel.SetActive(true);
        gameLostPanel.GetComponent<Animator>().SetTrigger("GameWon");
        EventSystem<Transform>.Unsubscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
        EventSystem.Unsubscribe(EventType.GAME_WON, GameWon);
    }

    void GameWon()
    {
        crossHair.SetActive(false);
        gameWonPanel.SetActive(true);
        gameWonPanel.GetComponent<Animator>().SetTrigger("GameWon");
        Invoke("CloseGame", 7.5f);
    }

    void CloseGame()
    {
        Application.Quit();
    }
}
