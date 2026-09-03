using UnityEngine;

[ExecuteAlways]
public class FitQuadToCamera : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private float distance = 10f;

    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        Fit();
    }

    void Update()
    {

        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            Fit();
        }
    }

    private void Fit()
    {
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;

        transform.localPosition = new Vector3(0f, 0f, distance);
        transform.localRotation = Quaternion.identity;

        float heightWorld = 2f * distance * Mathf.Tan(targetCamera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float widthWorld = heightWorld * targetCamera.aspect;

        transform.localScale = new Vector3(widthWorld, heightWorld, 1f);
    }
}