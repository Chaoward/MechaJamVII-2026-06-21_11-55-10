using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InputHandler : MonoBehaviour
{
    public string startingMap;
    public static InputActionMap curActionMap { get; private set; } = null;

    public static Vector2 moveInput;
    public static Vector2 lookInput;

    private static UnityInputAsset inputAsset;

    void Awake()
    {
        inputAsset = new UnityInputAsset();

        foreach (var map in inputAsset.asset.actionMaps)
        {
            if (startingMap == map.name)
            {
                curActionMap = map;
                curActionMap.Enable();
            }
            else
                map.Disable();
        }
    }

    void Update()
    {
        moveInput = inputAsset.Player.Move.ReadValue<Vector2>();
        lookInput = inputAsset.Player.Look.ReadValue<Vector2>();
    }


    // 0 => success, 1 => can't find action in current action map
    public static int BindInput(string actionName, Action<InputAction.CallbackContext> binding)
    {
        InputAction trgtAction = FindAction(actionName);

        //check if target action exist
        if (trgtAction == null)
            return 1;

        //bind to input event
        //trgtAction.started += binding;
        trgtAction.performed += binding;
        trgtAction.canceled += binding;

        return 0;
    }

    // 0 => success, 1 => can't find action in current action map
    public static int UnbindInput(string actionName, Action<InputAction.CallbackContext> binding)
    {
        InputAction trgtAction = FindAction(actionName);

        //check if target action exist
        if (trgtAction == null)
            return 1;

        //bind to input event
        trgtAction.performed -= binding;
        trgtAction.canceled -= binding;

        return 0;
    }

    // 0 => success, 1 => can't find action map, -1 => is current map
    public int SetActionMap(string mapName)
    {
        if (curActionMap.name == mapName) return -1;

        //find and set action map
        foreach (var actionMap in inputAsset.asset.actionMaps)
        {
            if (actionMap.name == mapName)
            {
                actionMap.Enable();
                curActionMap.Disable();
                curActionMap = actionMap;
                return 0;
            }
        }

        return 1;
    }


    private static InputAction FindAction(string actionName)
    {
        foreach (var action in curActionMap.actions)
        {
            if (action.name == actionName)
                return action;
        }

        return null;
    }


}
