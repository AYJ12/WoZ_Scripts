using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    [SerializeField]
    private WeaponSpawnData _weaponSpawnPositionData;
    private GameObject[] _weaponSpawnGameObject;
    [SerializeField]
    private GameObject[] _spawnWeaponObject;

    // Start is called before the first frame update
    void Start()
    {
        _weaponSpawnPositionData.spawnObject = Resources.LoadAll<GameObject>("Prefabs/Spawn/WeaponSpawnPoint");
        _weaponSpawnGameObject = new GameObject[_weaponSpawnPositionData.spawnObject.Length];

        for (int i = 0; i < _weaponSpawnGameObject.Length; i++)
        {
            GameObject go = Manager.Resource.Instantiate(_weaponSpawnPositionData.spawnObject[i], _weaponSpawnPositionData.spawnObject[i].transform.position, _weaponSpawnPositionData.spawnObject[i].transform.rotation);
            _weaponSpawnGameObject[i] = go;
            go.transform.parent = transform;
            int rnd = Random.Range(0, _spawnWeaponObject.Length);
            Manager.Resource.Instantiate(_spawnWeaponObject[rnd], go.transform.position, _spawnWeaponObject[rnd].transform.rotation);
        }
    }

}
