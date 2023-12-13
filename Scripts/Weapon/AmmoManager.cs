using System.Collections.Generic;
using UnityEngine;

public class AmmoManager : MonoBehaviour
{
    [SerializeField] private int _rifleMaxAmmo;
    [SerializeField] private int _sniperMaxAmmo;

    private Dictionary<WeaponType, int> ammoDictionary = new Dictionary<WeaponType, int>();
    public static AmmoManager instance { get; private set; }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); // Scene ��ü ����
        }
    }

    // 라이풀 최대 탄약 수 받기
    public int GetRifleMaxAmmo()
    {
        return _rifleMaxAmmo;
    }
    // 라이풀 최대 탄약 수 전달
    public void SetRifleMaxAmmo(int maxAmmo)
    {
        _rifleMaxAmmo = maxAmmo;
    }

    // 스나이퍼 최대 탄약 받기
    public int GetSniperMaxAmmo()
    {
        return _sniperMaxAmmo;
    }
    // 스나이퍼 최대 탄약 전달�
    public void SetSniperMaxAmmo(int maxAmmo)
    {
        _sniperMaxAmmo = maxAmmo;
    }


    // 현재 탄약 수 값 업데이트
    public void UpdateAmmo(WeaponType weaponType, int currentAmmo)
    {
        if (ammoDictionary.ContainsKey(weaponType))
        {
            ammoDictionary[weaponType] = currentAmmo;
        }
        else
        {
            ammoDictionary.Add(weaponType, currentAmmo);
        }
    }

    // 현재 탄약 수 값 받기
    public int GetAmmoCount(WeaponType weaponType)
    {
        if (ammoDictionary.TryGetValue(weaponType, out int ammoCount))
        {
            return ammoCount;
        }
        else
        {
            return 0;
        }
    }
}
