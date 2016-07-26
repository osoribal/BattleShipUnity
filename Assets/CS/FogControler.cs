using UnityEngine;
using System.Collections;

public class FogControler : MonoBehaviour {
    public GameObject fogPrefab;
    GameObject fog;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void fogOn()
    {
        fog = (GameObject)Instantiate(fogPrefab, transform.position, Quaternion.identity);
    }

    public void fogOff()
    {
        Destroy(fog);
    }
}
