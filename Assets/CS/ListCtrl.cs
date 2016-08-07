using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ListCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    List<ShipInfo> list = UserManager.list;

    /*
     * 
     * 배 gameobject를 생성
     * 고유번호, 위치, 방향 다 채워서 gc로 넘기기
     * 
     */
    public void OnBackClicked()
    {
        SceneManager.LoadScene("Title");
    }


    // Use this for initialization
    void Start () {

        for (int i = 0; i < list.Count; i++)
        {
            print(list[i].shipNum);
            GameObject elem = (GameObject)Instantiate(elemPrefab) as GameObject;
            elem.transform.SetParent(content.transform, false);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
