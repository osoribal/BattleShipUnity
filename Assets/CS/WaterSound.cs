using UnityEngine;
using System.Collections;

public class WaterSound : MonoBehaviour {
    private AudioSource source;
    //water sound
    public AudioClip waterSound;

    const string EFFECT = "Effect";
    const string ON = "on";
    const string OFF = "off";

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }


    void OnTriggerEnter(Collider coll)
    {
        if (PlayerPrefs.GetString(EFFECT) == ON)
        {
            //water sound
            source.PlayOneShot(waterSound, 0.1F);

        }
    }
}
