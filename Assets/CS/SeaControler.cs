using UnityEngine;
using System.Collections;

public class SeaControler : MonoBehaviour {
    public GameObject fogPrefab;
    public GameObject firePrefab;
    GameObject fog;
    GameObject fire;
    

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

    //fire particle
    public void fireOn()
    {
        fire = (GameObject)Instantiate(firePrefab, transform.position, Quaternion.Euler(-90, 0, 0));
    }
    
}
