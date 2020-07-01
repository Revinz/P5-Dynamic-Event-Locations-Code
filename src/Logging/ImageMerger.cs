using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ImageMerger : MonoBehaviour
{
    public Texture2D botImage;
    public Texture2D topImage;
    public string Path = "/OutputImages/";

    /// <summary>
    /// Merges the two image textures together
    /// </summary>
    /// <param name="_botImg">The bottom image</param>
    /// <param name="_topImg">The top image being added on top of the bottom image</param>
    public static void MergeImages(Texture2D _botImg, Texture2D _topImg) {
        ImageMerger merger = new ImageMerger();
        merger.MergeTextures(_botImg, _topImg);
    }

    public void MergeTextures(Texture2D _botImg, Texture2D _topImg) {
        Texture2D mergedImage = new Texture2D(_botImg.width, _botImg.height, TextureFormat.RGBA32, false);
        Color[] mergedColors = mergedImage.GetPixels();
        Color[] bg = _botImg.GetPixels();
        Color[] top = _topImg.GetPixels();

        for (var i = 0; i < bg.Length; ++i)
        {
            if (top[i].a == 0)
                mergedColors[i] = bg[i];
            else
                mergedColors[i] = top[i];
        }

        mergedImage.SetPixels(mergedColors);
        mergedImage.Apply();

        byte[] bytes = mergedImage.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + Path + "TestMergeImage.png", bytes);

    }

    /*private void Start() {
        MergeImages(botImage, topImage);
    }*/
}
