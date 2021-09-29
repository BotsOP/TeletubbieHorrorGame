using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController
{
    private GameObject playerBodyPrefab;

    private Camera cam;

    private LayerMask pickUpLayers;
    private LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;
    private float maxRayDistance;
    private bool isHolding = false;

    private GameObject holdingObject;
    private TextMeshProUGUI textForInteraction;

    private Dictionary<GameObject, GameObject> objectsToOpen = new Dictionary<GameObject, GameObject>();

    public PlayerController(GameObject _playerBodyPrefab, Camera _cam, LayerMask _layerMask, LayerMask _pickUpLayers, float _maxRayDistance, Dictionary<GameObject, GameObject> _objectsToOpen, TextMeshProUGUI _textForInteraction)
    {
        objectsToOpen = _objectsToOpen;
        playerBodyPrefab = _playerBodyPrefab;
        layerMask = _layerMask;
        pickUpLayers = _pickUpLayers;
        cam = _cam;
        maxRayDistance = _maxRayDistance;
        textForInteraction = _textForInteraction;

        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        MouseOver();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            MouseClick();
        }
    }

    private void MouseClick()
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponent<Animator>())
            {
                Animator animator = hit.transform.GetComponent<Animator>();

                if (AnimatorHasParameter("TriggerDoor", animator) && !CheckForLock(hit.transform.gameObject))
                {
                    hit.transform.GetComponent<Animator>().SetBool("TriggerDoor", !animator.GetBool("TriggerDoor"));
                }
                else if (AnimatorHasParameter("TriggerDoor", animator) && CheckForLock(hit.transform.gameObject))
                {
                    if (objectsToOpen[hit.transform.gameObject] == holdingObject)
                    {
                        objectsToOpen.Remove(hit.transform.gameObject);
                        //hit.transform.GetComponent<Animator>().SetTrigger("TriggerDoor");
                    }
                }
            }

            if (IsInLayerMask(hit.transform.gameObject, pickUpLayers) && !isHolding)
            {
                isHolding = true;
                holdingObject = hit.transform.gameObject;
                hit.transform.gameObject.SetActive(false);
            }
        }
        else
        {
        }
    }

    private void MouseOver()
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (IsInLayerMask(hit.transform.gameObject, pickUpLayers) && !isHolding)
            {
                textForInteraction.text = "Pick Up";
            }
            else if (hit.transform.GetComponent<Animator>())
            {
                Animator animator = hit.transform.GetComponent<Animator>();

                if (AnimatorHasParameter("TriggerDoor", animator) && !CheckForLock(hit.transform.gameObject))
                {
                    if (animator.GetBool("TriggerDoor"))
                    {
                        textForInteraction.text = "Close Door";
                    }
                    else
                    {
                        textForInteraction.text = "Open Door";
                    }
                }
                else if (AnimatorHasParameter("TriggerDoor", hit.transform.GetComponent<Animator>()) && CheckForLock(hit.transform.gameObject))
                {
                    if (objectsToOpen[hit.transform.gameObject] == holdingObject)
                    {
                        textForInteraction.text = "Unlock Door";
                    }
                    else
                    {
                        textForInteraction.text = "Door Locked";
                    }
                }
            }
            else
            {
                textForInteraction.text = "";
            }
        }
        else
        {
            textForInteraction.text = "";
        }
    }

    private bool AnimatorHasParameter(string paramName, Animator animator)
    {
        foreach (AnimatorControllerParameter param in animator.parameters)
        {
            if (param.name == paramName)
                return true;
        }
        return false;
    }

    private bool CheckForLock(GameObject _object)
    {
        if (objectsToOpen.ContainsKey(_object))
        {
            return true;
        }

        return false;
    }

    public bool IsInLayerMask(GameObject _obj, LayerMask _layerMask)
    {
        return ((_layerMask.value & (1 << _obj.layer)) > 0);
    }
}
