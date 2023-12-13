using UnityEngine;


[CreateAssetMenu(fileName = "ItemSpawnData", menuName = "Item SpawnPoint")]
public class ItemSpawnData : ScriptableObject
{
    [Header("ItemSpawnPoint")]
    public GameObject[] spawnObject;
}