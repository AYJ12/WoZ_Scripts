using TMPro;
using UnityEngine;

public interface WeaponInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class WeaponInteractionManager : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float _lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject _curInteractGameobject;
    private WeaponInteractable _curInteractable;

    private PlayerEventController _playerEvent;
    //private Outline _outline;

    public TextMeshProUGUI promptText;
    private Camera _camera;

    private void Awake()
    {
        _playerEvent = GetComponent<PlayerEventController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _playerEvent.OnInteractEvent += OnInteract;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != _curInteractGameobject)
                {
                    Debug.Log("weaponInteractabel");
                    DisableOutline();
                    _curInteractGameobject = hit.collider.gameObject;
                    _curInteractable = hit.collider.GetComponent<WeaponInteractable>();
                    SetPromptText();
                    EnableOutline();
                }
            }
            else
            {
                DisableOutline();
                _curInteractGameobject = null;
                _curInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }
    private void EnableOutline() // on
    {
        Outline outline = _curInteractGameobject.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
    private void DisableOutline() // off
    {
        if (_curInteractGameobject != null)
        {
            Outline outline = _curInteractGameobject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", _curInteractable.GetInteractPrompt());
    }

    public void OnInteract()
    {
        if (_curInteractable != null)
        {
            _curInteractable.OnInteract();
            _curInteractGameobject = null;
            _curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }

}
