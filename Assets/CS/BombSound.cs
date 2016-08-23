using UnityEngine;
using System.Collections;

public class BombSound : MonoBehaviour {


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
        //bomb sound
        bombSource.PlayOneShot(bombSound, 1F);
    }

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
