using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    private bool hovering = false;
    [SerializeField] LayerMask IgnoreMask;
    [SerializeField] Transform buyPoint;
    private bool pressingKey = false;
    [SerializeField] Image AKey;
    [SerializeField] Image DKey;
    [SerializeField] Sprite APressed;
    [SerializeField] Sprite ADefault;
    [SerializeField] Sprite DPressed;
    [SerializeField] Sprite DDefault;

    private void Update()
    {
        buyPoint.position = new Vector3(transform.position.x, buyPoint.position.y, buyPoint.position.z);
        //We use the singleton to check if we press the left button of our mouse
        if (InputManager.Instance.OnClick())
        {

            Vector2 mousePos = InputManager.Instance.MousePositionInScreenPosition();
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out hit,10000, ~IgnoreMask))
            {
                if (hit.transform.name == "Radio")
                {
                    EventManager.TriggerEvent("ChangedVolume");
                }

                if (hit.transform.gameObject.tag == "Interactable")
                {
                    if(hit.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
                    {
                        interactable.ClickInteraction();
                    }

                }

                if(hit.transform.gameObject.tag == "Squirrel")
                {
                    GrabSquirrel(hit.transform);
                }
            }
        }
        if (!pressingKey)
        {
            if (InputManager.Instance.OnHoldA())
                StartCoroutine(MoveCameraWithKeyboard(-1, "A"));

            if (InputManager.Instance.OnHoldD())
                StartCoroutine(MoveCameraWithKeyboard(1, "D"));
        }

    }

    
    private void GrabSquirrel(Transform squirrel)
    {
        NavMeshAgent agent = squirrel.GetComponent<NavMeshAgent>();
        StartCoroutine(GrabbedSquirrel(squirrel, agent));       
    }

    private IEnumerator GrabbedSquirrel(Transform squirrel, NavMeshAgent agent)
    {
        Vector3 originalScale = squirrel.localScale;
        agent.updatePosition = false;
        //squirrel.localScale *= 1.25f;
        Vector2 mousePos;
        Vector3 squirrelPos;
        while (InputManager.Instance.OnHoldClick())
        {
            mousePos = Mouse.current.position.ReadValue();
            squirrelPos = Camera.main.ScreenToWorldPoint(mousePos);
            squirrel.position = new Vector3(squirrelPos.x, squirrel.position.y, squirrel.position.z);
            yield return new WaitForEndOfFrame();
        }
        mousePos = Mouse.current.position.ReadValue();
        squirrelPos = Camera.main.ScreenToWorldPoint(mousePos);
        squirrel.position = new Vector3(squirrelPos.x, squirrel.position.y, squirrel.position.z);
        agent.nextPosition = new Vector3(squirrelPos.x, squirrel.position.y, squirrel.position.z);
        agent.updatePosition = true;
        squirrel.localScale = originalScale;
    }

    public void OnLeftSideHover()
    {
        StartCoroutine(MoveCamera(-1));
    }

    public void OnRightSideHover()
    {
        StartCoroutine(MoveCamera(1));
    }

    private IEnumerator MoveCamera(int movement)
    {
        hovering = true;
        yield return new WaitForSeconds(.09f);
        while (hovering)
        {
            float nextMovement = movement * Time.deltaTime * speed;
            if (transform.position.x + nextMovement > -35.09f && transform.position.x + nextMovement < 37.59f)
                transform.position = new Vector3(transform.position.x + nextMovement, transform.position.y, transform.position.z);
            yield return new WaitForEndOfFrameUnit();
        }
    }

    private IEnumerator MoveCameraWithKeyboard(int movement, string key)
    {
        pressingKey = true;
        bool pressing;
        yield return new WaitForSeconds(.05f);
        if (key == "A")
        {
            pressing = InputManager.Instance.OnHoldA();
            AKey.sprite = APressed;
        }
        else
        {
            pressing = InputManager.Instance.OnHoldD();
            DKey.sprite = DPressed;
        }
        
        while (pressing)
        {
            if (key == "A")
                pressing = InputManager.Instance.OnHoldA();
            else
                pressing = InputManager.Instance.OnHoldD();
            float nextMovement = movement * Time.deltaTime * speed * 1.5f;
            if (transform.position.x + nextMovement > -35.09f && transform.position.x + nextMovement < 37.59f)
                transform.position = new Vector3(transform.position.x + nextMovement, transform.position.y, transform.position.z);
            yield return new WaitForEndOfFrameUnit();
        }
        pressingKey = false;
        DKey.sprite = DDefault;
        AKey.sprite = ADefault;
    }

    public void OnHoveringSideExit()
    {
        hovering = false;
    }
}
