using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ListCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    List<ShipInfo> list = UserManager.list;
    
    public void OnBackClicked()
    {
        SceneManager.LoadScene("Title");
    }


    // Use this for initialization
    void Start () {
        //화면에 배들을 버튼으로 출력
        for (int i = 0; i < list.Count; i++)
        {
            GameObject elem = (GameObject)Instantiate(elemPrefab) as GameObject;
            elem.GetComponent<ElemCtrl>().info = list[i];
            elem.transform.SetParent(content.transform, false);
        }
    }
    
}
