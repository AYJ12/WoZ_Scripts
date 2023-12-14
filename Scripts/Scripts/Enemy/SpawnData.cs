using UnityEngine;


[CreateAssetMenu(fileName = "SpawnData", menuName = "New SpawnPoint")]
public class SpawnData : ScriptableObject
{
    [Header("SpawnPoint")]
    public GameObject[] spawnObject;
}
