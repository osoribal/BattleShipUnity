﻿using UnityEngine;
using System.Collections;

public class GameControler : MonoBehaviour {
    public int turn;
    public GameObject tilePrefab;
    public GameObject[,] userGrid = new GameObject[10,10];
    public GameObject[,] aiGrid = new GameObject[10, 10];
    
    // Use this for initialization
    void Start () {
        turn = 0;

        //격자 생성
        Vector3 userzero = new Vector3(1, 0, 5);
        Vector3 aizero = new Vector3(-11, 0, 5);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                userGrid[i, j] = (GameObject)Instantiate( tilePrefab, userzero, Quaternion.identity);
                aiGrid[i, j] = (GameObject)Instantiate(tilePrefab, aizero, Quaternion.identity);
                userzero.z--;
                aizero.z--;
            }
            userzero.z = 5;
            aizero.z = 5;
            userzero.x++;
            aizero.x++;
        }
        
        
    }
	
	// Update is called once per frame
	void Update () {
        switch(turn)
        {
            case 0: //user turn
                break;
            case 1: //ai turn
                turn = 0;
                break;
            case 2: //user win
                break;
            case 3: //ai win
                break;
            default:
                break;
        }
	
	}
    
}
