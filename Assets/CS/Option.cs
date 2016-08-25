using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Option : MonoBehaviour {

    OptionInfo opInfo = new OptionInfo(); //option information
    BackgroundMusic music;
    
    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";

    const string ON = "on";
    const string OFF = "off";

    //option state
    string effect;
    string back;

    //toggle button
    public Toggle togEffect;
    public Toggle togBack;

    // back button, go to titleScene
    public void OkayBtnListener(){
		SceneManager.LoadScene ("Title");
	}

    // Use this for initialization
    void Start () {
        togBack.isOn = true;
        togEffect.isOn = true;

        //init toggle buttons
        if (PlayerPrefs.GetString(BACKGROUND) == ON)
        { togBack.isOn = true; }
        else
        { togBack.isOn = false; }

        print("option effect sound : " + PlayerPrefs.GetString(EFFECT));
        if (PlayerPrefs.GetString(EFFECT) == ON)
        { togEffect.isOn = true;  }
        else
        { togEffect.isOn = false; }

    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape))
		{
			//Escape button codes
			SceneManager.LoadScene("Title");
		}
	}

    //toggle button event listener
    //effect
    public void Effect_Toggle_Changed(bool newValue)
    {
        //effect on
        if (newValue == true) {
            UserManager.opInfo.effect = ON;
            PlayerPrefs.SetString(EFFECT, ON);
        }
        //effect off
        if (newValue == false) {
            UserManager.opInfo.effect = OFF;
            PlayerPrefs.SetString(EFFECT, OFF);
        }
    }

    //background
    public void Background_Toggle_Changed(bool newValue)
    {
        //background on
        if (newValue == true)
        {
            UserManager.opInfo.back = ON;
            PlayerPrefs.SetString(BACKGROUND, ON);

			BackgroundMusic.musicOn ();
            music.trigger = true;
        }
        //background off
        if (newValue == false)
        {
            UserManager.opInfo.back = OFF;
            PlayerPrefs.SetString(BACKGROUND, OFF);

			BackgroundMusic.musicOff ();
            // music.musicOff();
            music.trigger = true;
        }
    }
}
