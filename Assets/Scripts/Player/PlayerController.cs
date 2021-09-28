using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    private GameObject playerBodyPrefab;

    private Camera cam;

    private LayerMask pickUpLayers;
    private LayerMask layerMask;
    private RaycastHit hit;
    private Ray ray;
    private float maxRayDistance;

    private GameObject holdingObject;

    private Dictionary<GameObject, GameObject> objectsToOpen = new Dictionary<GameObject, GameObject>();

    public PlayerController(GameObject _playerBodyPrefab, Camera _cam, LayerMask _layerMask, LayerMask _pickUpLayers, float _maxRayDistance, Dictionary<GameObject, GameObject> _objectsToOpen)
    {
        objectsToOpen = _objectsToOpen;
        playerBodyPrefab = _playerBodyPrefab;
        layerMask = _layerMask;
        pickUpLayers = _pickUpLayers;
        cam = _cam;
        maxRayDistance = _maxRayDistance;

        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);

            CheckForRayHit();
        }
    }

    private void CheckForRayHit()
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, layerMask, QueryTriggerInteraction.Ignore))
        {
            if (hit.transform.GetComponent<Animator>())
            {
                if (AnimatorHasParameter("TriggerDoor", hit.transform.GetComponent<Animator>()) && !CheckForLock(hit.transform.gameObject))
                {
                    hit.transform.GetComponent<Animator>().SetTrigger("TriggerDoor");
                }
                else if (AnimatorHasParameter("TriggerDoor", hit.transform.GetComponent<Animator>()) && CheckForLock(hit.transform.gameObject))
                {
                    if (objectsToOpen[hit.transform.gameObject] == holdingObject)
                    {
                        objectsToOpen.Remove(hit.transform.gameObject);
                        hit.transform.GetComponent<Animator>().SetTrigger("TriggerDoor");
                    }
                }
            }

            if (IsInLayerMask(hit.transform.gameObject, pickUpLayers))
            {
                holdingObject = hit.transform.gameObject;
                hit.transform.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Did not Hit");
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
