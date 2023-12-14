using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineEffect : MonoBehaviour
{
    public Color outlineColor = Color.white;
    public float outlineWidth = 0.02f;

    private Material outlineMaterial;
    private Material originalMaterial;
    private bool outlined = false;

    void Start()
    {
        outlineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        outlineMaterial.hideFlags = HideFlags.HideAndDontSave;
        outlineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        outlineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        outlineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        outlineMaterial.SetInt("_ZWrite", 0);

        originalMaterial = GetComponent<Renderer>().material;

        UpdateOutline(false);
    }

    public void UpdateOutline(bool outline)
    {
        if (outline)
        {
            ApplyOutline();
        }
        else
        {
            RemoveOutline();
        }

        outlined = outline;
    }

    void OnEnable()
    {
        UpdateOutline(outlined);
    }

    void OnDisable()
    {
        UpdateOutline(false);
    }

    void OnDestroy()
    {
        if (outlined)
        {
            RemoveOutline();
        }
        Destroy(outlineMaterial);
    }

    void ApplyOutline()
    {
        if (outlineMaterial != null)
        {
            Renderer renderer = GetComponent<Renderer>();

            renderer.material = outlineMaterial;

            outlineMaterial.SetColor("_Color", outlineColor);

            outlineMaterial.SetFloat("_Outline", outlineWidth);
        }
    }

    void RemoveOutline()
    {
        if (originalMaterial != null)
        {
            GetComponent<Renderer>().material = originalMaterial;
        }
    }
}
