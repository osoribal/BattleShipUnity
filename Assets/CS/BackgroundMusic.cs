using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
    public static BackgroundMusic instance;
    private static AudioSource source;
    public AudioClip back;

    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";
    const string ON = "on";
    const string OFF = "off";

    public string state;
    public bool trigger;

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
        print("awake");
    }

	// Use this for initialization
	void Start () {
        state = PlayerPrefs.GetString(BACKGROUND);
        trigger = false;
        print("back : " + state);
        if (state == OFF)
        {
            musicOff();
        }
        else {
            musicOn();
        }
	}

    public static void musicOn() { source.Play(); }
    public static void musicOff() { source.Stop(); }

	// Update is called once per frame
	void Update () {
    }
}