using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : PlayerEventController
{
    [SerializeField]
    private Camera _basicCamera;
    [SerializeField]
    private PlayerStat _updateStat;
    private PlayerStatHandler _playerStatHandler;
    public bool canLook = true;
    private bool isSwitch = false;

    private void Start()
    {
        _playerStatHandler = Player.Instance.playerStatHandler;
        _basicCamera = Camera.main;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>().normalized;
        Vector3 moveDirect = new Vector3(input.x, 0f, input.y);
        if(canLook)
            CallMove(moveDirect);
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Performed)
        {
            //_updateStat.isAttacking = true;
            _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, _playerStatHandler.playerLookType, PlayerActionState.Attack);
            CallAttack();
        }
        else
        {
            //_updateStat.isAttacking = false;
            _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, _playerStatHandler.playerLookType, PlayerActionState.Move);
            CallAttack();
        }
    }
    public void OnZoomMode(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Started)
        {
            //_updateStat.isZoom = !_playerStatHandler.currentStat.isZoom;
            if (Player.Instance.PossibleWeaponZoom())
            {
                if(_playerStatHandler.playerLookType == PlayerLookType.Idle)
                    _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, PlayerLookType.Zoom, _playerStatHandler.playerState);
                else
                    _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, PlayerLookType.Idle, _playerStatHandler.playerState);
            }
            CallZoom();
            Debug.Log(_playerStatHandler.playerLookType);
        }
        //else if (context.phase == InputActionPhase.Canceled)
        //{
        //    //_updateStat.isZoom = !_playerStatHandler.currentStat.isZoom;
        //    _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, PlayerLookType.Idle, _playerStatHandler.playerState);
        //    CallZoom();
        //}
    }

    public void OnRunning(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Performed)
        {
            //_updateStat.isRunning = true;
            _playerStatHandler.StatesUpdateEvent(MoveType.Run, _playerStatHandler.playerLookType, _playerStatHandler.playerState);
        }
        else
        {
            //_updateStat.isRunning = false;
            _playerStatHandler.StatesUpdateEvent(MoveType.Walk, _playerStatHandler.playerLookType, _playerStatHandler.playerState);
        }
        CallRunnig();
    }

    public void OnSitDown(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Performed)
        {
            //_updateStat.isSitdown = true;
            _playerStatHandler.StatesUpdateEvent(MoveType.Sit, _playerStatHandler.playerLookType, _playerStatHandler.playerState);
        }
        else
        {
            //_updateStat.isSitdown = false;
            _playerStatHandler.StatesUpdateEvent(MoveType.Walk, _playerStatHandler.playerLookType, _playerStatHandler.playerState);
        }
        CallSitDown();
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Started)
        {
            //_updateStat.isReloading = true;
            _playerStatHandler.StatesUpdateEvent(_playerStatHandler.moveType, _playerStatHandler.playerLookType, PlayerActionState.Reload);
            CallReload();
        }
        else
        
            //_updateStat.isReloading = false;
            //_playerStatHandler.StatsUpdateEvent(_updateStat); 
            // 애니메이션 끝났을 때 타입변경해주기
            return;
        
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        int numberKeyPressed = int.Parse(context.control.name);
        if (context.phase == InputActionPhase.Performed && !isSwitch)
        {
            isSwitch = true;

            if (numberKeyPressed > 0 && numberKeyPressed < 5)
            {
                CallSwitch(numberKeyPressed - 1); // -1�� �迭 �ε����� ��ȯ
            }
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isSwitch = false;
        }
        CallSwitch(numberKeyPressed - 1);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (!canLook)
            return;
        if (context.phase == InputActionPhase.Started)
        {
            CallInteract();
        }
        else
            return;
    }

    public void OnQuest(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallQuest();
        }
        else
            return;
    }

    public void OnWeaponInven(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallWeaponInven();
        }
        else
            return;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallInventory();
        }
        else
            return;
    }

    public void OnGuideUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallGuide();
        }
    }

    public void OnQuitUI(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            CallQuitUI();
        }
    }

    public void OnMapUI(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            CallMapUI();
        }
        else
            return;
    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
        Cursor.visible = toggle;
    }
}