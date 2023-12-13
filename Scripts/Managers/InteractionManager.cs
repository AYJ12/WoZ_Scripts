using TMPro;
using UnityEngine;
using UnityEngine.UI;

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}

public class InteractionManager : MonoBehaviour
{
    public float checkRate = 0.02f;
    private float _lastCheckTime;
    public float maxCheckDistance = 3.0f;
    public LayerMask layerMask;

    private GameObject _curInteractGameobject;
    private IInteractable _curInteractable;

    private UI_GameScene _uiGameScene;
    private PlayerEventController _playerEventController;
    private TextMeshProUGUI _promptText;
    [SerializeField] private Image _promptBG;
    private Camera _camera;

    private void Awake()
    {
        _playerEventController = GetComponent<PlayerEventController>();
    }

    private void Start()
    {
        _camera = Camera.main;
        _playerEventController.OnInteractEvent += OnInteractInput;
        _uiGameScene = Manager.UI.SceneUI.GetComponent<UI_GameScene>();
        _promptText = _uiGameScene.GetTextMesh((int)UI_GameScene.TextMeshs.Prompt);
        _promptBG = _uiGameScene.GetImage((int)UI_GameScene.Images.PromptBG);
    }

    private void Update()
    {
        CheckInteract();
    }

    private void CheckInteract()
    {
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = new Ray(_camera.transform.position, transform.forward);//(new Vector3(Screen.width / 2, Screen.height / 2) + (Vector3.forward * 3));
            Debug.DrawRay(_camera.transform.position, transform.forward * 10, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != _curInteractGameobject)
                {
                    DisableOutline();

                    _curInteractGameobject = hit.collider.gameObject;
                    if ((_curInteractable = hit.collider.GetComponent<IInteractable>()) != null)
                    {
                        if (hit.collider.gameObject == NpcObject.Instance.gameObject)
                        {
                            NpcInteractAI npc = _curInteractGameobject.GetComponent<NpcInteractAI>();
                            if (!npc.isActive)
                            {
                                SetPromptText();
                            }

                        }
                        else if (hit.collider.CompareTag("Weapon"))
                        {
                            _curInteractable = hit.collider.GetComponent<IInteractable>();
                            SetPromptText();
                            EnableOutline();
                        }
                        //else if (hit.collider.tag == "Item")
                        //{

                        //}
                    }
                }
            }
            else
            {
                DisableOutline();
                _curInteractGameobject = null;
                _curInteractable = null;
                _promptText.gameObject.SetActive(false);
                _promptBG.gameObject.SetActive(false);
            }
        }
    }
    private void SetPromptText()
    {
        _promptBG.gameObject.SetActive(true);
        _promptText.gameObject.SetActive(true);
        _promptText.text = string.Format("{0}", _curInteractable.GetInteractPrompt());
    }


    public void OnInteractInput()
    {
        if (_curInteractable != null)
        {
            _curInteractable.OnInteract();
            _curInteractGameobject = null;
            _curInteractable = null;
            _promptText.gameObject.SetActive(false);
        }
    }


    private void EnableOutline() // Outline on
    {
        Outline outline = _curInteractGameobject.GetComponent<Outline>();
        if (outline != null)
        {
            outline.enabled = true;
        }
    }
    private void DisableOutline() // Outline off
    {
        if (_curInteractGameobject != null && _curInteractGameobject.CompareTag("Weapon"))
        {
            Outline outline = _curInteractGameobject.GetComponent<Outline>();
            if (outline != null)
            {
                outline.enabled = false;
            }
        }
    }

}
