using UnityEngine;
using System.Collections;

public class AIControler : MonoBehaviour {
    int turn;
    bool firstHit;  //두 번 발사하는 배의 경우 몇 번째 발사인지 기록
    public GameObject DialogPrefab;

    //direction info
    private const int EAST = 1;
    private const int WEST = 3;
    private const int SOUTH = 2;
    private const int NORTH = 0;

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
    public int prevX, prevY, prevR;
    public int curX, curY, curR;
    public int stdX, stdY, stdR;
    public int hit;
    int[,] shootingGrid = new int[10, 10];

    // Use this for initialization
    void Start () {
        turn = 5;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        firstHit = true;
        
        //prev point init
        prevX = -1;
        prevY = -1;
        prevR = -1;

        hit = 0;

        //init 0 shooting grid
        for (int i = 0; i < 10; i++) {
            for(int j=0; j<10; j++) {
                shootingGrid[i,j] = 0;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == AI_TURN)
        {
            DialogCtrl dialog = Instantiate(DialogPrefab).GetComponent<DialogCtrl>();
            dialog.setLifetime(1.0f);
            dialog.setText("AI 턴 입니다.");
            gc.turn = AI_BLOCK;    //block turn
			int time = Random.Range(2, 3);
            Invoke("shooting", time);
        }
	}

    void shooting()
    {
        selectTargetPoint();
        shot(curX, curY);
        //save shooting
        shootingGrid[curX, curY] = 1;
    }

    void selectTargetPoint()
    {
        //target point x y - in user grid
        int userGridX, userGridY;
        int userRot;

        //check prev hit state
        hit = gc.contnueAttack;
        print("hit : " + hit);
        //init
        userGridX = 0;
        userGridY = 0;
        userRot = 0;

        if (prevR == -1 && hit == 0)
        {
            //first shoot - random
            //select random
            userGridX = Random.Range(0, 10);
            userGridY = Random.Range(0, 10);
            userRot = Random.Range(0, 4);
            print("first shoot " + userGridX + " " + userGridY);
        }
        else if (prevR != -1 && hit == 0)
        {
            selectRandomPoint();
            print("first prevR != -1 && hit == 0 : " + userGridX + " " + userGridY);
        }
        else if (hit == 1) {
            print("1hit");
            // save standart point - previous
            stdX = prevX;
            stdY = prevY;
            stdR = prevR;

            //next target point
            switch (stdR)
            {
                case EAST:
                    print("east : " + hit);
                    if (userGridX < 9)
                    {
                        userGridX++;
                    }
                    else {
                        //change west
                        userGridX = stdX--;
                        userGridY = stdY;
                        userRot = WEST;
                    }
                    break;

                case WEST:
                    print("west : " + hit);
                    if (userGridX > 0)
                    {
                        userGridX--;
                    }
                    else
                    {
                        //change east
                        userGridX = stdX++;
                        userGridY = stdY;
                        userRot = EAST;
                    }
                    break;

                case SOUTH:
                    print("south : " + hit);
                    if (userGridY < 9)
                    {
                        userGridY++;
                    }
                    else
                    {
                        //change north
                        userGridX = stdX;
                        userGridY = stdY--;
                        userRot = NORTH;
                    }
                    break;

                case NORTH:
                    print("north : " + hit);
                    if (userGridY > 0)
                    {
                        userGridY--;
                    }
                    else
                    {
                        //change south
                        userGridX = stdX;
                        userGridY = stdY++;
                        userRot = SOUTH;
                    }
                    break;
            }

        }
        else
        {
            print("1 hit more");
            //hit previous attack
            switch (stdR)
            {
                case EAST:
                    if (userGridX < 9)
                    {
                        userGridX++;
                    }
                    else
                    {
                        //change west
                        userGridX = stdX--;
                        userGridY = stdY;
                        userRot = WEST;
                    }
                    break;

                case WEST:
                    if (userGridX > 0)
                    {
                        userGridX--;
                    }
                    else
                    {
                        //change east
                        userGridX = stdX++;
                        userGridY = stdY;
                        userRot = EAST;
                    }
                    break;

                case SOUTH:
                    if (userGridY < 9)
                    {
                        userGridY++;
                    }
                    else
                    {
                        //change north
                        userGridX = stdX;
                        userGridY = stdY--;
                        userRot = NORTH;
                    }
                    break;

                case NORTH:
                    if (userGridY > 0)
                    {
                        userGridY--;
                    }
                    else
                    {
                        //change south
                        userGridX = stdX;
                        userGridY = stdY++;
                        userRot = SOUTH;
                    }
                    break;
            }

        }

        curX = userGridX;
        curY = userGridY;
        curR = userRot;

        print("select: " + curX + " " + curY );
        //save current point
        savePrevPoint();
    }

    //select random point
    void selectRandomPoint() {
        //target point x y - in user grid
        int userGridX, userGridY;
        int userRot;

        //not first shoot - no hit
        do
        {
            //select random - but no repeat
            userGridX = Random.Range(0, 10);
            userGridY = Random.Range(0, 10);
            userRot = Random.Range(0, 4);
        } while (shootingGrid[prevX, prevY] != 0);

        curX = userGridX;
        curY = userGridY;
        curR = userRot;

    }

    public void shot(int Gx, int Gy)
    {
        
        //Gx, Gy -> real x z
        //Gx : 0 -> 1
        //Gy : 0 -> -5
        float realX = Gy + 1;
        float realZ = Gx - 5;

        print("Gx,Gy : "+ Gx + " " + Gy + "realxz : " + realX + " " + realZ );
        //탄환 생성
        GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(-11, 1, 0), Quaternion.identity);
        //탄환 코드에 변수값 전달 -> 탄환 스스로 발사
        Bullet bc = bullet.GetComponent<Bullet>();
        bc.from = new Vector3(-12, 2, 0);
        bc.to = new Vector3(realX, 0, realZ);

        print(gc.ships[turn].shipID);
        //두 발 쏘는 특수기능 처리
        if (gc.ships[turn].shipID % 10 == 3)   //두 발 쏘기
        {
            //두 발 중 첫 발
            if (firstHit == true)   
            {
                firstHit = false;
                bc.turnChange = false;
            }
            else
            {
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
    }

    void savePrevPoint() {
        //save current targt point to prev point
        prevX = curX;
        prevY = curY;
        prevR = curR;
    }

    void changeTurn()
    {
        turn++;
        if (turn > 9)
            turn = 5;
    }
}
