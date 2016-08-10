using UnityEngine;
using System.Collections;

public class SeaControler : MonoBehaviour {
    public GameObject fogPrefab;
    public int x, y;
    GameObject fog;
    public int occpied; //현재 타일에 배가 있는지 검사할 때 이용

    void Start()
    {
        occpied = 0;
    }

    //안개 생성
    public void fogOn()
    {
        fog = (GameObject)Instantiate(fogPrefab, transform.position, Quaternion.identity);
    }

    //안개 제거
    public void fogOff()
    {
        Destroy(fog);
    }
}
