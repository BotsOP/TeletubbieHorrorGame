using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController
{
    private GameObject objectHolder;

    private Camera cam;

    private LayerMask pickUpLayers;
    private LayerMask interactableLayers;
    private LayerMask enemyLayers;
    private RaycastHit hit;
    private Ray ray;
    private float throwForce;
    private float maxRayDistance;
    private bool isHolding = false;
    private bool canThrow = false;
    private bool canUseFlashlight = true;
    private float flashLightCoolDown;
    private float flashLightMaxUsage;
    private float flashLightTimeUsed;
    private float flashLightDistance;
    private float currentRechargeTime;

    private GameObject holdingObject;
    private TextMeshProUGUI textForInteraction;
    private GameObject flashLight;

    private Dictionary<GameObject, GameObject> objectsToOpen = new Dictionary<GameObject, GameObject>();

    public PlayerController(GameObject _objectHolder, GameObject _flashLight, float _flashLightCoolDown, float _flashLightMaxUsage, float _flashLightDistance, Camera _cam, LayerMask _interactableLayers, LayerMask _pickUpLayers, LayerMask _enemyLayers, float _maxRayDistance, Dictionary<GameObject, GameObject> _objectsToOpen, TextMeshProUGUI _textForInteraction, float _throwForce)
    {
        objectHolder = _objectHolder;
        objectsToOpen = _objectsToOpen;
        flashLight = _flashLight;
        flashLightCoolDown = _flashLightCoolDown;
        flashLightMaxUsage = _flashLightMaxUsage;
        flashLightDistance = _flashLightDistance;
        interactableLayers = _interactableLayers;
        pickUpLayers = _pickUpLayers;
        enemyLayers = _enemyLayers;
        cam = _cam;
        maxRayDistance = _maxRayDistance;
        textForInteraction = _textForInteraction;
        throwForce = _throwForce;

        EventSystem.Subscribe(EventType.UPDATE, Update);
    }

    private void Update()
    {
        ray = cam.ScreenPointToRay(Input.mousePosition);

        MouseOver();

        CheckForEnemyStun();

        FlashLightLogic();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MouseClick();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isHolding)
            {
                DropObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashLight();
        }
    }

    private void ToggleFlashLight()
    {
        if (canUseFlashlight)
        {
            flashLight.SetActive(!flashLight.activeSelf);
        }
    }

    private void FlashLightLogic()
    {
        if (flashLight.activeSelf)
        {
            flashLightTimeUsed += Time.deltaTime;

            if (flashLightTimeUsed >= flashLightMaxUsage)
            {
                flashLight.SetActive(false);
                canUseFlashlight = false;
                currentRechargeTime = 0;
                flashLightTimeUsed = 0;
            }
        }
        else if (!canUseFlashlight)
        {
            currentRechargeTime += Time.deltaTime;

            if (currentRechargeTime >= flashLightCoolDown)
            {
                canUseFlashlight = true;
            }
        }
    }

    private void CheckForEnemyStun()
    {
        if (flashLight.activeSelf)
        {
            if (Physics.Raycast(ray, out hit, maxRayDistance, enemyLayers, QueryTriggerInteraction.Ignore))
            {
                EventSystem<GameObject>.RaiseEvent(EventType.FLASHLIGHT, hit.transform.gameObject);
            }
        }
    }

    private void MouseClick()
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayers, QueryTriggerInteraction.Ignore))
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
                        Object.Destroy(holdingObject);
                        holdingObject = null;
                        //hit.transform.GetComponent<Animator>().SetTrigger("TriggerDoor");
                    }
                }
            }

            if (IsInLayerMask(hit.transform.gameObject, pickUpLayers) && !isHolding)
            {
                if (hit.transform.gameObject.tag == "Throwable")
                {
                    canThrow = true;
                }

                isHolding = true;
                hit.transform.SetParent(objectHolder.transform);
                Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
                hit.transform.GetComponent<Collider>().enabled = false;
                hit.transform.localPosition = Vector3.zero;
                hit.transform.localRotation = Quaternion.identity;
                rb.constraints = RigidbodyConstraints.FreezeAll;
                holdingObject = hit.transform.gameObject;
            }
        }
        else
        {
        }
    }

    private void MouseOver()
    {
        if (Physics.Raycast(ray, out hit, maxRayDistance, interactableLayers, QueryTriggerInteraction.Ignore))
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

    private void DropObject()
    {
        if (canThrow)
        {
            isHolding = false;
            canThrow = false;
            Rigidbody rb = holdingObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
            holdingObject.transform.GetComponent<Collider>().enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            holdingObject.transform.SetParent(null);
            rb.AddForce(objectHolder.transform.forward * throwForce, ForceMode.Impulse);
            holdingObject = null;
        }
        else
        {
            isHolding = false;
            Rigidbody rb = holdingObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = true;
            rb.isKinematic = false;
            holdingObject.transform.GetComponent<Collider>().enabled = true;
            rb.constraints = RigidbodyConstraints.None;
            holdingObject.transform.SetParent(null);
            holdingObject = null;
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
