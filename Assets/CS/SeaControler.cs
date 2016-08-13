﻿using UnityEngine;
using System.Collections;

public class SeaControler : MonoBehaviour {
    public GameObject fogPrefab;
    public int x, y;
    GameObject fog;
    
    public int occpied; //현재 타일에 배가 있는지 검사할 때 이용
    GameControler gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();

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
        //change to grid x y
        if (x > 0)//user grid
        {
            /*
             * x : 1~10
             * z : -5~4
             */
            gridX = (int)x-1;
            gridY = (int)y + 5;
            Debug.Log("SEA CON USER, getOccupiedWithXY): " + x + " " + y);
        }
        else //ai grid
        {
            /*
             * x : -11~-2
             * z : -5~4
             */
            Debug.Log("SEA CON AI, getOccupiedWithXY): " + x + " " + y);
            gridX = (int)x +11;
            gridY = (int)y + 5;
        }

        
        int occ = gameController.getGridOcc(gridY, gridX);
        Debug.Log("SEA CON, getOccupiedWithXY): " + gridY + " " + gridX + " occ : " + occ);
        return occ;
    }
}
