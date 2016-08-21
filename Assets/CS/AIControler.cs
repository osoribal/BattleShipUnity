using UnityEngine;
using System.Collections;

public class AIControler : MonoBehaviour {
    int turn;
    bool firstHit;

    //shot bullet
    public GameControler gc;
    public Camera camera;
    public GameObject bulletPrefab;
    public GameObject arrowPrefab;  //맞을 지점을 표시할 프리팹

    //ai turn
    public const int AI_TURN = 1;
    public const int USER_TURN = 0;
    public const int AI_BLOCK = -2;

    //previous target point
    int prevX, prevY, prevR;
    bool hit;

    // Use this for initialization
    void Start () {
        turn = 5;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        firstHit = true;

        //prev point init
        prevX = -1;
        prevY = -1;
        prevR = -1;

        hit = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == AI_TURN)
        {
            gc.turn = AI_BLOCK;    //block turn
            selectTargetPoint();
            shot(prevX, prevY);
        }
	}

    void selectTargetPoint()
    {
        //target point x y - in user grid
        int userGridX, userGridY;

        //init
        userGridX = 0;
        userGridY = 0;
        if (prevR == -1 && hit == false)
        {
            //first shoot - random
            userGridX = Random.Range(0,10);
            userGridY = Random.Range(0, 10);
            //Debug.Log(userGridX + " real " + userGridY);
        }
        else if(prevR != -1 && hit == false){
            userGridX = Random.Range(0, 10);
            userGridY = Random.Range(0, 10);
        }

        prevX = userGridX;
        prevY = userGridY;
    }

    public void shot(int Gx, int Gy)
    {
        //Gx, Gy -> real x z
        //Gx : 0 -> 1
        //Gy : 0 -> -5
        float realX = Gx + 1;
        float realZ = Gy - 5;

        //카메라 정보 저장
        Vector3 beforePosition = camera.transform.position;
        Quaternion beforeLookAt = camera.transform.rotation;

        //카메라 시점 이동
        //camera.transform.position = new Vector3(2, 2, 0);
        //camera.transform.LookAt(new Vector3(-50, 1, 0));

        //탄환 생성
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(-11, 1, 0), Quaternion.identity);
        //탄환 코드에 변수값 전달 -> 탄환 스스로 발사
        Bullet bc = bullet.GetComponent<Bullet>();
        bc.from = new Vector3(-12, 2, 0);
        //bullet target transform
        //var newTrans = new GameObject().transform;
        //Vector3 targetVec = new Vector3(realX, 0, realZ);
        //Debug.Log("input : " + Gx + " " + Gy + " real : " + realX + " " + realZ);
        //newTrans.position = targetVec;
        bc.to = new Vector3(realX, 0, realZ);
        //print(realX + " " + realZ);

        print(gc.ships[turn].shipID);
        //두 발 쏘는 특수기능 처리
        if (gc.ships[turn].shipID % 10 == 3)   //두 발 쏘기
        {
            //두 발 중 첫 발
            if (firstHit == true)   
            {
                print("firstHit");
                firstHit = false;
                bc.turnChange = false;
            }
            else
            {
                print("secondHit");
                //두 발 중 두번째 발
                firstHit = true;
                bc.turnChange = true;
                changeTurn();
            }
        }
        else
        {
            //한 발 쏘기
            bc.turnChange = true;
            changeTurn();
        }

        //카메라 원위치
        camera.transform.position = beforePosition;
        camera.transform.rotation = beforeLookAt;
                    
     
    }

    void changeTurn()
    {
        turn++;
        if (turn > 9)
            turn = 5;
    }
}
