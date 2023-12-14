using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private SpawnData _spawnData;
    private GameObject[] _spawnObjects;
    private List<GameObject> poolList = new List<GameObject>();
    [SerializeField]
    private GameObject[] _randomZombie;
    private string _path;

    private void OnEnable()
    {
        _spawnData.spawnObject = Resources.LoadAll<GameObject>("Prefabs/Spawn/EnemySpawnPoint");
        _path = "Enemy/";
        _spawnObjects = new GameObject[_spawnData.spawnObject.Length];
    }

    private void FixedUpdate()
    {
        foreach (GameObject obj in _spawnObjects)
        {
            Vector3 disVec = (obj.transform.position - Player.Instance.transform.position);
            float distance = Vector3.SqrMagnitude(disVec);
            if (distance >= 100f * 100f)
                obj.SetActive(false);
            else
                obj.SetActive(true);
        }
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
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        for (int i = 0; i < _spawnObjects.Length; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Vector3 randomPosition = RandomPointInCircle(_spawnObjects[i].transform.position, _spawnObjects[i].transform.localScale.x * 5f);
                Quaternion randomQua = Quaternion.Euler(Quaternion.identity.x, Random.Range(0f, 360f), Quaternion.identity.z);
                //, randomPosition, randomQua);
                int rnd = Random.Range(0, _randomZombie.Length);
                GameObject go = Manager.Resource.Instantiate($"{ _path}{_randomZombie[rnd].gameObject.name}");
                poolList.Add(go);
                go.transform.position = randomPosition;
                go.transform.rotation = randomQua;
                go.transform.parent = _spawnObjects[i].transform;
            }
        }
    }
    
    private void ReSpawnEnemy()
    {
        foreach(GameObject spawnGo in _spawnObjects)
        {
            if(spawnGo.transform.childCount != 5)
            {
                Vector3 randomPosition = RandomPointInCircle(spawnGo.transform.position, spawnGo.transform.localScale.x * 5f);
                Quaternion randomQua = Quaternion.Euler(Quaternion.identity.x, Random.Range(0f, 360f), Quaternion.identity.z);
                int rnd = Random.Range(0, _randomZombie.Length);
                GameObject go = Manager.Resource.Instantiate(_randomZombie[rnd].name);
                poolList.Add(go);
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
        poolList.Remove(gameObject);
        Invoke("ReSpawnEnemy", 3f);
    }


    //private IEnumerator Spawn()
    //{

    //    while (Input.GetKeyDown("0"))    // ����
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //    }
    //    yield return null;
    //}
}
