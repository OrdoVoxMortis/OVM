using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class OutlineController : MonoBehaviour
{
    [SerializeField] private Material outlineMaterial;
    
    [Header("Outline Settings")]
    public Color outlineColor = Color.black;
    public float outlineWidth = 0.01f;

    [Header("Outline Control")]
    public bool outlineEnabled = true;

    [Header("Animation Settings")]
    public bool animateWidth = false;
    public float widthSpeed = 1.0f;
    public bool animateColor = false;
    public float colorSpeed = 1.0f;

    void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            outlineMaterial = renderer.material;
            ApplySettings();
            outlineMaterial.SetFloat("_OutlineWidth", outlineEnabled ? outlineWidth : 0f);
        }
    }

    void Update()
    {
        if (outlineMaterial == null) return;

        // 애니메이션이나 수동 조정이 있을 경우 매 프레임 적용
        ApplySettings();
    }
    
    public void ApplySettings()
    {
        if (outlineMaterial == null)
            return;
        
        outlineMaterial.SetColor("_OutlineColor", outlineColor);
        outlineMaterial.SetFloat("_OutlineWidth", outlineWidth);
        outlineMaterial.SetFloat("_AnimateWidth", animateWidth ? 1.0f : 0.0f);
        outlineMaterial.SetFloat("_WidthSpeed", widthSpeed);
        outlineMaterial.SetFloat("_AnimateColor", animateColor ? 1.0f : 0.0f);
        outlineMaterial.SetFloat("_ColorSpeed", colorSpeed);
    }

    public void ToggleOutline(bool enabled)
    {
        outlineEnabled = enabled;
        if (outlineMaterial != null)
        {
            outlineMaterial.SetFloat("_OutlineWidth", enabled ? outlineWidth : 0f);
        }
    }
}