using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; private set; }
        private Stack<Poolable> _poolStack = new Stack<Poolable>();

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{ Original.name}_Root";

            for(int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            return ComponentEx.GetOrAddComponent<Poolable>(go);
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if(_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            if(parent == null)
            {
                poolable.transform.parent = Manager.Scene.CurrentScene.transform;
            }

            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }


    Dictionary<string, Pool> _pools = new Dictionary<string, Pool>();
    private Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject original, int count = 5)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = _root;

        _pools.Add(original.name, pool);

        //if (_prefabs == null)
        //    _prefabs = Manager.Resource.Load<GameObject>(_prefabs.name);
        //for (int i = 0; i < poolSize; i++)
        //{
        //    GameObject go = Manager.Resource.Instantiate(_prefabs.name);
        //    _prefabs.SetActive(false);
        //    _objectPool.Enqueue(go);
        //}
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;
        if(_pools.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (_pools.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }
        return _pools[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name)
    {
        if(_pools.ContainsKey(name) == false)
            return null;
        return _pools[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pools.Clear();
    }

    //public void SetPrefabs(string path)
    //{
    //    _prefabs = Manager.Resource.Load<GameObject>(path);
    //}

    //public GameObject SpawnPool(Vector3 position, Quaternion rotation)
    //{
    //    // Ǯ���� ��Ȱ��ȭ�� ������Ʈ ã�Ƽ� Ȱ��ȭ �� ��ġ �� ȸ�� ����

    //    GameObject go = null;
    //    if (_objectPool.Count == 0)
    //    {
    //        CreatePool();
    //    }
    //    foreach (GameObject obj in _objectPool)
    //    {
    //        if (!obj.activeSelf)
    //        {
    //            go = obj;
    //            obj.SetActive(true);
    //            obj.transform.position = position;
    //            obj.transform.rotation = rotation;
    //            _objectPool.Dequeue();

    //            return go;
    //        }
    //    }

    //    return go; // Ǯ���� ��� ������ ������Ʈ�� ã�� ���� ���
    //}

    //public void ReturnToPool(GameObject obj)
    //{
    //    // ������Ʈ�� Ǯ�� �ٽ� ��ȯ�ϰ� ��Ȱ��ȭ
    //    obj.SetActive(false);
    //}

    //class Pool
    //{
    //    public GameObject oringinObject;
    //    public Transform objectRoot;
    //    public string objectPath;

    //    Queue<Poolable> _poolStack = new Queue<Poolable>();

    //    public void Init(GameObject original, string path, int count = 5)
    //    {
    //        oringinObject = original;
    //        objectRoot = new GameObject().transform;
    //        objectPath = path;
    //        objectRoot.name = $"{oringinObject.name}Root";

    //        for (int i = 0; i < count; i++)
    //        {
    //            Push(Create());
    //        }
    //    }

    //    Poolable Create()
    //    {
    //        GameObject go = GameManager.Resource.Load<GameObject>($"Prefabs/{objectPath}");
    //        go.SetActive(false);
    //        go = Object.Instantiate(go);
    //        go.name = oringinObject.name;
    //        return go.GetOrAddComponent<Poolable>();
    //    }

    //    public void Push(Poolable poolable)
    //    {
    //        if (poolable == null)
    //            return;

    //        poolable.transform.SetParent(objectRoot);
    //        poolable.gameObject.SetActive(false);

    //        _poolStack.Enqueue(poolable);
    //    }

    //    public Poolable Pop(Vector3 pos, Quaternion q, Transform parent)
    //    {
    //        Poolable poolable;

    //        if (_poolStack.Count > 0)
    //            poolable = _poolStack.Dequeue();
    //        else
    //            poolable = Create();

    //        if (parent == null)
    //            if(SceneManager.GetActiveScene().name == "GameScene")
    //            //poolable.transform.SetParent();


    //        poolable.transform.SetParent(parent);
    //        poolable.transform.position = pos;
    //        poolable.transform.rotation = q;
    //        poolable.gameObject.SetActive(true);

    //        return poolable;
    //    }
    //}

    //Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    //public Transform _root;
    //public void Init()
    //{
    //    if (_root == null)
    //    {
    //        _root = new GameObject { name = "PoolRoot" }.transform;
    //        Object.DontDestroyOnLoad(_root);
    //    }
    //}
    ////public GameObject objectPrefab; // ������ ������Ʈ�� ������
    ////public int poolSize = 10;        // Ǯ ũ��

    ////private List<GameObject> objectPool = new List<GameObject>();

    //public void CreatePool(GameObject original, string path, int count = 5)
    //{
    //    Pool pool = new Pool();
    //    pool.Init(original, path, count);
    //    pool.objectRoot.parent = _root;

    //    _pool.Add(original.name, pool);
    //}

    //public void Push(Poolable poolable)
    //{
    //    string name = poolable.gameObject.name;
    //    if (_pool.ContainsKey(name) == false)
    //    {
    //        GameObject.Destroy(poolable.gameObject);
    //        return;
    //    }

    //    _pool[name].Push(poolable);
    //}

    //public Poolable Pop(GameObject original, string path, Vector3 pos, Quaternion q, Transform parent)
    //{
    //    if (_pool.ContainsKey(original.name) == false)
    //        CreatePool(original, path);

    //    return _pool[original.name].Pop(pos, q, parent);
    //}
    //public void Clear()
    //{
    //    foreach (Transform child in _root)
    //        GameObject.Destroy(child.gameObject);

    //    _pool.Clear();
    //}
    //public GameObject SpawnFromPool(Vector3 position, Quaternion rotation)
    //{
    //    // Ǯ���� ��Ȱ��ȭ�� ������Ʈ ã�Ƽ� Ȱ��ȭ �� ��ġ �� ȸ�� ����
    //    for (int i = 0; i < objectPool.Count; i++)
    //    {
    //        if (!objectPool[i].activeInHierarchy)
    //        {
    //            objectPool[i].SetActive(true);
    //            objectPool[i].transform.position = position;
    //            objectPool[i].transform.rotation = rotation;
    //            return objectPool[i];
    //        }
    //    }
    //    return null; // Ǯ���� ��� ������ ������Ʈ�� ã�� ���� ���
    //}

}
