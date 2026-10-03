using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{
    [SerializeField] private float aspectX = 4f;
    [SerializeField] private float aspectY = 3f;
    [SerializeField] private Color barColor = Color.black;
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        CreateBackgroundCamera();
    }

    void Update()
    {
        float targetAspect = aspectX / aspectY;
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scale = windowAspect / targetAspect;

        if (scale < 1f)
        {
            cam.rect = new Rect(0, (1f - scale) / 2f, 1f, scale);
        }
        else
        {
            float scaleWidth = 1f / scale;
            cam.rect = new Rect((1f - scaleWidth) / 2f, 0, scaleWidth, 1f);
        }
    }

    private void CreateBackgroundCamera()
    {
        GameObject backgroundObject = new GameObject("BackgroundCamera");
        Camera backgroundCam = backgroundObject.AddComponent<Camera>();

        backgroundCam.clearFlags = CameraClearFlags.SolidColor; // Background Type: Solid Color
        backgroundCam.backgroundColor = barColor;
        backgroundCam.cullingMask = 0;                          // Culling Mask: Nothing
        backgroundCam.rect = new Rect(0, 0, 1, 1);              // whole screen
        backgroundCam.depth = cam.depth - 1;                    // Priority: always below the main camera
    }
}
