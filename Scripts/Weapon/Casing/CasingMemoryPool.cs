using UnityEngine;

public class CasingMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject _casingPrefab; // ź�� ������Ʈ
    private MemoryPool _memoryPool; // ź�� memoryPool

    private void Awake()
    {
        _memoryPool = new MemoryPool(_casingPrefab);
    }
    public void SpawnCasing(Vector3 position, Vector3 direction) // ��ġ�� ���� �ޱ�
    {
        GameObject item = _memoryPool.ActivatePoolItem();
        item.transform.position = position; // ��ġ ����
        item.transform.rotation = Random.rotation; // ȸ�� ����
        item.GetComponent<Casing>().Setup(_memoryPool, direction);
    }
}
