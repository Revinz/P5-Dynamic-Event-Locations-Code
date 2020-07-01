using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPath : MonoBehaviour {

    public Texture2D DrawPath() {

        FileInfo playerPath = new FileInfo("Assets/playerPos.txt");
        StreamReader reader = playerPath.OpenText();
        Texture2D texture = new Texture2D(1000, 1000);
        texture = TextureToTransparent(texture);

        string line = null;

        Color[] col = new Color[4];

        for (int i = 0; i < col.Length; i++) {
            col[i] = Color.yellow;
        }

        while ((line = reader.ReadLine()) != null) { //Read the next line first then check if there was a line that got read
            string[] splitLine = line.Split(' ');
            Debug.Log(splitLine[3]);
            float x = float.Parse(splitLine[4]); //You are trying to read a float, not an int. Therefor float.parse should be used.
            float z = float.Parse(splitLine[6]);
            texture.SetPixels((int)x, (int)z, 2, 2, col); //Convert the float to an int in the method

        }

        texture.Apply();

        SaveTextureToFile(texture, "PlayerPath.png");

        return texture;
    }

    private void SaveTextureToFile(Texture2D texture, string fileName) {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/OutputImages/" + fileName, bytes);
        Debug.Log("Texture saved");
        /*FileStream file = File.Open(Application.dataPath + "" + fileName, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();*/
    }

    private Texture2D TextureToTransparent(Texture2D tex) {

        Color colour = new Color(0, 0, 0, 0);
        Color[] colourArray = tex.GetPixels();

        for (int i = 0; i < colourArray.Length; i++) {
            colourArray[i] = colour;
        }

        tex.SetPixels(colourArray);

        return tex;
    }
}
