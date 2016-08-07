using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ShowShipList : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    List<ShipInfo> list = ShipListControl.list;


    public void OnBackClicked()
    {
        SceneManager.LoadScene("Title");
    }


    // Use this for initialization
    void Start () {

        for (int i = 0; i < list.Count; i++)
        {
            print(list[i].size);
            GameObject elem = (GameObject)Instantiate(elemPrefab) as GameObject;
            elem.transform.SetParent(content.transform, false);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
