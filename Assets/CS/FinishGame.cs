using UnityEngine;
using System.Collections;

public class FinishGame : MonoBehaviour {

    int gold;
	// Use this for initialization
	void Start () {
		//set gold using PlayerPref
		//get current money
		gold = PlayerPrefs.GetInt("gold");
        gold = 1001;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onClickGoMain()
    {
        //load main title page
        Application.LoadLevel("Title");
    }

    public void onClickGoRandom()
    {
        //load random select ship page
        //Gold > 1000 : active
        if (gold >= 1000)
        { Application.LoadLevel("RandomSelect"); }
        else
        { }
        
    }
}
