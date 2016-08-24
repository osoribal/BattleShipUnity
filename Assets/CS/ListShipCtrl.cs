using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ListShipCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    public Sprite[] shipImage;
    public GameObject DialogPrefab;
    GameObject[] elems;

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
        listElems();
    }

    void removeElem(int shipNum)
    {
        UserManager.removeShip(shipNum);
        listElems();

        DialogCtrl dialog = Instantiate(DialogPrefab).GetComponent<DialogCtrl>();
        dialog.setLifetime(1.0f);
        dialog.setText("배가 판매되었습니다.\n + 100 Gold\n");
        UserManager.updateGold(100);
    }

    void initElems()
    {
        if (elems != null)
        {
            for (int i = 0; i < elems.Length; i++)
                Destroy(elems[i]);
        }
        elems = new GameObject[UserManager.list.Count];
    }
    
    void listElems()
    {
        initElems();
        //화면에 배들을 버튼으로 출력
        for (int i = 0; i < UserManager.list.Count; i++)
        {
            elems[i] = (GameObject)Instantiate(elemPrefab) as GameObject;
            //스냅샷 
            elems[i].GetComponentsInChildren<Image>()[1].overrideSprite = shipImage[UserManager.list[i].shipNum / 10 - 1];
            //배의 정보를 string으로 만들기

			string str = "배 길이 : " + UserManager.list[i].shipNum / 10
				+ "\n보유 수 : " + UserManager.list[i].count;
			elems[i].GetComponentInChildren<Text>().alignByGeometry = true;
			switch (UserManager.list[i].shipNum % 10) {
			case 2:
				str = str + "\nskill : 동귀어진";
				break;
			case 3:
				str = str + "\nskill : 두 발 쏘기";
				break;
			case 4:
				str = str + "\nskill : 보상 up";
				break;
			default:
				str = str + "\nskill : None";
				break;
			}
			elems[i].GetComponentInChildren<Text>().text = str;
			int buf = UserManager.list[i].shipNum;
			elems[i].GetComponentsInChildren<Button>()[1].onClick.AddListener(() => removeElem(buf));
            //배치
            elems[i].transform.SetParent(content.transform, false);
        }
    }
}
