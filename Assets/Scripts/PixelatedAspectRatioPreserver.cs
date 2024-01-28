using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelatedAspectRatioPreserver : MonoBehaviour
{
    [NotNull]
    public RenderTexture renderTexture;
    public Camera cam;
    public RawImage rawImage;
    private int width = -1;
    private int height = -1;

    private RenderTexture runtimeRenderTexture;
    void Update()
    {
        if (Screen.width != width || Screen.height != height)
        {
            //Debug.Log("Screen size changed.");
            width = Screen.width;
            height = Screen.height;
            float aspectRatio = (float)width / height;

            /*            renderTexture.Release();
                        renderTexture.width = (int)(renderTexture.height * aspectRatio);
                        renderTexture.Create();*/
            /*
            if (runtimeRenderTexture == null)
            {
                runtimeRenderTexture = new RenderTexture((int)(renderTexture.height * aspectRatio), renderTexture.height, 24);
                runtimeRenderTexture.name = $"{renderTexture.name} (clone)";
                cam.targetTexture = runtimeRenderTexture;
                rawImage.texture = runtimeRenderTexture;
            }
            else
            {
                runtimeRenderTexture.Release();
                runtimeRenderTexture.width = (int)(renderTexture.height * aspectRatio);
                runtimeRenderTexture.Create();
                cam.targetTexture = runtimeRenderTexture;
                rawImage.texture = runtimeRenderTexture;
            }*/
            if (runtimeRenderTexture != null)
            {
                runtimeRenderTexture.Release();
                Destroy(runtimeRenderTexture);
            }

            runtimeRenderTexture = new RenderTexture((int)(renderTexture.height * aspectRatio), renderTexture.height, 24)
            {
                name = $"{renderTexture.name} (clone)",
                filterMode = renderTexture.filterMode
            };
            cam.targetTexture = runtimeRenderTexture;
            //mat.SetTexture("_MainTex", runtimeRenderTexture);
            rawImage.texture = runtimeRenderTexture;
        }
    }
}
