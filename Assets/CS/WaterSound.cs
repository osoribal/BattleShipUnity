using UnityEngine;
using System.Collections;

public class WaterSound : MonoBehaviour {
    private AudioSource source;
    //water sound
    public AudioClip waterSound;
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    
    void OnTriggerEnter(Collider coll)
    {

        source.PlayOneShot(waterSound, 0.1F);
    }
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
