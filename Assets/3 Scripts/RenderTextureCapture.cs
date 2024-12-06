using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderTextureCapture : MonoBehaviour
{
    public RenderTexture ExportCamera;

    public void ExportPhoto()
    {
        byte[] bytes = toTexture2D(ExportCamera).EncodeToPNG();
        var dirPath = Application.persistentDataPath + "/ExportPhoto";
        if(!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }
        System.IO.File.WriteAllBytes(dirPath + "/Photo_" + Random.Range(0, 100000) + ".png", bytes);
        Debug.Log(bytes.Length / 1024 + "kb was saved as: " + dirPath);
    }

    Texture2D toTexture2D(RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(1920,1080,TextureFormat.RGB24,false);
        RenderTexture.active = rTex;
        tex.ReadPixels(new Rect(0,0,rTex.width,rTex.height),0,0);
        tex.Apply();
        return tex;
    }

}
