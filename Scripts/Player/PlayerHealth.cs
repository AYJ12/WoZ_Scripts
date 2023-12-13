using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CharacterHealth
{
    private PlayerStatHandler _playerStatHandler;
    private const float MAX_HP = 100.0f;
    private const float MAX_STAMINA = 100.0f;
    [SerializeField] private int _playerRifleMaxAmmo = 100;
    [SerializeField] private int _playerSniperMaxAmmo = 70;

    private GameObject _rootObject;
    private GameObject _attackGo;


    private void Awake()
    {
        _playerStatHandler = Player.Instance.playerStatHandler;
    }
    private void Start()
    {
        AmmoManager.instance.SetRifleMaxAmmo(_playerRifleMaxAmmo); // 라이풀 최대 탄약 수 전달
        AmmoManager.instance.SetSniperMaxAmmo(_playerSniperMaxAmmo); // 스나이퍼 최대 탄약 수 전달
    }

    private void Update()
    {
        _playerRifleMaxAmmo = AmmoManager.instance.GetRifleMaxAmmo(); // 라이풀 최대 탄약 수 받기
        _playerSniperMaxAmmo = AmmoManager.instance.GetSniperMaxAmmo(); // 스나이퍼 최대 탄약 수 받기

        Die();
        //if (!_playerStatHandler.currentStat.isRunning)
        //{
        //    //Add(0.0f, 10.0f * Time.deltaTime);
        //}
    }

    public override void Sub(float hp, float stamina)
    {
        _playerStatHandler.currentStat.playerHp -= hp;
        _playerStatHandler.currentStat.playerStamina -= stamina;
        //_playerStatHandler = LimitALLStats(_playerStat);
    }

    //public void Add(float hp, float stamina)
    //{
    //    _playerStatHandler.currentStat.playerHp += hp;
    //    _playerStatHandler.currentStat.playerStamina += stamina;
    //    _playerStatHandler.currentStat = LimitALLStats(_playerStat);
    //}

    //private PlayerStat LimitALLStats(PlayerStatHandler playerStat)
    //{
    //    playerStat.currentStat.playerHp = Mathf.Min(MAX_HP, _playerStat.currentStat.playerHp);
    //    playerStat.currentStat.playerStamina = Mathf.Min(MAX_STAMINA, _playerStat.currentStat.playerStamina);
    //    return playerStat;
    //}

    private void OnCollisionEnter(Collision other) //���ݹ޾�����
    {
        Debug.Log("�浹");
        //_attackGo = other.gameObject.transform.root.gameObject;
        if (other.collider.tag == "ZombieHand")
        {
            _rootObject = other.gameObject.transform.root.gameObject;
            _attackGo = _rootObject.transform.GetChild(0).gameObject;
            if (_attackGo.GetComponent<ZombieControl>().isAttacking)
            {
                Debug.Log("Hit");
                Sub(_attackGo.GetComponent<ZombieControl>().attackDamage, 0.0f);  // zombie ���ݷ�
            }
        }
    }

    private void Die()
    {
        if (_playerStatHandler.currentStat.playerHp <= 0)
        {
            //Manager.SceneManager.GoEndScene();
        }
    }

    // 총알 아이템 충전 메서드
    public void UpdateAmmo(Item _item)
    {
        if (_item.ammoType == AmmoType.Rifle)
        {
            _playerRifleMaxAmmo += 30;
        }
        else if(_item.ammoType == AmmoType.Sniper)
        {
            _playerSniperMaxAmmo += 30;
        }
        else if(_item.ammoType == AmmoType.Total)
        {
            _playerRifleMaxAmmo += 30;
            _playerSniperMaxAmmo += 30;
        }
    }
}
