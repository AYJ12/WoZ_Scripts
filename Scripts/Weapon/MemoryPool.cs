using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    // MemoryPool�� �����Ǵ� ������Ʈ ����
    private class PoolItem
    {
        public bool isActive;   // ���� ������Ʈ Ȱ��ȭ ���� ����
        public GameObject gameObject;  // ���� ���� ������Ʈ
    }

    private int _increaseCount = 5;  // ������Ʈ ���� ��, Instantiate()�� �߰� �����Ǵ� ������Ʈ ����
    private int _maxCount;           // ���� ����Ʈ�� ��� ������Ʈ ����
    private int _activeCount;        // ���� ���ӿ� ���ǰ� �ִ� Ȱ��ȭ ������Ʈ ����

    private GameObject _poolObject;  // ������Ʈ Ǯ������ �����ϴ� ���� ������Ʈ Prefab
    private List<PoolItem> _poolItemList;    // �����Ǵ� ��� ������Ʈ ���� ����Ʈ

    public int MaxCount => _maxCount; // �ܺο��� ���� ����Ʈ�� ��ϵǾ� �ִ� ������Ʈ ���� Ȯ���� ���� Property
    public int ActiveCount => _activeCount; // �ܺο��� ���� Ȱ��ȭ �Ǿ� �ִ� ������Ʈ ���� Ȯ���� ���� Property

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        this._poolObject = poolObject; // Prefab ȣ���Ͽ� ���� ź�� 5�� �̸� ����

        _poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    // _increaseCount ������ ������Ʈ ����
    public void InstantiateObjects()
    {
        _maxCount += _increaseCount;

        for (int i = 0; i < _increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false; // ������ �ʰ�
            poolItem.gameObject = GameObject.Instantiate(_poolObject); // Prefab �ҷ�����
            poolItem.gameObject.SetActive(false); // 

            _poolItemList.Add(poolItem); // ������Ʈ ���� ����
        }
    }

    /// <summary>
    /// ���� �������� ��ü ������Ʈ ����
    /// �� �����̰ų� ���� ���� ��, �ѹ��� �����Ͽ� ���̾��Ű�� ������ ��� ������Ʈ �ѹ��� ����
    /// </summary>
    public void DestoryObjects()
    {
        if (_poolItemList == null)
            return;
        int count = _poolItemList.Count;
        for (int i = 0; i < count; i++)
        {
            GameObject.Destroy(_poolItemList[i].gameObject);
        }

        _poolItemList.Clear();
    }

    // _poolItemList�� ����� ������Ʈ Ȱ��ȭ�Ͽ� ���
    public GameObject ActivatePoolItem()
    {
        if (_poolItemList == null)
            return null;

        if (MaxCount == ActiveCount)  // ���� ��� ������Ʈ�� ���� �� ��� ���̸� 
        {
            InstantiateObjects(); // �߰� ����
        }

        int count = _poolItemList.Count;

        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = _poolItemList[i];

            if (poolItem.isActive == false)
            {
                _activeCount++;

                poolItem.isActive = true;
                poolItem.gameObject.SetActive(true);

                return poolItem.gameObject;
            }
        }
        return null;
    }

    // ���� ����� ���� ������Ʈ�� ��Ȱ��Ȳ ���·� ����
    public void DeactivatePoolItem(GameObject removeObject)
    {
        if (_poolItemList == null || removeObject == null)
            return;

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem pooIItem = _poolItemList[i];

            if (pooIItem.gameObject == removeObject)
            {
                _activeCount--;

                pooIItem.isActive = false;
                pooIItem.gameObject.SetActive(false);

                return;
            }
        }
    }

    // ���ӿ� ��� ���� ��ü ������Ʈ�� ��Ȱ��ȭ ���·� ����
    public void DeactivateAllPoolItems()
    {
        if (_poolItemList == null)
            return;

        int count = _poolItemList.Count;
        for (int i = 0; i < count; ++i)
        {
            PoolItem poolItem = _poolItemList[i];

            if (poolItem.gameObject != null && poolItem.isActive == true)
            {
                poolItem.isActive = false;
                poolItem.gameObject.SetActive(false);
            }
        }
        _activeCount = 0;
    }
}
