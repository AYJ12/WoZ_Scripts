using System;
using UnityEngine;

public class PlayerEventController : MonoBehaviour
{
    public event Action KeyDownEvent;
    public event Action KeyUpEvent;
    public event Action KeyPressEvent;

    public event Action OnHitEvent;
    public event Action OnAttackEvent;
    //public event Action EquipEvent;
    //public event Action UnEquipEvent;

    public event Action OnZoomEvent;
    public event Action OnInteractEvent;

    public event Action OnRunnigEvent;
    public event Action OnSitDownEvent;
    public event Action OnReloadEvent;
    public event Action OnInventoryEvent;
    public event Action OnWeaponInvenEvent;
    public event Action OnQuestEvent;
    public event Action OnGuideEvent;
    public event Action OnQuitUIEvent;
    public event Action OnMapUIEvent;
    public event Action<int> OnSwitchEvent;
    public event Action<Vector3> OnMoveEvent;


    public void CallMouseKeyDown()   //key down call event
    {
        KeyDownEvent?.Invoke();
    }

    public void CallMouseKeyUp()     //key up call event
    {
        KeyUpEvent?.Invoke();
    }

    public void CallMouseKeyPress()      //key press call event
    {
        KeyPressEvent?.Invoke();
    }

    public void CallMove(Vector3 direction)      //on move event
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallAttack()    //on attack event
    {
        OnAttackEvent?.Invoke();
    }
    public void CallHit()
    {
        OnHitEvent?.Invoke();
    }

    public void CallRunnig()
    {
        OnRunnigEvent?.Invoke();
    }

    public void CallSitDown()
    {
        OnSitDownEvent?.Invoke();
    }

    public void CallReload()
    {
        OnReloadEvent?.Invoke();
    }

    public void CallZoom()
    {
        OnZoomEvent?.Invoke();
    }

    public void CallSwitch(int numPress)
    {
        OnSwitchEvent?.Invoke(numPress);
    }

    public void CallInteract()
    {
        OnInteractEvent?.Invoke();
    }

    public void CallInventory()
    {
        OnInventoryEvent?.Invoke();
    }

    public void CallWeaponInven()
    {
        OnWeaponInvenEvent?.Invoke();
    }

    public void CallQuest()
    {
        OnQuestEvent?.Invoke();
    }

    public void CallGuide()
    {
        OnGuideEvent?.Invoke();
    }

    public void CallQuitUI()
    {
        OnQuitUIEvent?.Invoke();
    }

    public void CallMapUI()
    {
        OnMapUIEvent?.Invoke();
    }

}