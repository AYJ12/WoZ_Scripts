using UnityEngine;

public enum ImpactType
{
    Normal = 0, // ��, �ٴ�
    Obstacle, // ��ֹ�
    Enemy, // ��
}

public class ImpactMemoryPool : MonoBehaviour
{
    [SerializeField] private GameObject[] _impactPrefab; // �ǰ� impact
    [SerializeField] private GameObject _bloodPrefab;
    private MemoryPool[] _memoryPool;    // �ǰ� impact MemoryPool;

    private void Awake()
    {
        _memoryPool = new MemoryPool[_impactPrefab.Length];
        for (int i = 0; i < _impactPrefab.Length; ++i)
        {
            _memoryPool[i] = new MemoryPool(_impactPrefab[i]);
        }
    }

    public void SpawnImpact(RaycastHit hit)
    {
        int layer = hit.transform.gameObject.layer;
        // �浹�� ������Ʈ�� Tag �̸��� ���� ó��
        if (LayerMask.LayerToName(layer) == "ImpactNormal")
        {
            OnSpawnImpact(ImpactType.Normal, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (LayerMask.LayerToName(layer) == "ImpactObstacle")
        {
            OnSpawnImpact(ImpactType.Obstacle, hit.point, Quaternion.LookRotation(hit.normal));
        }
        else if (LayerMask.LayerToName(layer) == "Zombie")
        {
            OnSpawnBloodImpact(ImpactType.Enemy, hit.point, Quaternion.LookRotation(hit.normal));
            
        }
    }

    public void SpawnImpact(Collider other, Transform MeleeTransform)
    {
        int layer = other.gameObject.layer;

        if (LayerMask.LayerToName(layer) == "ImpactNormal")
        {
            OnSpawnImpact(ImpactType.Normal, MeleeTransform.position, Quaternion.Inverse(MeleeTransform.rotation));
        }
        else if (LayerMask.LayerToName(layer) == "ImpactObstacle")
        {
            OnSpawnImpact(ImpactType.Obstacle, MeleeTransform.position, Quaternion.Inverse(MeleeTransform.rotation));
        }
        else if (LayerMask.LayerToName(layer) == "Zombie")
        {
            OnSpawnBloodImpact(ImpactType.Enemy, MeleeTransform.position, Quaternion.Inverse(MeleeTransform.rotation));
        }
    }

    public void OnSpawnImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        GameObject item = _memoryPool[(int)type].ActivatePoolItem();
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.GetComponent<Impact>().Setup(_memoryPool[(int)type]);
    }

    public void OnSpawnBloodImpact(ImpactType type, Vector3 position, Quaternion rotation, Color color = new Color())
    {
        if (_bloodPrefab != null)
        {
            GameObject blood = Instantiate(_bloodPrefab, position, rotation);
        }
    }
}
