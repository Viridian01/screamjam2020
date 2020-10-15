using UnityEngine;
using UnityEngine.UI;

public class CrunchyCamera : MonoBehaviour
{
    public Camera PlayerCam;
    RenderTexture PixelView;

    public GameObject RenderTarget;
    Vector2 res = Vector2.zero;

    public int TargetWidth = 320;
    public int TargetHeight = 180;

    // Start is called before the first frame update
    void Start()
    {
        int ViewWidth = Screen.width;
        int ViewHeight = Screen.height;

        res = new Vector2(ViewWidth, ViewHeight);

        float ScreenRatio = ViewWidth / ViewHeight;

        PixelView = new RenderTexture(TargetWidth, TargetHeight, 0, RenderTextureFormat.ARGB32);
        PixelView.filterMode = FilterMode.Point;
        PixelView.Create();
        PlayerCam.targetTexture = PixelView;

        RenderTarget.GetComponent<RawImage>().texture = PixelView;
        RenderTarget.GetComponent<RectTransform>().sizeDelta = res;
    }

    private void UpdateSize()
    {
        res = new Vector2(Screen.width, Screen.height);
        RenderTarget.GetComponent<RectTransform>().sizeDelta = res;
    }

    // Update is called once per frame
    void Update()
    {
        if (res != new Vector2(Screen.width, Screen.height))
        {
            UpdateSize();
        }
    }
}
