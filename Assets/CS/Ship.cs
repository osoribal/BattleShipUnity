using UnityEngine;
using System.Collections;


public class Ship : MonoBehaviour {
    public int shipID;      //ship number
    public int x, y;        //배 머리 위치
    public int direction;   //배 방향

    /*
     * switch 문 2개 구현 : prefab, 특수능력
     * prefab은 swith문 안만들어도..?
     * 
     */
    
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
    
    public IEnumerator move()
    {
        Ray ray;
        RaycastHit rayHit;
        float rayLength = 100f;
        while (true)    //드래그 하는 동안
        {
            if (Input.GetButtonUp("Fire1")) //드래그 끝
            {
                break;
            }

            /******move******/
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, rayLength))
            {
                Vector3 pos = rayHit.transform.position;
                this.gameObject.transform.position = pos;
                //x, y 설정
                x = (int)pos.x;
                y = -(int)pos.z;
            }
            yield return null;
        }

        //드래그 끝
        /******Rotate******/
        //if (Physics.Raycast(ray, out rayHit, rayLength))
        //{
        //    //탄환 생성
        //    GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(2, 1, 0), Quaternion.identity);
        //    //탄환 코드에 변수값 전달 -> 탄환 스스로 발사
        //    Bullet bc = bullet.GetComponent<Bullet>();
        //    bc.from = bullet.transform;
        //    bc.to = rayHit.transform.gameObject.transform;
        //}
        yield return null;
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
