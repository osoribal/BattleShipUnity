using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {
    //골드 보유 ui
    public Text txt;
    
    void Start()
    {
        //보유골드 표시
        //title 씬에서는 골드의 변동이 없기 때문에 Start 에서만 txt를 지정해준다
        txt.text = "gold : " + PlayerPrefs.GetInt("gold");
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("SelectShip");
    }

    public void OnManagementClicked()
    {
        SceneManager.LoadScene("Ship List");
    }

    public void OnRandomClicked()
    {
        SceneManager.LoadScene("RandomSelect");
    }

	public void OnOptionClicked()
	{
		SceneManager.LoadScene("Option");
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Escape button codes
            Application.Quit();
        }

    }
}
