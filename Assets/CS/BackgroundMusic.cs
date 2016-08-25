using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour {
    public static BackgroundMusic instance;
    public static AudioSource source;
    public AudioClip back;

    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";
    const string ON = "on";
    const string OFF = "off";

    public string state;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            source = GetComponent<AudioSource>();
        }
        else if (instance != null)
        {
            Destroy(gameObject);
            //source = GetComponent<AudioSource>();
        }
        
    }

	// Use this for initialization
	void Start () {
        source = GetComponent<AudioSource>();
        state = PlayerPrefs.GetString(BACKGROUND);
        print("music start(): " + state);
        //trigger = false;
        if (state == OFF)
        {
            musicOff();
        }
        else
        {
            musicOn();
        }
    }

    public static void musicOn() { source.Play(); }
    public static void musicOff() { source.Stop(); }
    
}