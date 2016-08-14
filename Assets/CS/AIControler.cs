using UnityEngine;
using System.Collections;

public class AIControler : MonoBehaviour {

    //turn
    int turn;

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
        //my turn
        this.turn = AI_TURN;

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

        if (prevR == -1 && hit == false)
        {
            //first shoot - random
            userGridX = Random.Range(0,10);
            userGridY = Random.Range(0, 10);
            Debug.Log(userGridX + " real " + userGridY);
        }
        else if(prevR != -1 && hit == false){
            //
        }
    }

    public void shot(int Gx, int Gy)
    {
        //Gx, Gy -> real x z
        float realX = 1;
        float realZ = 4;
        

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
        bc.from = bullet.transform;
        //bullet target transform
        var newTrans = new GameObject().transform;
        Vector3 targetVec = new Vector3(realX, 0, realZ);
        Debug.Log(realX + " real " + realZ);
        newTrans.position = targetVec;
        bc.to = newTrans;
        
        
        //카메라 원위치
        camera.transform.position = beforePosition;
        camera.transform.rotation = beforeLookAt;
                    
     
    }
}
