using UnityEngine;

public class BlurManager : MonoBehaviour
{
    public Camera blurCamera;
    public RenderTexture blurTexture;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        blurTexture = new RenderTexture(Screen.width, Screen.height, 16);
        blurCamera.targetTexture = blurTexture;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
