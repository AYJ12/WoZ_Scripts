using UnityEngine;

public class WeaponSwitchSystem : MonoBehaviour
{
    [SerializeField] private PlayerMovement _playerMovement;
    private PlayerEventController _playerEvent;

    [SerializeField] private PlayerHUD _playerHUD;
    [SerializeField] private WeaponInventory _weaponInventory;

    public WeaponBase _currentWeapon;
    private WeaponBase _previousWeapon;

    private void Awake()
    { 
        _playerEvent = GetComponent<PlayerEventController>();
    }

    private void Start()
    {
        SwitchingWeapon(WeaponType.Main);
        _playerEvent.OnSwitchEvent += UpdateSwitch;
    }


    private void UpdateSwitch(int pressNum)
    {
        Debug.Log("UpdateSwitch");
        SwitchingWeapon((WeaponType)pressNum);
        _currentWeapon.gameObject.SetActive(true);
    }

    private void SwitchingWeapon(WeaponType weaponType)
    {
        int slotIndex = (int)weaponType;

        if (WeaponManager.instance.PlayerweaponsList[slotIndex] == null)
        {
            return;
        }

        if (_currentWeapon != null)
        {
            _previousWeapon = _currentWeapon;
        }

        _currentWeapon = WeaponManager.instance.PlayerweaponsList[slotIndex];

        if (_currentWeapon == _previousWeapon)
        {
            return;
        }

        _playerMovement.SwitchingWeapon(_currentWeapon); 

        if (_previousWeapon != null)
        {
            Player.Instance.playerStatHandler.playerLookType = PlayerLookType.Idle;
            _previousWeapon.gameObject.SetActive(false); // �� �� ���� ���
        }
        _currentWeapon.gameObject.SetActive(true); // ���� ���� �ѱ�

    }

    public void GetCurrentWeaponType(int pressNum)
    {
        SwitchingWeapon((WeaponType)pressNum);
    }

}

