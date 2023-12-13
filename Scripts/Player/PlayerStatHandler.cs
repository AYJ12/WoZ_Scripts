using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveType
{
    Sit,
    Walk,
    Run,
    Die
}

public enum PlayerLookType
{
    Idle,
    Zoom
}

public enum PlayerActionState
{
    Move,
    Attack,
    Reload
}

public class PlayerStatHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerData _playerData;

    public MoveType moveType;
    public PlayerLookType playerLookType;
    public PlayerActionState playerState;
    private GameObject _attackGo;
    private ZombieControl _zombieControl;
    private UI_DamageIndicator _uI_HitEffect;
    public const float MAX_HP = 100.0f;
    public const float MAX_STAMINA = 100.0f;
    public float playerMaxHp = MAX_HP;
    public float playerMaxStamina = MAX_STAMINA;
    public int _playerRifleMaxAmmo = 100; public int _playerSniperMaxAmmo = 70;


    public List<PlayerStat> statsModifiers = new List<PlayerStat>();
    void Awake()
    {
        currentStat = new PlayerStat();
        PlayerSetStat();
        AmmoManager.instance.SetRifleMaxAmmo(_playerRifleMaxAmmo);
        AmmoManager.instance.SetSniperMaxAmmo(_playerSniperMaxAmmo);
    }

    private void Start()
    {
        _uI_HitEffect = Manager.UI.SceneUI.GetComponent<UI_DamageIndicator>();
    }

    private void LateUpdate()
    {
        _playerRifleMaxAmmo = AmmoManager.instance.GetRifleMaxAmmo(); // 라이풀 최대 탄약 수 받기
        _playerSniperMaxAmmo = AmmoManager.instance.GetSniperMaxAmmo(); // 스나이퍼 최대 탄약 수 받기
    }

    public PlayerStat currentStat { get; private set; }

    public void PlayerSetStat()
    {
        moveType = MoveType.Walk;
        playerLookType = PlayerLookType.Idle;
        playerState = PlayerActionState.Move;
        currentStat.playerHp = _playerData.hp;
        currentStat.playerStamina = _playerData.stamina;
        currentStat.playerMoveSpeed = _playerData.moveSpeed;
        //currentStat.isRunning = _playerData.isRunning;
        //currentStat.isAttacking = _playerData.isAttacking;
        //currentStat.isZoom = _playerData.isZoom;
        //currentStat.isSitdown = _playerData.isSitdown;
        //currentStat.isReloading = _playerData.isReloading;

    }
    public void StatesUpdateEvent(MoveType moveModifire, PlayerLookType lookModifire, PlayerActionState actionModifre)
    {
        //currentStat.isAttacking = modifire.isAttacking;
        //currentStat.isRunning = modifire.isRunning;
        //currentStat.isReloading = modifire.isReloading;
        //currentStat.isSitdown = modifire.isSitdown;
        //currentStat.isZoom = modifire.isZoom;
        moveType = moveModifire;
        playerLookType = lookModifire;
        playerState = actionModifre;
    }

    public void SetMoveType(MoveType move)
    {
        moveType = move;
    }

    public void SetLookType(PlayerLookType look)
    {
        playerLookType = look;
    }

    public void SetState(PlayerActionState state)
    {
        playerState = state;
    }
    private void Update()
    {
        if (moveType != MoveType.Run)
        {
            Add(0.0f, 10.0f * Time.deltaTime);
        }
    }

    public void Sub(float hp, float stamina)
    {
        currentStat.playerHp -= hp;
        currentStat.playerStamina -= stamina;
        currentStat = LimitALLStats();
        if (currentStat.playerHp <= 0)
        {
            moveType = MoveType.Die;
            Die();
        }
    }

    public void Add(float hp, float stamina)
    {
        currentStat.playerHp += hp;
        currentStat.playerStamina += stamina;
        currentStat = LimitALLStats();
    }

    private PlayerStat LimitALLStats()
    {
        currentStat.playerHp = Mathf.Min(MAX_HP, currentStat.playerHp);
        currentStat.playerStamina = Mathf.Min(MAX_STAMINA, currentStat.playerStamina);
        return currentStat;
    }

    private void OnCollisionEnter(Collision other) //??????????
    {
        //_attackGo = other.gameObject.transform.root.gameObject;
        if (other.collider.CompareTag("ZombieHand"))
        {
            _attackGo = other.gameObject;
            _zombieControl = _attackGo.GetComponentInParent<ZombieControl>();
            if (_zombieControl.isAttacking)
            {
                _uI_HitEffect.Flash();
                Sub(_zombieControl.attackDamage, 0.0f);  // zombie ?????
                Manager.Sound.Play("Damage_Zombie");
            }
        }
    }

    private void Die()
    {
        Manager.Sound.Play("Die");
        Manager.UI.ShowPopupUI<UI_QuitPopUp>("QuitGame");
        GetComponent<PlayerInputController>().ToggleCursor(true);
        Time.timeScale = 0;

    }

    // 총알 아이템 충전 메서드
    public void UpdateAmmo(Item _item)
    {
        switch (_item.ammoType)
        {
            case AmmoType.Rifle:
                _playerRifleMaxAmmo += _item.rifleAmmoValue;
                AmmoManager.instance.SetRifleMaxAmmo(_playerRifleMaxAmmo);
                break;
            case AmmoType.Sniper:
                _playerSniperMaxAmmo += _item.sniperAmmoValue;
                AmmoManager.instance.SetSniperMaxAmmo(_playerSniperMaxAmmo);
                break;
            case AmmoType.Total:
                _playerRifleMaxAmmo += _item.rifleAmmoValue;
                AmmoManager.instance.SetRifleMaxAmmo(_playerRifleMaxAmmo);
                _playerSniperMaxAmmo += _item.sniperAmmoValue;
                AmmoManager.instance.SetSniperMaxAmmo(_playerSniperMaxAmmo);
                break;
        }
    }
}
