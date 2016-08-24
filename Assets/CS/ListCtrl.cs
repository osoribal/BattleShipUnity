using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ListCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    public Sprite[] shipImage;
    List<ShipInfo> list = UserManager.list;

    void Update()
            elem.GetComponentInChildren<Text>().text = str;
            //필요없는 ElemCtrl 삭제
            Destroy(elem.GetComponent<ElemCtrl>());
            //배치
            elem.transform.SetParent(content.transform, false);
        }
    }
    
}
