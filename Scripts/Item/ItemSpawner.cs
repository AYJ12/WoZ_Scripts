using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private ItemSpawnData _spawnData;
    private GameObject[] _spawnObjects;
    private List<GameObject> _poolList = new List<GameObject>();

    [SerializeField]
    private string[] _allPrefabsPath;

    // 아이템 프리팹이 들어있는 Resources 폴더 내의 하위 폴더명
    private string prefabFolderPath = "ItemInventory/ItemPrefabs/Item";

    private void OnEnable()
    {
        _spawnObjects = new GameObject[_spawnData.spawnObject.Length];
    }
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < _spawnData.spawnObject.Length; i++)
        {
            GameObject go = Manager.Resource.Instantiate(_spawnData.spawnObject[i], _spawnData.spawnObject[i].transform.position, _spawnData.spawnObject[i].transform.rotation);
            _spawnObjects[i] = go;
            go.transform.parent = transform;
        }
        SpawnItem();
    }

    private void SpawnItem()
    {
        for (int i = 0; i < _spawnObjects.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3 randomPosition = RandomPointInCircle(_spawnObjects[i].transform.position, _spawnObjects[i].transform.localScale.x * 5f);
                Quaternion randomQua = Quaternion.Euler(Quaternion.identity.x, Random.Range(0f, 360f), Quaternion.identity.z);
                //, randomPosition, randomQua);

                string path = GetRandomPrefab(_allPrefabsPath);
                GameObject go = Manager.Resource.Instantiate(path);
                _poolList.Add(go);
                go.transform.position = randomPosition;
                go.transform.rotation = randomQua;
                go.transform.parent = _spawnObjects[i].transform;
            }
        }
    }

    private void ReSpawnItem()
    {
        foreach (GameObject spawnGo in _spawnObjects)
        {
            if (spawnGo.transform.childCount != 5)
            {
                Vector3 randomPosition = RandomPointInCircle(spawnGo.transform.position, spawnGo.transform.localScale.x * 5f);
                Quaternion randomQua = Quaternion.Euler(Quaternion.identity.x, Random.Range(0f, 360f), Quaternion.identity.z);
                string path = GetRandomPrefab(_allPrefabsPath);
                GameObject go = Manager.Resource.Instantiate(path);

                _poolList.Add(go);
                go.transform.position = randomPosition;
                go.transform.rotation = randomQua;
                go.transform.parent = spawnGo.transform;
            }
        }
    }

    private Vector3 RandomPointInCircle(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 360f);
        float x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        float z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(x, center.y, z);
    }

    public void ObjectDestroyedSpawn()
    {
        _poolList.Remove(gameObject);
        Invoke("ReSpawnItem", 180f);
    }

    string GetRandomPrefab(string[] prefabs)
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogError("프리팹이 없거나 유효하지 않습니다.");
            return null;
        }

        // 랜덤한 인덱스 선택
        int randomIndex = Random.Range(0, prefabs.Length);

        // 랜덤한 인덱스에 해당하는 프리팹 반환
        return prefabs[randomIndex];
    }
}