using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapOutput : MonoBehaviour {

    public Texture2D DrawMap() {

        Texture2D texture = new Texture2D(1000, 1000);

        texture = TextureToBlack(texture);

        bool dynamicScene = false;

        if (SceneManager.GetActiveScene().name.ToLower().Contains("dynamic"))
            dynamicScene = true;

        List<GameObject> events = CreateEventList(dynamicScene);

        int eventAmount = events.Count;

        Color[] col = new Color[25];

        for (int i = 0; i < col.Length; i++) {
            col[i] = Color.white;
        }

        foreach (GameObject obj in events) {
            texture = DrawCircle(texture, Color.blue, (int)obj.transform.position.x, (int)obj.transform.position.z, 70);
            texture = DrawCircle(texture, Color.magenta, (int)obj.transform.position.x, (int)obj.transform.position.z, 60);
            texture = DrawCircle(texture, Color.red, (int)obj.transform.position.x, (int)obj.transform.position.z, 30);
            texture.SetPixels((int)obj.transform.position.x, (int)obj.transform.position.z, 5, 5, col);
        }

        for (int i = 5; i > 0; i--) {
            texture = DrawCircle(texture, Color.green, (int)events[eventAmount - i].transform.position.x,
                                                       (int)events[eventAmount - i].transform.position.z, 30);
        }

        texture.Apply();

        SaveTextureToFile(texture, "WorldMap.png");

        return texture;
    }

    private List<GameObject> CreateEventList(bool dynamic) {

        List<GameObject> events = new List<GameObject>();

        if (dynamic) {
            GameObject eventAreaParent = GameObject.Find("EventAreas");

            EventSpawner[] spawners = eventAreaParent.GetComponentsInChildren<EventSpawner>();

            foreach (EventSpawner spawner in spawners) {
                events.Add(spawner.gameObject);
            }
        }

        EventCapsule[] eventCapsules = GameObject.FindObjectsOfType<EventCapsule>();

        foreach (EventCapsule capsule in eventCapsules) {
            events.Add(capsule.gameObject);
        }

        return events;
    }

    private Texture2D DrawCircle(Texture2D tex, Color colour, int cx, int cy, int radius) {
        
        Color[] col = new Color[4];

        for (int i = 0; i < col.Length; i++) {
            col[i] = colour;
        }

        //Outline circle
        int y = radius;
        int d = 1 / 4 - radius;
        float end = Mathf.Ceil(radius / Mathf.Sqrt(2));

        for (int x = 0; x <= end; x++) {
            tex.SetPixels(cx + x, cy + y, 2, 2, col);
            tex.SetPixels(cx + x, cy - y, 2, 2, col);
            tex.SetPixels(cx - x, cy + y, 2, 2, col);
            tex.SetPixels(cx - x, cy - y, 2, 2, col);
            tex.SetPixels(cx + y, cy + x, 2, 2, col);
            tex.SetPixels(cx - y, cy + x, 2, 2, col);
            tex.SetPixels(cx + y, cy - x, 2, 2, col);
            tex.SetPixels(cx - y, cy - x, 2, 2, col);

            d += 2 * x + 1;
            if (d > 0) {
                d += 2 - 2 * y--;
            }
        }

        //Filled circle
        /*float rSquared = radius * radius;

        for (int u = x - radius; u < x + radius + 1; u++)
            for (int v = y - radius; v < y + radius + 1; v++)
                if ((x - u) * (x - u) + (y - v) * (y - v) < rSquared)
                    tex.SetPixels(u, v, 2, 2, col);*/

        return tex;
    }

    private void SaveTextureToFile(Texture2D texture, string fileName) {
        byte[] bytes = texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/OutputImages/" + fileName, bytes);
        /*FileStream file = File.Open(Application.dataPath + "" + fileName, FileMode.Create);
        BinaryWriter binary = new BinaryWriter(file);
        binary.Write(bytes);
        file.Close();*/
    }

    private Texture2D TextureToBlack(Texture2D tex) {

        Color colour = Color.black;
        Color[] colourArray = tex.GetPixels();

        for (int i = 0; i < colourArray.Length; i++) {
            colourArray[i] = colour;
        }

        tex.SetPixels(colourArray);

        return tex;
    }
}
