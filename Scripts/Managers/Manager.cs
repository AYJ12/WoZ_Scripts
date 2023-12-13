using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager instance = null;
    static Manager Instance
    {
        get
        {
            Init();
            return instance;
        }
    }

    ResourceManager resourceManager = new ResourceManager();
    UIManager uiManager = new UIManager();
    SoundManager soundManager = new SoundManager();
    PoolManager poolManager = new PoolManager();
    SceneManagerEx sceneManagerEx = new SceneManagerEx();
    SettingManager settingManager = new SettingManager();
    //private static LoadManager loadManager = new LoadManager();

    public static ResourceManager Resource { get { return Instance.resourceManager; } }
    public static UIManager UI { get { return Instance.uiManager; } }
    public static SoundManager Sound { get { return Instance.soundManager; } }
    public static PoolManager Pool { get { return Instance.poolManager; } }
    public static SceneManagerEx Scene { get { return Instance.sceneManagerEx; } }
    public static SettingManager Setting { get { return Instance.settingManager; } }
    //public static LoadManager LoadManager { get { Init(); return loadManager; } }
    private void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (instance == null)
        {
            GameObject go = GameObject.Find("Manager");
            if (go == null)
            {
                go = new GameObject { name = "Manager" };
                instance = ComponentEx.GetOrAddComponent<Manager>(go);
            }
            DontDestroyOnLoad(go);
            instance = go.GetComponent<Manager>();
            instance.InitManagers();
        }
    }

    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
    }

    private void InitManagers()
    {
        instance.resourceManager.Init();
        instance.uiManager.Init();
        instance.soundManager.Init();
        instance.poolManager.Init();
        instance.sceneManagerEx.Init();
        instance.settingManager.Init();
        //loadManager.Init();
    }
}