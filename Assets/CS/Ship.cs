using UnityEngine;
using System.Collections;


public class Ship : MonoBehaviour {
    public int x, y;    //배 머리 위치
    public int direction;   //배 방향

    /*
     * switch 문 2개 구현 : prefab, 특수능력
     * prefab은 swith문 안만들어도..?
     * 
     */
    //ship number
    int shipID;
   /*
   * 위치, 방향 채우기
   * east = 0
   * west = 1
   * south = 2
   * north = 3
   * 
   */

    //turn flags
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;
    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    // Use this for initialization
    void Start () {
        //size
        int size = shipID/10;
        switch (size)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

        //fuction
        int func = shipID%10;
        switch (func)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;

        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    
    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Bullet") Destroy(other.gameObject);
        //notify hit
        //OnNotify();
    }

 /*   void OnNotify()
    {
        //notify to game controller
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        int whoseTurn = gameController.GetTurn();
        
        switch (whoseTurn)
        {
            case USER_TURN:
                break;
            case AI_TURN:
                break;
            case USER_BLOCK:
                gameController.turn = USER_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = AI_TURN;
                break;
                
        }
    }*/


}
