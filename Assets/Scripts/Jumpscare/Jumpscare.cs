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

    private GameObject mainCam;

    private float jumpscareTime;
    private bool isTriggered = false;

    public Jumpscare(GameObject _playerBodyPrefab, Transform _jumpscareTrigger, GameObject _jumpscareCam, GameObject _flashingImage, AudioSource _scream, AudioClip _screamClip)
    {
        EventSystem.Subscribe(EventType.UPDATE, Update);
        EventSystem<Transform>.Subscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);

        playerBodyPrefab = _playerBodyPrefab;
        jumpscareTrigger = _jumpscareTrigger;
        jumpscareCam = _jumpscareCam;
        flashingImage = _flashingImage;
        scream = _scream;
        screamClip = _screamClip;
        mainCam = Camera.main.gameObject;

        playerBodyPrefab.SetActive(true);
        jumpscareCam.SetActive(false);
        flashingImage.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        jumpscareCam.transform.position = mainCam.transform.position;
        jumpscareCam.transform.rotation = playerBodyPrefab.transform.rotation;

        Collider[] hitColliders = Physics.OverlapBox(jumpscareTrigger.transform.position, jumpscareTrigger.GetComponent<BoxCollider>().bounds.extents, Quaternion.identity);
        if (hitColliders.Length != 0)
        {
            foreach (var item in hitColliders)
            {
                if(item.gameObject == playerBodyPrefab && !isTriggered)
                {
                    scream.gameObject.SetActive(true);
                    isTriggered = true;
                    jumpscareTrigger.transform.gameObject.SetActive(false);

                    mainCam.SetActive(false);
                    jumpscareCam.SetActive(true);
                    flashingImage.SetActive(true);
                    scream.PlayOneShot(screamClip, 0.3f);
                }
            }    
        }

        if (isTriggered)
        {
            jumpscareTime += Time.deltaTime;
            if (jumpscareTime >= 2f)
            {
                jumpscareCam.SetActive(false);
                flashingImage.SetActive(false);
                mainCam.SetActive(true);
                scream.gameObject.SetActive(false);
                isTriggered = false;
            }
        }
    }

    private void PlayerAttacked(Transform transform)
    {
        EventSystem.Unsubscribe(EventType.UPDATE, Update);
        EventSystem<Transform>.Unsubscribe(EventType.PLAYER_ATTACKED, PlayerAttacked);
    }
}
