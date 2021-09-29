using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumpscare
{

    private GameObject playerBodyPrefab;
    private Transform jumpscareTrigger;

    private AudioSource scream;
    private AudioClip screamClip;
    private GameObject jumpscareCam;
    private GameObject flashingImage;

    public Jumpscare(GameObject _playerBodyPrefab, Transform _jumpscareTrigger, GameObject _jumpscareCam, GameObject _flashingImage, AudioSource _scream, AudioClip _screamClip)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem.Subscribe(EventType.FIXED_UPDATE, FixedUpdate);

        playerBodyPrefab = _playerBodyPrefab;
        jumpscareTrigger = _jumpscareTrigger;
        jumpscareCam = _jumpscareCam;
        flashingImage = _flashingImage;
        scream = _scream;
        screamClip = _screamClip;

        playerBodyPrefab.SetActive(true);
        jumpscareCam.SetActive(false);
        flashingImage.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        jumpscareCam.transform.position = playerBodyPrefab.transform.position;

        if(playerBodyPrefab.transform.position.z > jumpscareTrigger.position.z)
        {
            playerBodyPrefab.SetActive(false);
            jumpscareCam.SetActive(true);
            flashingImage.SetActive(true);
            scream.PlayOneShot(screamClip, 0.02f);
        }
        else
        {
            playerBodyPrefab.SetActive(true);
            jumpscareCam.SetActive(false);
            flashingImage.SetActive(false);
        }
    }

    private void FixedUpdate()
    {

    }
}
