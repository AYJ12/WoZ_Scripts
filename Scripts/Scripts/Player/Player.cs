using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public GameObject player;
    public Transform playerTransform;
    public PlayerEventController playerEvent;
    public PlayerInputController playerInput;
    public PlayerStatHandler playerStatHandler;
    public InteractionManager interactionManager;
    public WeaponSwitchSystem playerSwitch;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        if(instance == null)
        {
            instance = this;
        }
        player = gameObject;
        playerTransform = player.transform;
        playerEvent = GetComponent<PlayerEventController>();
        playerInput = GetComponent<PlayerInputController>();
        playerStatHandler = GetComponent<PlayerStatHandler>();
        interactionManager = GetComponent<InteractionManager>();
        playerSwitch = GetComponent<WeaponSwitchSystem>();
    }

    public static Player Instance
    {
        get
        {
            if( instance == null )
                return null;
            return instance;
        }
    }

    public bool PossibleWeaponZoom()
    {
        if (playerSwitch._currentWeapon.weaponType != WeaponType.Melee && playerSwitch._currentWeapon.weaponType != WeaponType.Throw)
            return true;
        return false;
    }
}
