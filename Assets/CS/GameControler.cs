using UnityEngine;
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

                //ai 격자 전체에 안개 씌우기
                FogControler fg = aiGrid[i, j].GetComponent<FogControler>();
                fg.fogOn();

                userzero.z--;
                aizero.z--;
            }
            userzero.z = 5;
            aizero.z = 5;
            userzero.x++;
            aizero.x++;
        }
    }

    //get turn
    public int GetTurn()
    {
        return turn;
    }
	
	// Update is called once per frame
	void Update () {
        switch(turn)
        {
            case 0: //user turn
                break;
            case 1: //ai turn
               // turn = 0;
                break;
            case 2: //user win
                break;
            case 3: //ai win
                break;
            default:
                break;
        }
	
	}

    /*
     * 유저 격자의 x, y 좌표의 Transform을 반환한다.
     * 반환된 Transform을 탄환 발사 시 이용할 수 있다.
     */
    public Transform getTransformOfUserTile(int x, int y)
    {
        return userGrid[x, y].transform;
    }
    
}
