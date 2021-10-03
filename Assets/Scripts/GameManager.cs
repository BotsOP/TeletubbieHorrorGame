using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Player References")]
    private PlayerController playerController;
    private PlayerLook playerLook;
    private PlayerMovement playerMovement;
    private PlayerHeadBob playerHeadBob;

    [SerializeField] private GameObject playerBodyPrefab;
    [SerializeField] private float playerSensitivity, minAngleY, maxAnglyY, playerWalkSpeed, playerSneakSpeed, groundDistance = 0.4f, distanceToTravelPerStep = 1f;
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
    [SerializeField] private GameObject crossHair;
    [SerializeField] private GameObject howToUsePanel;

    private GameObject[] objectsToCollect;
    private TextMeshProUGUI[] objectsToCollectTexts;
    private bool gameOver = false;

    Dictionary<GameObject, GameObject> objectsToOpenDict = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        for (int i = 0; i < objectToOpen.Length; i++)
        {
            objectsToOpenDict.Add(objectToOpen[i], keyToUse[i]);
        }

        gameOver = false;
        howToUsePanel.SetActive(true);
        gameWonPanel.SetActive(false);
        playerController = new PlayerController(playerBodyPrefab, objectHolder, flashLight, flashLightCoolDown, flashLightMaxUsage, flashLightDistance, playerRaycastLayer, playerPickUpLayer, enemyLayer, maxPlayerRayDistance, objectsToOpenDict, textForInteraction, throwForce, openDoorSound, objectsToCollect, objectsToCollectTexts);
        playerLook = new PlayerLook(playerBodyPrefab, playerSensitivity, maxAnglyY);
        playerMovement = new PlayerMovement(playerBodyPrefab, groundCheck, playerWalkSpeed, playerSneakSpeed, groundDistance, groundLayer, distanceToTravelPerStep, footStepSounds);
        playerHeadBob = new PlayerHeadBob(playerBodyPrefab, bobbingSpeed, bobbingAmount);

        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
        EventSystem.Subscribe(EventType.GAME_WON, GameWon);
    }

    void Update()
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

    void FixedUpdate()
    {
        EventSystem.RaiseEvent(EventType.FIXED_UPDATE);
    }

    void PlayerAttacked(Transform _transform)
    {
        crossHair.SetActive(false);
        Destroy(playerBodyPrefab.GetComponent<Rigidbody>());
        gameOver = true;
        EventSystem<Transform>.Unsubscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
        EventSystem.Unsubscribe(EventType.GAME_WON, GameWon);
    }

    void GameWon()
    {
        crossHair.SetActive(false);
        gameWonPanel.SetActive(true);
        gameWonPanel.GetComponent<Animator>().SetTrigger("GameWon");
    }
}
