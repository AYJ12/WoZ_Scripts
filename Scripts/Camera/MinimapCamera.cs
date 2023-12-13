using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    private Camera _miniCamera;
    [SerializeField]
    private LayerMask layersMask;

    void Start()
    {
        _miniCamera = GetComponent<Camera>();
        //int playerLayerIndex = LayerMask.GetMask("Player");
        //int enemyLayerIndex = LayerMask.GetMask("Zombie");
        //int impactObstacleLayerIndex = LayerMask.GetMask("ImpactObstacle");
        //int impactNormalLayerIndex = LayerMask.GetMask("ImpactNormal");
        _miniCamera.cullingMask &= ~(layersMask);
    }
}
