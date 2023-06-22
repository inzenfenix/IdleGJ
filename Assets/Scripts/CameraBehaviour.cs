using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    private bool hovering = false;
    [SerializeField] LayerMask IgnoreMask;

    private void Update()
    {
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
            }
        }
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

    public void OnHoveringSideExit()
    {
        hovering = false;
    }
}
