using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    //We make an instance of this object, this is called a singleton
    //Meaning that we can use this exact same object for different tasks
    public static InputManager Instance { get; private set; }
    //In this case we create a singleton for our controller, so other objects have easier access to this data
    private InputController controller;

    private void Start()
    {
        //We check if we already have a GameObject with this script
        if (Instance != null && Instance != this)
        {
            //If we do, we destroy any clones
            Destroy(this);
        }
        else
        {
            //If we dont, then that means we only have on GameObject with this script
            Instance = this;
        }
    }

    private void Awake()
    {
        //We create a new InputController
        controller = new InputController();
    }

    private void OnEnable()
    {
        //Enable it, right after Awake
        controller.Enable();
    }

    private void OnDisable()
    {
        //Disable it, if we stop using the input system
        controller.Disable();
    }

    public Vector2 MousePositionInScreenPosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public bool OnClick()
    {
        //We use the systems from controller to check if we triggered the left button of our mouse
        return controller.Mouse.Click.triggered;
    }

    public bool GoLeft()
    {
        return controller.Keyboard.A.triggered;
    }

    public bool GoRight()
    {
        return controller.Keyboard.D.triggered;
    }

    public bool OnExitClick()
    {
        return controller.Mouse.Click.WasReleasedThisFrame();
    }

    public bool OnHoldClick()
    {
        return controller.Mouse.Click.IsPressed();
    }

    public bool OnHoldA()
    {
        return controller.Keyboard.A.IsPressed();
    }

    public bool OnHoldD()
    {
        return controller.Keyboard.D.IsPressed();
    }
}
