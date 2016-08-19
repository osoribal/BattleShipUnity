using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    
    public Vector3 from;
    public Vector3 to;
    private float startTime;
    public GameControler gameController;    //여기가 아니라 start에서 초기화해야 한다.
    SeaControler sea;
    bool hit;   //배를 맞췄는지 여부를 저장하는 변수
    Vector3 hitPosition; //맞은 배의 좌표
    //turn
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;
    public const int USER_WIN = 2;
    public const int AI_WIN = 3;

    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        startTime = Time.time;
        hit = false;
       // gameController.show();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform != null) {
            Vector3 center = (from + to) * 0.5F;
            center -= new Vector3(0, 2, 0);
            Vector3 riseRelCenter = from - center;
            Vector3 setRelCenter = to - center;
            float fracComplete = (Time.time - startTime);
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
        }

    }

    //destroy bullet
    void OnTriggerEnter(Collider other)
    {
        //gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        switch (other.gameObject.tag)
        {
            case "Arrow":
                //Debug.Log("arrow hit");
                Destroy(other.gameObject);
                to.y = 0f;
                break;
            case "Ship":
                //ship hit
                hit = true;
                hitPosition = other.gameObject.transform.position;
                break;
            case "Tile":
                //Debug.Log("tile hit");
                //check occpied
                sea = other.GetComponent<SeaControler>();
                //ai shot
                //if (sea.transform.position.x > 0)
                //{
                //    if (hit == true)    //hit user ship
                //    {
                //        sea.fireOn();
                //        //attack again - no change turn
                //        AttackAgain();
                //    }
                //    else
                //    {
                //        //change turn
                //        ChangeTurn();
                //    }
                //}
                //else
                //{   //user shot
                    //get occupied value
                int isOcc = getOccFromMap(sea.transform.position.x, sea.transform.position.z);
                //print(isOcc);
                if (isOcc == 0)
                {
                    //no hit 
                    if (hit == false)
                    {
                        //remove fog
                        sea.fogOff();
                    }

                    //change turn
                    ChangeTurn();
                }
                else
                {//occpied > 0 -> occpied --; life--; 연기, AttackAgain()

                    //decrease occupied value
                    decOccAtMap(sea.transform.position.x, sea.transform.position.z);

                    //fire on
                    sea.fireOn(getOccFromMap(sea.transform.position.x, sea.transform.position.z));

                    //check all parts of ship is hitted
                    //only for aigrid
                    if (sea.transform.position.x < 0)
                    {
                        gameController.checkShipHitted(hitPosition);
                    }
                        

                    //decrease life
                    switch (whoseTurn())
                    {
                        case USER_BLOCK:
                            //minus ai life
                            gameController.minusAILife();
                            //check life is 0
                            if (gameController.GetAILife() == 0)
                            {
                                Debug.Log("user win");
                                gameController.turn = USER_WIN;
                            }
                            break;
                        case AI_BLOCK:
                            //minus user life
                            gameController.minusUserLife();
                            //check life is 0
                            if (gameController.GetUserLife() == 0)
                            {
                                Debug.Log("ai win");
                                gameController.turn = AI_WIN;
                            }
                            break;
                    }
                    //attack again - no change turn
                    AttackAgain();
                }
                //}

                //remove bullet
                destroyBullet();
                break;
        }
    }

    void destroyBullet()
    {
        //destroy bullet game object
        Destroy(this.gameObject);
    }

    //no hit - change turn
    void ChangeTurn()
    {
        //change turn each case
        switch (whoseTurn())
        {
            case USER_BLOCK:
                gameController.turn = AI_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = USER_TURN;
                break;
        }
    }

    //get turn from game controller
    public int whoseTurn()
    {
        //get game controller's turn
        int whoseTurn = gameController.GetTurn();
        return whoseTurn;
    }

    //hit - not change turn, attack again
    void AttackAgain()
    {
        //change turn each case
        switch (whoseTurn())
        {
            case USER_BLOCK:
                gameController.turn = USER_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = AI_TURN;
                break;
        }
    }

    //get occupied value from each map
    //input : location of sea - in real point
    int getOccFromMap(float x, float y)
    {
        //in grid point
        int gridX, gridY;
        int occ = 0;
        //change to grid x y
        if (x > 0)//user grid
        {
            /*
             * x : 1~10
             * z : -5~4
             */
            gridX = (int)x - 1;
            gridY = (int)y + 5;
            
            occ = gameController.getUserOcc(gridX, gridY);
            //Debug.Log("USER, getOccFromMap : " + gridX + " " + gridY + " " + occ);
            return occ;

        }
        else //ai grid
        {
            /*
             * x : -11~-2
             * z : -5~4
             */
            gridX = (int)x + 11;
            gridY = (int)y + 5;

            occ = gameController.getAIOcc(gridX, gridY);
            //Debug.Log("aI, getOccFromMap : " + x + " " + y + " " + gridX + " " + gridY + " " + occ);
            return occ;

        }
    }

    //dec occupied value from each map
    //input : location of sea - in real point
    int decOccAtMap(float x, float y)
    {
        //in grid point
        int gridX, gridY;
        int occ = 0;
        //change to grid x y
        if (x > 0)//user grid
        {
            /*
             * x : 1~10
             * z : -5~4
             */
            gridX = (int)x - 1;
            gridY = (int)y + 5;

            gameController.decUserOcc(gridX, gridY);
            //Debug.Log("USER, getOccFromMap : " + gridX + " " + gridY + " " + occ);
            return occ;

        }
        else //ai grid
        {
            /*
             * x : -11~-2
             * z : -5~4
             */
            gridX = (int)x + 11;
            gridY = (int)y + 5;

            gameController.decAiOcc(gridX, gridY);
            //Debug.Log("aI, getOccFromMap : " + x + " " + y + " " + gridX + " " + gridY + " " + occ);
            return occ;

        }
    }
}
