using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraBehaviour : MonoBehaviour
{
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
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.position);
            }
        }
    }
}
