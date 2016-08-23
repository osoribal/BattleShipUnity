using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogCtrl : MonoBehaviour {
    private float startTime;
    private float lifeTime;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {

        if (Time.time - startTime >= lifeTime)
            Destroy(this.gameObject);
	}

    public void setText(string str)
    {
        GetComponentInChildren<Text>().text = str;
    }

    public void setLifetime(float f)
    {
        lifeTime = f;
    }
}
