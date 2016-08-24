using UnityEngine;
using System.Collections;

public class BombSound : MonoBehaviour {

    const string EFFECT = "Effect";
    const string ON = "on";
    const string OFF = "off";
    //bom sound - bomb with ship
    public AudioClip bombSound;
    private AudioSource bombSource;
    

    //awake sound
    void Awake()
    {
        bombSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision coll)
    {
        if (PlayerPrefs.GetString(EFFECT) == ON)
        {   
            //bomb sound
            bombSource.PlayOneShot(bombSound, 1F);
        }

    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
