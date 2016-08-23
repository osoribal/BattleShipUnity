using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
    private AudioSource source;
    public AudioClip back;

    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";
    const string ON = "on";
    const string OFF = "off";

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

	// Use this for initialization
	void Start () {
        string onOff = PlayerPrefs.GetString(BACKGROUND);
        print("back : " + onOff);
        if (onOff == ON) {
            source.Play();
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
