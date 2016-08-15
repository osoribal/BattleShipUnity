using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Option : MonoBehaviour {

    OptionInfo opInfo = new OptionInfo(); //option information
    //option string
    const string EFFECT = "Effect";
    const string BACKGROUND = "Back";

    //option state
    string effect;
    string back;

	public void OkayBtnListener(){
		SceneManager.LoadScene ("Title");
	}

    // Use this for initialization
    void Start () {
        opInfo = UserManager.opInfo;
        Debug.Log(opInfo.effect + " " + opInfo.back);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //toggle button event listener
    //effect
    public void Effect_Toggle_Changed(bool newValue)
    {
        //effect on
        if (newValue == true) {
           
            UserManager.opInfo.effect = "on";
            Debug.Log(opInfo.effect + " true" + opInfo.back);
        }
        //effect off
        if (newValue == false) {
            
            UserManager.opInfo.effect = "off";
            Debug.Log(opInfo.effect + " false" + opInfo.back);
        }
    }
}
