using UnityEngine;


[RequireComponent(typeof(Camera))]
public class DitheringEffect : MonoBehaviour
{

    Material baseMaterial;

    [SerializeField]
    private Texture2D ditherTex;

    [SerializeField]
    private Texture2D rampTex;

    [SerializeField]
    private bool useScrolling = false;

    [SerializeField]
    private FilterMode filterMode = FilterMode.Bilinear;

    public void Start()
    {

        baseMaterial = new Material(Resources.Load<Shader>("Shaders/Dithering"));
        baseMaterial.SetTexture("_NoiseTex", ditherTex);
        baseMaterial.SetTexture("_ColorRampTex", rampTex);
    }

    public void Render(RenderTexture src, RenderTexture dst)
    {
        var xOffset = 0.0f;
        var yOffset = 0.0f;

        if (useScrolling)
        {
            var camEuler = Camera.main.transform.eulerAngles;
            xOffset = 4.0f * camEuler.y / Camera.main.fieldOfView;
            yOffset = -2.0f * Camera.main.aspect * camEuler.x / Camera.main.fieldOfView;
        }

        baseMaterial.SetFloat("_XOffset", xOffset);
        baseMaterial.SetFloat("_YOffset", yOffset);
        RenderTexture super = RenderTexture.GetTemporary(src.width * 2, src.height * 2);
        RenderTexture half = RenderTexture.GetTemporary(src.width / 2, src.height / 2);

        super.filterMode = filterMode;
        half.filterMode = filterMode;

        Graphics.Blit(src, super);
        Graphics.Blit(super, half, baseMaterial);
        Graphics.Blit(half, dst);

        RenderTexture.ReleaseTemporary(half);
        RenderTexture.ReleaseTemporary(super);
        Debug.Log("bruh");

    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Render(src, dst);
    }
}
