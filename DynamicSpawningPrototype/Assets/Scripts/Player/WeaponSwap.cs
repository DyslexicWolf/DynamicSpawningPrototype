using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSwap : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] weapons;
    
    private float switchTime = 1f;
    private int activeWeaponIndex = 0;
    private float timeSinceLastSwitch;
    private PlayerInputActions playerInputActions;
    private Vector2 scrollInput;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Scroll.performed += OnScrollPerformed;
        playerInputActions.Player.Scroll.canceled += OnScrollCancelled;
    }

    private void OnScrollCancelled(InputAction.CallbackContext context)
    {
        scrollInput = Vector2.zero;
    }

    private void OnScrollPerformed(InputAction.CallbackContext context)
    {
        scrollInput = context.ReadValue<Vector2>();
    }
    private void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    private void Start()
    {
        SetWeapons();
        SelectWeapon(activeWeaponIndex);

        timeSinceLastSwitch = 0f;
    }

    private void Update()
    {
        if(scrollInput.y > 0f && timeSinceLastSwitch >= switchTime)
        {
            activeWeaponIndex++;
            if(activeWeaponIndex >= weapons.Length)
            {
                activeWeaponIndex = 0;
            }
            SelectWeapon(activeWeaponIndex);
        }

        if (scrollInput.y < 0f && timeSinceLastSwitch >= switchTime)
        {
            activeWeaponIndex--;
            if(activeWeaponIndex < 0)
            {
                activeWeaponIndex = weapons.Length - 1;
            }
            SelectWeapon(activeWeaponIndex);
        }

        timeSinceLastSwitch += Time.deltaTime;
    }

    private void SetWeapons()
    {
        //because this is on the weaponholder, it will count the number of children, setting the length of the array
        //later on I can change this with perks if programmed this way
        weapons = new Transform[transform.childCount];

        for(int i = 0; i < weapons.Length; i++)
        {
            weapons[i] = transform.GetChild(i);
        }
    }

    private void SelectWeapon(int weaponIndex)
    {
        for(int i = 0; i < weapons.Length; i++)
        {
            if(i == weaponIndex)
            {
                weapons[i].gameObject.SetActive(true);
                activeWeaponIndex = i;
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
        timeSinceLastSwitch = 0f;

        OnWeaponSelected();
    }

    private void OnWeaponSelected()
    {
        Debug.Log("On weapon selected logic still needs to be implemented.");
    }
}
