using System.Collections.Generic;
using UnityEngine;

public class MemoryPool
{
    // MemoryPool로 관리되는 오브젝트 정보
    private class PoolItem
    {
        public bool isActive;   // 게임 오브젝트 활성화 여부 정보
        public GameObject gameObject;  // 실제 게임 오브젝트
    }

    private int _increaseCount = 5;  // 오브젝트 부족 시, Instantiate()로 추가 생성되는 오브젝트 개수
    private int _maxCount;           // 현재 리스트에 담긴 오브젝트 개수
    private int _activeCount;        // 현재 게임에 사용되고 있는 활성화 오브젝트 개수

    private GameObject _poolObject;  // 오브젝트 풀링에서 관리하는 게임 오브젝트 Prefab
    private List<PoolItem> _poolItemList;    // 관리되는 모든 오브젝트 저장 리스트

    public int MaxCount => _maxCount; // 외부에서 현재 리스트에 등록되어 있는 오브젝트 개수 확인을 위한 Property
    public int ActiveCount => _activeCount; // 외부에서 현재 활성화 되어 있는 오브젝트 개수 확인을 위한 Property

    public MemoryPool(GameObject poolObject)
    {
        _maxCount = 0;
        _activeCount = 0;
        this._poolObject = poolObject; // Prefab 호출하여 최초 탄알 5개 미리 생성

        _poolItemList = new List<PoolItem>();

        InstantiateObjects();
    }

    // _increaseCount 단위로 오브젝트 생성
    public void InstantiateObjects()
    {
        _maxCount += _increaseCount;

        for (int i = 0; i < _increaseCount; ++i)
        {
            PoolItem poolItem = new PoolItem();

            poolItem.isActive = false; // 보이지 않게
            poolItem.gameObject = GameObject.Instantiate(_poolObject); // Prefab 불러오기
            poolItem.gameObject.SetActive(false); // 

            _poolItemList.Add(poolItem); // 오브젝트 정보 저장
        }
    }

    /// <summary>
    /// 현재 관리중인 전체 오브젝트 삭제
    /// 씬 변경이거나 게임 종료 때, 한번만 수행하여 하이어라키에 생성된 모든 오브젝트 한번에 삭제
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

    // _poolItemList에 저장된 오브젝트 활성화하여 사용
    public GameObject ActivatePoolItem()
    {
        if (_poolItemList == null)
            return null;

        if (MaxCount == ActiveCount)  // 현재 모든 오브젝트가 게임 중 사용 중이면 
        {
            InstantiateObjects(); // 추가 생성
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

    // 현재 사용이 끝난 오브젝트를 비활성황 상태로 설정
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

    // 게임에 사용 중인 전체 오브젝트를 비활성화 상태로 설정
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
