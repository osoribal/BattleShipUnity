using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishGame : MonoBehaviour {

    int gold;
    string winner;
    public UserManager userManager;
    public Text gold_text;
    public Text winner_text;
	// Use this for initialization
	void Start () {
        //title
        //show who is the winner
        winner = PlayerPrefs.GetString("winner");
        if (winner == "user")
        {
            winner_text.text = "당신의 승리!!";
        }
        else if (winner == "ai")
        {
            winner_text.text = "Game Over";
        }
        else
        {
            winner_text.text = "Game Over";
        }

        //set gold using PlayerPref
        //get current money
        gold = PlayerPrefs.GetInt("getGold");
        gold_text.text = gold + " 골드 획득!";
        userManager.updateGold(gold);

        //pref init
        PlayerPrefs.SetInt("getGold", 0);
        PlayerPrefs.SetString("winner", "");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void onClickGoMain()
    {
        //load main title page
        SceneManager.LoadScene("Title");
    }

    public void onClickGoRandom()
    {
        //load random select ship page
        //Gold > 1000 : active
        if (gold >= 1000)
        { SceneManager.LoadScene("RandomSelect"); }
        else
        { }
        
    }
}
