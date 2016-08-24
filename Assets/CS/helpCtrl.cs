using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class helpCtrl : MonoBehaviour {
    public Texture[] images;
    int now;

	// Use this for initialization
	void Start () {
        now = 0;
        gameObject.GetComponent<RawImage>().texture = images[now];
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
