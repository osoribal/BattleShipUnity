using UnityEngine;
using System.Collections;

public class SeaControler : MonoBehaviour {
    public GameObject fogPrefab;
    GameObject fog;
    
    public int occpied; //현재 타일에 배가 있는지 검사할 때 이용
    GameControler gameController;   //여기가 아니라 start에서 초기화해야 한다.

    void Start()
    {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
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

    //get occupied value
    public int getOcc()
    { return occpied;  }

    //set occupied value
    public void setOcc(int occ)
    { occpied = occ;  }

    //decrease occupied value
    public void decOcc()
    { occpied--;  }

    //get occupied value with real x, y
    public int getOccupiedWithXY(float x, float y)
    {
        int gridX, gridY;
        int occ = 0;
        //change to grid x y
        if (x > 0)//user grid
        {
            /*
             * x : 1~10
             * z : -5~4
             */
            gridX = (int)x-1;
            gridY = (int)y + 5;
            
            Debug.Log("USER, getOccupiedWithXY): " + x + " " + y);
        }
        else //ai grid
        {
            /*
             * x : -11~-2
             * z : -5~4
             */
            Debug.Log("AI, getOccupiedWithXY): " + x + " " + y);
            gridX = (int)x +11;
            gridY = (int)y + 5;
            
        }

        Debug.Log("SEA CON, getOccupiedWithXY): " + gridY + " " + gridX + " occ : " + occ);
        return occ;
    }
}
