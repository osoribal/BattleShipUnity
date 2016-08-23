using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
    public static BackgroundMusic instance;
    private AudioSource source;
    public AudioClip back;

    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";
    const string ON = "on";
    const string OFF = "off";

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(gameObject);
        }

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
