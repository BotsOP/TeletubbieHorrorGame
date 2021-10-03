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

    [SerializeField] private GameObject[] jumpscareTriggers;

    private List<Jumpscare> JumpscaresScripts = new List<Jumpscare>();

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < jumpscareTriggers.Length; i++)
        {
            Jumpscare jumpscareScript = new Jumpscare(playerBodyPrefab, jumpscareTriggers[i].transform, jumpscareCam, flashingImage, scream, screamClip);
            JumpscaresScripts.Add(jumpscareScript);
        }
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
