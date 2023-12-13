using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot dragSlot;

    [SerializeField]
    private Image _imageItem;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image itemimage)
    {
        _imageItem.sprite = itemimage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = _imageItem.color;
        color.a = _alpha;
        _imageItem.color = color;
    }
}
