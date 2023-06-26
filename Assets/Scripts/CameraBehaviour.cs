using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        if (PlayerInput._Instance.OnClick())
        {
            //If we do we get the position of our mouse on screen
            Vector2 mousePos = Mouse.current.position.ReadValue();
            // create a hit which will tell us where the hit ended up in game space
            RaycastHit hit;
            //A ray using position of our mouse, we use this to look where we are pointing
            Ray ray = Camera.main.ScreenPointToRay(mousePos);

            //If we hit something with the ray, we take OUT the information on hit
            if (Physics.Raycast(ray, out hit,10000, ~IgnoreMask))
            {
                if (hit.transform.gameObject.tag == "Interactable")
                {
                    EventManager.TriggerEvent("Clicked", hit.transform.name);
                }
                if(hit.transform.gameObject.tag == "Squirrel")
                {
                    GrabSquirrel(hit.transform);
                }
            }
        }
        if (!pressingKey)
        {
            if (PlayerInput._Instance.OnHoldA())
                StartCoroutine(MoveCameraWithKeyboard(-1, "A"));

            if (PlayerInput._Instance.OnHoldD())
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
        while (PlayerInput._Instance.OnHoldClick())
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
            pressing = PlayerInput._Instance.OnHoldA();
            AKey.sprite = APressed;
        }
        else
        {
            pressing = PlayerInput._Instance.OnHoldD();
            DKey.sprite = DPressed;
        }
        
        while (pressing)
        {
            if (key == "A")
                pressing = PlayerInput._Instance.OnHoldA();
            else
                pressing = PlayerInput._Instance.OnHoldD();
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
