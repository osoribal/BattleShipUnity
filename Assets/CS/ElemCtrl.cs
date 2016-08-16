using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ElemCtrl : MonoBehaviour {
    public ShipInfo info;   //배의 정보
    public SelectShipCtrl selectCtrl;
    bool isSelected; //이 버튼이 선택되었는지 여부 저장
    int index;       //이 배가 몇 번 째로 선택되었는지 저장
    string str;

    // Use this for initialization
    void Start () {
        str = "ship length : " + info.shipNum / 10;
        switch (info.shipNum % 10)
        {
            case 1:
                str = str + "\nskill : 칸 당 hp 2";
                break;
            case 2:
                str = str + "\nskill : 대응 좌표점 같이 폭발";
                break;
            case 3:
                str = str + "\nskill : 두 발 쏘기";
                break;
            case 4:
                str = str + "\nskill : 보상 up";
                break;
        }
        this.GetComponentInChildren<Text>().text = str + "\nunselected";
        isSelected = false;
        this.gameObject.GetComponent<Button>().onClick.AddListener(() => elemOnClick());
    }

    //이 배가 선택 되었을 때 
    void selected(int i)
    {
        this.GetComponentInChildren<Text>().text = str + "\nselected";
        isSelected = true;
        index = i;
    }

    //이 배가 선택 해제 될 때
    void unSelected()
    {
        this.GetComponentInChildren<Text>().text = str + "\nunselected";
        isSelected = false;
    }

    //Button listener
    void elemOnClick()
    {
        if (isSelected)
        {
            //배가 선택되어 있을 때는 선택을 해제
            unSelected();
            selectCtrl.selectedShipArr[index] = 0;
            selectCtrl.selectedShipCount += -1;
            selectCtrl.userLife += -(info.shipNum / 10);
        }
        else
        {
            //배 선택 개수 제한
            if (selectCtrl.selectedShipCount == 5)
            {
                return;
            }
            //배가 선택되어 있지 않으므로 선택
            selectCtrl.selectedShipCount += 1;
            for (int i = 0; i < 5; i++)
            {
                if (selectCtrl.selectedShipArr[i] == 0)
                {
                    selected(i);
                    selectCtrl.selectedShipArr[i] = info.shipNum;
                    selectCtrl.userLife += info.shipNum / 10;
                    return;
                }
            }
        }
    }
}
