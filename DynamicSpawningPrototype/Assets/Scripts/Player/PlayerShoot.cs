using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    private PlayerInputActions playerInputActions;
    private bool shooting;
    private bool reloading;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Shoot.performed += OnShootPerformed;
        playerInputActions.Player.Shoot.canceled += OnShootCancelled;
        playerInputActions.Player.Reload.performed += OnReloadPerformed;
        playerInputActions.Player.Reload.canceled += OnReloadCancelled;
    }

    private void OnReloadCancelled(InputAction.CallbackContext context)
    {
        reloading = false;
    }

    private void OnReloadPerformed(InputAction.CallbackContext context)
    {
        reloading = true;
    }

    private void OnShootCancelled(InputAction.CallbackContext context)
    {
        shooting = false;
    }

    private void OnShootPerformed(InputAction.CallbackContext context)
    {
        shooting = true;
    }
    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        if (shooting)
        {
            shootInput?.Invoke();
        }
        if (reloading)
        {
            reloadInput?.Invoke();
        }
    }
    
}
