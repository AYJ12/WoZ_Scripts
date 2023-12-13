using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    [SerializeField]
    private SpawnData _spawnData;
    [SerializeField]
    private GameObject _playerSpawnPoint;
    private GameObject[] _npcSpawnPoint;  // npc ������Ʈ �������� �ڷᱸ�� ���
    public GameObject weaponDrop;
    private List<GameObject> _weaponSlot;


    public override void Init()
    {
        base.Init();
        
        SceneType = Define.Scene.GameScene;

        //QualitySettings.renderPipeline = SettingManager.Instance.curRenderPipelinAssets;
        //Screen.SetResolution(PlayerPrefs.GetInt("ScreenWidth"), PlayerPrefs.GetInt("ScreenHeight"), SettingManager.Instance.fullScreenMode);

        _npcSpawnPoint = _spawnData.spawnObject;

        InstantiateObjects();
        InstantiateUIObjects();
    }


    void InstantiateUIObjects()
    {
        //UI����
        Manager.UI.ShowSceneUI<UI_GameScene>();
    }

    void InstantiateObjects()
    {
        Manager.Resource.Instantiate("Map/GameMap");
        Manager.Resource.Instantiate("Player/Player", _playerSpawnPoint.transform.position, _playerSpawnPoint.transform.rotation);  // ���� ��ġ ����
        int rnd = Random.Range(0, _npcSpawnPoint.Length);
        Manager.Resource.Instantiate("Npc/Npc", _npcSpawnPoint[rnd].transform.position, _npcSpawnPoint[rnd].transform.rotation);
        weaponDrop = Manager.Resource.Instantiate("Spawn/WeaponSpawner");
        Manager.Resource.Instantiate("Spawn/EnemySpawner");
        Manager.Resource.Instantiate("Spawn/ItemSpawner");
    }

    public override void Clear()
    {
        // ���� ����� �� �ʿ��� ���� ������ �ۼ�
    }
}
