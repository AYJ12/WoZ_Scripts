using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject _casingPrefab; // 탄피 오브젝트
    private MemoryPool _memoryPool; // 탄피 memoryPool

    private void Awake()
    {
        _memoryPool = new MemoryPool(_casingPrefab);
    }
    public void SpawnCasing(Vector3 position, Vector3 direction) // 위치와 방향 받기
    {
        GameObject item = _memoryPool.ActivatePoolItem();
        item.transform.position = position; // 위치 설정
        item.transform.rotation = Random.rotation; // 회전 설정
        item.GetComponent<Casing>().Setup(_memoryPool, direction);
    }
}
