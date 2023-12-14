using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpawnData", menuName = "New WeaponSpawnPoint")]
public class WeaponSpawnData : ScriptableObject
{
    [Header("SpawnPoint")]
    public GameObject[] spawnObject;
}
