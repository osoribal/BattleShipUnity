using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ListShipCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    public Sprite[] shipImage;
    List<ShipInfo> list = UserManager.list;

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Escape button codes
            SceneManager.LoadScene("Title");
        }
    }

    // Use this for initialization
    void Start () {
        //화면에 배들을 버튼으로 출력
        for (int i = 0; i < list.Count; i++)
        {
            GameObject elem = (GameObject)Instantiate(elemPrefab) as GameObject;
            //스냅샷 
            elem.GetComponentsInChildren<Image>()[1].overrideSprite = shipImage[list[i].shipNum / 10 - 1];
            //배의 정보를 string으로 만들기
            string str = "배 길이 : " + list[i].shipNum / 10
                + "\n보유 수 : " + list[i].count;
            switch(list[i].shipNum % 10)
            {
                case 2:
                    str = str + "\nskill : 대응 좌표점\n\t 같이 폭발";
                    this.GetComponentInChildren<Text>().alignByGeometry = false;
                    break;
                case 3:
                    str = str + "\n   skill : 두 발 쏘기\n";
                    break;
                case 4:
                    str = str + "\n       skill : 보상 up\n";
                    break;
                default:
                    str = str + "\n           skill : None\n";
                    break;
            }
            elem.GetComponentInChildren<Text>().text = str;
            //필요없는 ElemCtrl 삭제
            //Destroy(elem.GetComponent<ElemCtrl>());
            //배치
            elem.transform.SetParent(content.transform, false);
        }
    }
    
}
