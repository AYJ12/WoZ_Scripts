using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField]
    private float _sensitivity = 300f; //���� ����
    float rotationX = 0.0f;  //x�� ȸ����
    float rotationY = 0.0f;  //z�� ȸ����
    private PlayerEventController _playerEventController;

    private void Start()
    {
        _playerEventController = Player.Instance.playerEvent;
        _playerEventController.OnZoomEvent += MouseSensitivity;
        _playerEventController.OnSwitchEvent += SwitchMouseSen;
        _sensitivity = Manager.Setting.idleSensitivity;
    }

    void Update()
    {
        if (!Inventory.inventoryActivated && Player.Instance.playerInput.canLook)
        {
            MouseSencer();
        }
    }

    void MouseSencer()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        rotationX += x * _sensitivity * Time.deltaTime;
        rotationY += y * _sensitivity * Time.deltaTime;

        if (rotationY > 90)
        {
            rotationY = 90;
        }
        else if (rotationY < -90)
        {
            rotationY = -90;
        }
        transform.eulerAngles = new Vector3(-rotationY, rotationX, 0.0f);
    }

    private void SwitchMouseSen(int i)
    {
        MouseSensitivity();
    }

    private void MouseSensitivity()
    {
        if (Player.Instance.PossibleWeaponZoom() && Manager.Setting != null)
            _sensitivity = (Player.Instance.playerStatHandler.playerLookType == PlayerLookType.Idle ?
            Manager.Setting.idleSensitivity : Manager.Setting.zoomSensitivity);
        else
            _sensitivity = (Player.Instance.playerStatHandler.playerLookType == PlayerLookType.Idle ?
                300f : 100f);
    }
}


