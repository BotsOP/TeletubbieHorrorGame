using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerKubus : MonoBehaviour
{
    [SerializeField] private Jumpscare jumpscareScript;

    [SerializeField] private Transform jumpscareTrigger;

    [SerializeField] private AudioSource scream;
    [SerializeField] private AudioClip screamClip;
    [SerializeField] private GameObject playerBodyPrefab;
    [SerializeField] private GameObject jumpscareCam;
    [SerializeField] private GameObject flashingImage;

    // Start is called before the first frame update
    void Start()
    {
        Jumpscare jumpscareScript = new Jumpscare(playerBodyPrefab, jumpscareTrigger, jumpscareCam, flashingImage, scream, screamClip);
    }

    // Update is called once per frame
    void Update()
    {
        EventSystem.RaiseEvent(EventType.UPDATE);
    }

    void FixedUpdate()
    {
        EventSystem.RaiseEvent(EventType.FIXED_UPDATE);
    }
}
