using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectShipCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    
    public void OnBackClicked()
    {
        SceneManager.LoadScene("Title");
    }
    public void OnNextClicked()
    {
        if (UserManager.selectedShipCount != 5)
            return;
        SceneManager.LoadScene("PlaceShip");
    }

    // Use this for initialization
    void Start()
    {
        UserManager.selectedShipCount = 0;
        UserManager.selectedShipArr[0] = -1;
        UserManager.selectedShipArr[1] = -1;
        UserManager.selectedShipArr[2] = -1;
        UserManager.selectedShipArr[3] = -1;
        UserManager.selectedShipArr[4] = -1;

        //화면에 배들을 버튼으로 출력
        for (int i = 0; i < UserManager.list.Count; i++)
        {
            //보유개수 만큼 출력
            for (int j = 1; j <= UserManager.list[i].count; j++)
            {
                //오브젝트 생성
                GameObject elem = (GameObject)Instantiate(elemPrefab) as GameObject;
                //배의 정보를 항목에 전달
                elem.GetComponent<ElemCtrl>().info = UserManager.list[i];   
                //리스트에 추가
                elem.transform.SetParent(content.transform, false);
            }
            
        }
    }

    //선택된 배의 개수 변화를 ui에 반영
    void Update()
    {
        this.GetComponentsInChildren<Text>()[1].text = UserManager.selectedShipCount + " / 5";
    }

}
