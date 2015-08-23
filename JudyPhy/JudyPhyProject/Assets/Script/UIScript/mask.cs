using UnityEngine;
using System.Collections;

public class mask : MonoBehaviour {

    private float offsetX = 0;
    private float offsetY = 0;

    public Texture[] masks;
    private float b = 0;
    private float c = 0.1f;
    private int index = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Cycle();

        offsetX = 0.5f;
        offsetY = 0.5f;
        renderer.material.SetTextureOffset("_Mask", new Vector2(offsetX, offsetY));
	}

    private void Cycle() {
        if (Time.time > b) {
            b = b + c;
            if (index < masks.Length - 1)
            {
                index++;
            }
            else
            {
                index = 0;
            }
            renderer.material.SetTexture("_Mask", masks[index]);
        }
    }
}
