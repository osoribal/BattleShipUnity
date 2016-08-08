using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaceShipCtrl : MonoBehaviour {
    /*
    * 
    * 배 gameobject를 생성
    * 고유번호, 위치, 방향 다 채워서 gc로 넘기기
    * 
    */

    public void OnBackClicked()
    {
        SceneManager.LoadScene("SelectShip");
    }
    public void OnNextClicked()
    {
        SceneManager.LoadScene("Battle");
    }

    // Use this for initialization
    void Start () {
	    for (int i = 0; i < 5; i++)
        {
            print(UserManager.selectedShipArr[i]);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
