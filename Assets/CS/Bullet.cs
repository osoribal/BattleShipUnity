using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
    const string EFFECT = "Effect";
    const string ON = "on";
    const string OFF = "off";

    public bool turnChange; //발사 후 턴을 바꿀 지 다시 쏠지 결정하는 변수

    public Vector3 from;
    public Vector3 to;
    public float value; //총알의 속도를 조절하는 변수
    private float startTime;
    public GameControler gameController;    //여기가 아니라 start에서 초기화해야 한다.
    
    SeaControler sea;
    bool hit;   //배를 맞췄는지 여부를 저장하는 변수
    Vector3 hitPosition; //맞은 배의 좌표
    Vector3 aimPosition; //number 2 option - aim position
    //turn
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;
    public const int USER_WIN = 2;
    public const int AI_WIN = 3;

    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    //audio
    private AudioSource source;
    //shoot sound - bullet shoot start
    public AudioClip shootSound;

    



    //sound awake
    void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        
        startTime = Time.time;
        hit = false;

        print("shoot sound : " + PlayerPrefs.GetString(EFFECT));
        if (PlayerPrefs.GetString(EFFECT) == ON)
        {         //shoot sound at init
            source.PlayOneShot(shootSound, 1F);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(transform != null) {
            Vector3 center = (from + to) * 0.5F;
            center -= new Vector3(0, 2, 0);
            Vector3 riseRelCenter = from - center;
            Vector3 setRelCenter = to - center;
            float fracComplete = (Time.time - startTime)/value;
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
                //bomb with enemy at same point
                int num = gameController.getHittedShipNumber(hitPosition);
                print("num : " + num);
                if (num%10 == 2) {
                    bombWithShip(hitPosition);
                 }
                
                break;
            case "Tile":
                
                //Debug.Log("tile hit");
                //check occpied
                sea = other.GetComponent<SeaControler>();
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

                    //두 발 쏘는 특수능력 처리
                    if (turnChange == false)
                    {
                        print("attack again");
                        AttackAgain();
                    }
                    else
                    {
                        //change turn
                        ChangeTurn();
                    }
                }
                else
                {//occpied > 0 -> occpied --; life--; 연기, AttackAgain()

                    //decrease occupied value
                    decOccAtMap(sea.transform.position.x, sea.transform.position.z);

                    //fire on
                    sea.fireOn();

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
        print("destroyed");
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

    void bombWithShip(Vector3 position) {
        //hitted ship - user
        //change to ai grid
        if (position.x > 0) {
            print("user hitted");
            aimPosition.x = position.x - 12;
            aimPosition.y = position.y;
            aimPosition.z = position.z;
            //shoot
            gameController.shoot(new Vector3(2, 1, 0), aimPosition);
        }

        //hitted ship - ai
        //change to user grid
        if (position.x < 0)
        {
            print("ai hitted");
            aimPosition.x = position.x + 12;
            aimPosition.y = position.y;
            aimPosition.z = position.z;

            //shoot
            gameController.shoot(new Vector3(-12, 2, 0), aimPosition);
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
