using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Transform from;
    public Transform to;
    public float value = 10.0F;
    private float startTime;
    public GameControler gameController;    //여기가 아니라 start에서 초기화해야 한다.
    SeaControler sea;
    //turn
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;

    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    // Use this for initialization
    void Start () {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        startTime = Time.time;

       // gameController.show();
    }

    // Update is called once per frame
    void Update()
    {
       //탄환이 포물선을 그리며 이동
        if (!transform.position.Equals(to))
        {
            Vector3 center = (from.position + to.position) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = from.position - center;
            Vector3 setRelCenter = to.position - center;
            float fracComplete = (Time.time - startTime) / value;
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
                Debug.Log("arrow hit");
                Destroy(other.gameObject);
                break;
            case "Tile":
                Debug.Log("tile hit");
                //check occpied
                sea = other.GetComponent<SeaControler>();
                //get occupied value
                //int isOcc = sea.getOccupiedWithXY(sea.transform.position.x, sea.transform.position.z);
                int isOcc = getOccFromMap(sea.transform.position.x, sea.transform.position.z);
                Debug.Log("sea point"  + sea.transform.position.x + " " + sea.transform.position.z + " " + isOcc);
                if (isOcc == 0)
                {
                    //no hit
                    //remove fog
                    sea.fogOn();
                    //change turn
                    ChangeTurn();
                }
                else
                {//occpied > 0 -> occpied --; life--; 연기, AttackAgain()
                    //decrease occupied value
                    sea.decOcc();
                    //decrease life
                    
                    switch (whoseTurn()) {
                        case USER_BLOCK:
                            //minus ai life
                            gameController.minusAILife();
                            //check life is 0
                            if (gameController.GetAILife() == 0) {
                                Debug.Log("user win");
                            }
                            break;
                        case AI_BLOCK:
                            //minus user life
                            gameController.minusUserLife();
                            //check life is 0
                            if (gameController.GetUserLife() == 0)
                            {
                                Debug.Log("ai win");
                            }
                            break;
                    }
                    //smog on
                    //attack again - no change turn
                    AttackAgain();

                }
                //remove bullet
                destroyBullet();

                break;

            //case "Ship":
            //    hit - not change turn, attack again
            //    AttackAgain();
            //    destroyBullet();
            //    break;
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
    public int getOccFromMap(float x, float y)
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
            gridX = (int)y + 5;
            gridY = (int)x - 1;
            Debug.Log("USER, getOccFromMap : " + gridX + " " + gridY + " " + occ);
            occ = gameController.setOccFromUserMap(gridX, gridY);
            
            return occ;

        }
        else //ai grid
        {
            /*
             * x : -11~-2
             * z : -5~4
             */
            gridX = (int)y + 5;
            gridY = (int)x + 11;
            Debug.Log("aI, getOccFromMap : " + x  + " " +  y + " " + gridX + " " + gridY + " " + occ);
            occ = gameController.setOccFromAIMap(gridX, gridY);
            
            return occ;

        }
    }


}
