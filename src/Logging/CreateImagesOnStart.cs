using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateImagesOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start() {
        Texture2D path = this.gameObject.GetComponent<PlayerPath>().DrawPath();
        Texture2D map = this.gameObject.GetComponent<MapOutput>().DrawMap();
        ImageMerger.MergeImages(map, path);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
