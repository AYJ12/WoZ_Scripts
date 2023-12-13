using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    private UI_GameScene _uiGameScene;
    private WeaponSwitchSystem _weaponSwitchSystem;
    private PlayerEventController _playerEventController;

    [Header("Weapon Base")]
    [SerializeField] private TextMeshProUGUI _weaponName; 
    [SerializeField] private Image _weaponImage;
    [SerializeField] private Sprite[] _weaponSprites; 
    [SerializeField] private Vector2[] sizeWeaponIcons; 

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI _weaponAmmo; // ����/�ִ� ź �� ��� Text

    private void Awake()
    {
        _uiGameScene = GetComponent<UI_GameScene>();
        _playerEventController = Player.Instance.playerEvent;
        _weaponSwitchSystem = Player.Instance.GetComponent<WeaponSwitchSystem>();
    }

    private void Start()
    {
        _weaponName = _uiGameScene.GetTextMesh((int)UI_GameScene.TextMeshs.WeaponName);
        _weaponAmmo = _uiGameScene.GetTextMesh((int)UI_GameScene.TextMeshs.WeaponAmmo);
        _weaponImage = _uiGameScene.GetImage((int)UI_GameScene.Images.WeaponIcon);
        _weaponName.text = _weaponSwitchSystem._currentWeapon.weaponData.weaponName.ToString();
        _weaponImage.sprite = _weaponSwitchSystem._currentWeapon.weaponData.viewImage;
        SetupWeapon();
    }

    private void LateUpdate()
    {
        SetupWeapon();
    }

    //public void SetupStatUIWeapons()
    //{
    //    if(_weaponSwitchSystem._currentWeapon.weaponType != WeaponType.Melee)
    //        _weaponSwitchSystem._currentWeapon.onAmmoEvent.AddListener(UpdateAmmoHUD);
    //}

    //public void SwitchingWeapon(int pressNum)
    //{
    //    SetupWeapon();
    //}



    private void SetupWeapon()
    {
        _weaponName.text = _weaponSwitchSystem._currentWeapon.weaponData.weaponName.ToString();
        _weaponImage.sprite = _weaponSwitchSystem._currentWeapon.weaponData.viewImage;
        int currentammo = AmmoManager.instance.GetAmmoCount(_weaponSwitchSystem._currentWeapon.weaponType);
        int maxammo = 0;
        switch (_weaponSwitchSystem._currentWeapon.weaponType)
        {
            case WeaponType.Main:
                maxammo = Player.Instance.playerStatHandler._playerRifleMaxAmmo;
                break;
            case WeaponType.Sub:
                maxammo = Player.Instance.playerStatHandler._playerSniperMaxAmmo;
                break;
        }
        
        if (_weaponSwitchSystem._currentWeapon.weaponType == WeaponType.Melee)
        {
            _weaponAmmo.text = "∞";
            return;
        }

        if(_weaponSwitchSystem._currentWeapon.weaponType == WeaponType.Throw)
        {
            _weaponAmmo.text = $"<size=50><color=#EADF9F>{currentammo}";
            return;
        }

        if(currentammo <= 5 ) // 5알 남으면
        {
            _weaponAmmo.text = $"<size=50><color=#ff0000>{currentammo}</color></size> / {maxammo}"; // 빨강
        }
        else
        {
            _weaponAmmo.text = $"<size=50><color=#EADF9F>{currentammo}</color></size> / {maxammo}"; // 노랑
        }
       
    }
}
