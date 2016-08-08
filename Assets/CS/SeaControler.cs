using UnityEngine;
using System.Collections;

public class SeaControler : MonoBehaviour {
    public GameObject fogPrefab;
    GameObject fog;
    public int occpied;
    /*
     * 
     * 코드 이름 tile로 변경
     * occpied 값은 gc가 주겠지
     * 
     */
    void Start()
    {
        occpied = 0;
    }

    public void fogOn()
    {
        fog = (GameObject)Instantiate(fogPrefab, transform.position, Quaternion.identity);
    }

    public void fogOff()
    {
        Destroy(fog);
    }
}
