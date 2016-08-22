using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameControler : MonoBehaviour {
    public bool firstHit;
    public int turn;
    public GameObject tilePrefab;
    UserManager userManager;
    
    //public GameObject[,] userGrid = new GameObject[10, 10];
    //public GameObject[,] aiGrid = new GameObject[10, 10];
    public SeaControler[,] userGridCtrl = new SeaControler[10, 10];
    public SeaControler[,] aiGridCtrl = new SeaControler[10, 10];
    
    public GameObject[] shipPrefabs;
    public GameObject[] shipObjs = new GameObject[10];
    int shipCount;
    public Ship[] ships = new Ship[10];

    //direction info
    private const int EAST = 1;
    private const int WEST = 3;
    private const int SOUTH = 2;
    private const int NORTH = 0;

    //result
    int getGold;
    string winner;

    /*
     * 
     * 배 열 대의 정보 저장 필요... gameobject?
     * ai 배 생성
     *  0 ~ 4 : 유저 배 정보 ship list, life도 전달
     *  5 ~ 9 : ai 배. 여기서 생성
     * 배 배치
     * 배 정보 -> ai, user controler 넘김
     * 
     */

    //life
    public int userLife;
    public int startUserLife;
    public int aiLife;
    
    
    //occupied map
    public static int[,] userMap = new int[10, 10];
    public static int[,] aiMap = new int[10, 10];
    

    // Use this for initialization
    void Start () {
        firstHit = true;
        shipCount = 0;
        turn = 0;
        userLife = 0;
        aiLife = 0;
        
        //격자 생성
        Vector3 userzero = new Vector3(1, 0, -5);
        Vector3 aizero = new Vector3(-11, 0, -5);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                //userGrid[i, j] = (GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity);
                //aiGrid[i, j] = (GameObject)Instantiate(tilePrefab, aizero, Quaternion.identity);

                userGridCtrl[i, j] = ((GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity)).GetComponent<SeaControler>();
                aiGridCtrl[i, j] = ((GameObject)Instantiate(tilePrefab, aizero, Quaternion.identity)).GetComponent<SeaControler>();

                //ai 격자 전체에 안개 씌우기
                aiGridCtrl[i, j].fogOn();

                userzero.z++;
                aizero.z++;
            }
            userzero.z = -5;
            aizero.z = -5;
            userzero.x++;
            aizero.x++;
        }

        //select ship number
        int[] shipID = new int[10];


        //get user ships
        for (int i = 0; i < 5; i++)
        {
            //get gameobject ship from user manager
            ShipInfo userShip = new ShipInfo(0);
            userShip = UserManager.userShips[i];
            
            ships[i] = new Ship();
            //set id
            ships[i].shipID = userShip.shipNum;
            //get size - set life
            //userLife += ships[i].shipID / 10;
            userLife = PlayerPrefs.GetInt("userLife");
            //save userLife when start
            startUserLife = userLife;
            //set occpied value
            if (ships[i].shipID % 10 == 1)
            {
                ships[i].occ = 2;   //두 번 맞는 애
            }
            else
            {
                ships[i].occ = 1;
            }
            //set position
            ships[i].x = userShip.x;
            ships[i].y = userShip.y;
            ships[i].direction = userShip.direction;
            //create ship
            createUserShip(ships[i]);
            setUserOccupied(ships[i]);
        }

        //select ai ships
        shipID[5] = 13;
        shipID[6] = 12;
        shipID[7] = 12;
        shipID[8] = 12;
        shipID[9] = 12;

        //ships - initialize
        for (int s = 5; s < 10; s++)
        {
            ships[s] = new Ship();
            //set id
            ships[s].shipID = shipID[s];
            //get size - set life
            if (ships[s].shipID % 10 == 1)
            {
                aiLife += (ships[s].shipID / 10);
                aiLife += (ships[s].shipID / 10);
            }
            else
            {
                aiLife += (ships[s].shipID / 10);
            }
            //set occpied value
            if (ships[s].shipID % 10 == 1)
            {
                ships[s].occ = 2;   //두 번 맞는 애
            }
            else
            {
                ships[s].occ = 1;
            }
            //select random ai ships location
            location(ships[s]);
        }

        //set map
        //for (int i = 0; i < 10; i++) {
        //    for (int j = 0; j < 10; j++) {
        //        if (getAIOcc(i, j) == 1)
        //        {
        //            aiMap[i, j] = 1;
        //        }
        //        else
        //        {
        //            aiMap[i, j] = 0;
        //        }

        //        if (getUserOcc(i, j) == 1)
        //        {
        //            userMap[i, j] = 1;
        //        }
        //        else
        //        {
        //            userMap[i, j] = 0;
        //        }
        //    }
        //}



    }


    void location(Ship ship)
    {
        //select random location according to each direction
          //check is it occupied?
          //occupied - select again
        do{
            selectRandomLocation(ship);
        } while (checkAIOccpied(ship) == false);

        //not occupied - set occupied value
        setAIOccupied(ship);

        //create ship at the location
        createAIShip(ship);
    }

    void selectRandomLocation(Ship ship)
    {
        //get size info
        int size = ship.shipID / 10;
        //select direction
        int direction = Random.Range(0, 4);

        //x, y point - in Grind
        int Gx = 0;
        int Gy = 0;
        //select point according to direction
        switch (direction) {
            case EAST:
                //x : 0 ~ (10-s), y: -
                Gx = Random.Range(0, 10);
                Gy = Random.Range(0, 11-size);
                break;
            case WEST:
                //x : (s-1) ~ 9, y: -
                Gx = Random.Range(0, 10);
                Gy = Random.Range(size - 1, 10);
                break;
            case SOUTH:
                //x :-, y: 0 ~ (10-s)
                Gx = Random.Range(0, 11 - size);
                Gy = Random.Range(0, 10);
                break;
            case NORTH:
                //x : -, y: (s-1) ~ 0
                Gx = Random.Range(size - 1, 10);
                Gy = Random.Range(0, 10);
                break;
        }
        //set the position info
        ship.x = Gx;
        ship.y = Gy;
        ship.direction = direction;
    }

    bool checkAIOccpied(Ship ship)
    {
        //get ship info
        int size = ship.shipID / 10;
        int dir = ship.direction;
        int x = ship.x;
        int z = ship.y;

        //check occupied according to direction
        int chk = 0;
        switch (dir) {
            case EAST:
                for (chk = 0; chk < size; chk++) {

                    if (getAIOcc(x, z + chk) != 0)

                        return false;
                }
                break;
            case WEST:
                for (chk = 0; chk < size; chk++)
                {

                    if (getAIOcc(x, z-chk) != 0)

                        return false;
                }
                break;
            case SOUTH:
                for (chk = 0; chk < size; chk++)
                {

                    if (getAIOcc(x+chk, z) != 0)

                        return false;
                }
                break;
            case NORTH:
                for (chk = 0; chk < size; chk++)
                {

                    if (getAIOcc(x-chk, z) != 0)

                        return false;
                }
                break;
        }
        return true;
    }

    void createAIShip(Ship ship)
    {
        //get size and func from ship number
        int size = ship.shipID/10;
        int dir = ship.direction;
        int x = ship.x;
        int z = ship.y;

        //prefab location
        Vector3 pos = new Vector3(0,0,0);
        Vector3 rot = new Vector3(0,0,0);

        float s = (float)0.5* (size-1);

        //get sea location
        Transform seaPos = getTransformOfAITile(x, z);
        float realX = seaPos.position.x;
        float realZ = seaPos.position.z;

        switch (dir)
        {
            case EAST:
                //rotate +90, move z + s
                rot.y = 90;
                pos.x = realX;
                pos.z = realZ + s;
                break;
            case WEST:
                //rotate -90, move z - s 
                rot.y = -90;
                pos.x = realX;
                pos.z = realZ - s;
                break;
            case SOUTH:
                //rotate 180, move x + s 
                rot.y = 180;
                pos.x = realX + s;
                pos.z = realZ;
                break;
            case NORTH:
                //rotate 0, move x - s 
                rot.y = 0;
                pos.x = realX - s;
                pos.z = realZ;
                break;
        }

        //locate at x, 0, z
        shipObjs[shipCount++] = (GameObject)Instantiate(shipPrefabs[size-1], pos, Quaternion.Euler(rot));
    }

    void createUserShip(Ship ship)
    {
        //get size and func from ship number
        int size = ship.shipID / 10;
        int dir = ship.direction;
        int x = ship.x;
        int z = ship.y;

        //prefab location
        Vector3 pos = new Vector3(0, 0.5f, 0);
        Vector3 rot = new Vector3(0, 0, 0);

        float s = (float)0.5 * (size - 1);

        //get sea location
        Transform seaPos = getTransformOfUserTile(x, z);
        float realX = seaPos.position.x;
        float realZ = seaPos.position.z;

        switch (dir)
        {
            case EAST:
                //rotate +90, move z + s
                rot.y = 90;
                pos.x = realX;
                pos.z = realZ + s;
                break;
            case WEST:
                //rotate -90, move z - s 
                rot.y = -90;
                pos.x = realX;
                pos.z = realZ - s;
                break;
            case SOUTH:
                //rotate 180, move x + s 
                rot.y = 180;
                pos.x = realX + s;
                pos.z = realZ;
                break;
            case NORTH:
                //rotate 0, move x - s 
                rot.y = 0;
                pos.x = realX - s;
                pos.z = realZ;
                break;
        }

        //locate at x, 0, z
        shipObjs[shipCount++] = (GameObject)Instantiate(shipPrefabs[size - 1], pos, Quaternion.Euler(rot));
        
    }

    //set ship with direction  - occupied
    void setUserOccupied(Ship ship)
    {
        //get info from ship
        int size = ship.shipID / 10;
        int direction = ship.direction;
        int GridX = ship.x;
        int GridY = ship.y;

        int chk = 0;
        switch (direction)
        {
            case EAST:
                //increase y
                for (chk = 0; chk < size; chk++)
                {
                    setUserOcc(GridX, GridY + chk, ship.occ);
                }
                break;

            case WEST:
                //decrease y
                for (chk = 0; chk < size; chk++)
                {
                    setUserOcc(GridX, GridY - chk, ship.occ);
                }
                break;

            case SOUTH:
                //increase x
                for (chk = 0; chk < size; chk++)
                {
                    setUserOcc(GridX + chk, GridY, ship.occ);

                }
                break;

            case NORTH:
                //decrease x
                for (chk = 0; chk < size; chk++)
                {
                    setUserOcc(GridX - chk, GridY, ship.occ);
                }
                break;
        }
    }

    //set ship with direction  - occupied
    void setAIOccupied(Ship ship)
    {
        //get info from ship
        int size = ship.shipID / 10;
        int direction = ship.direction;
        int GridX = ship.x;
        int GridY = ship.y;

        int chk = 0;
        switch (direction)
        {
            case EAST:
                //increase y
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX, GridY + chk, ship.occ);
                }
                break;

            case WEST:
                //decrease y
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX, GridY - chk, ship.occ);
                }
                break;

            case SOUTH:
                //increase x
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX + chk, GridY, ship.occ);

                }
                break;

            case NORTH:
                //decrease x
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX - chk, GridY, ship.occ);
                }
                break;
        }
    }


    //get turn
    public int GetTurn()
    {
        return turn;
    }

    //get user life
    public int GetUserLife()
    {
        return userLife;
    }

    //get ai life
    public int GetAILife()
    {
        return aiLife;
    }

    //decrease user life
    public void minusUserLife()
    {
        userLife = userLife - 1;
    }

    //decrease ai life
    public void minusAILife()
    {
        aiLife = aiLife - 1;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Escape button codes
            SceneManager.LoadScene("Title");
        }

        switch (turn)
        {
            case 0: //user turn
                break;
            case 1: //ai turn
               // turn = 0;
                break;
            case 2: //user win
                //게임이 끝나면 골드 획득
                //ceck rest occupied - calculate gold
                getGold = saveGold();
                winner = "user";
                PlayerPrefs.SetInt("getGold", getGold);
                PlayerPrefs.SetString("winner", winner);
                print(".getGold " + getGold + " .winner" + winner);
                SceneManager.LoadScene("FinishGame");
                break;
            case 3: //ai win
                //game over message
                //go to title
                winner = "ai";
                PlayerPrefs.SetInt("getGold", 0);
                PlayerPrefs.SetString("winner", winner);
                print(".getGold " + getGold + " .winner" + winner);
                SceneManager.LoadScene("FinishGame");
                break;
            default:
                break;
        }
    }

    
    int saveGold()
    {
        int gold = calculGold();
        return gold;
    }

    //check rest occupied -for  calculate gold
    int calculGold()
    {
        //total grid : 100 - (occupied when start - currect occupied)
        int gold = 100 - (startUserLife - userLife);
        print("gold = 100 - " + startUserLife  + " - " + userLife + " : " + gold);
        int bonus = 100;

        //gold bonus ship
        // +50%
        for (int i = 0; i < 5; i++) {
            if (ships[i].shipID%10 == 4)
            {
                print("bonus");
                bonus = bonus + 100;
            }
        }

        gold = gold * (bonus / 100);
        return gold;
    }

    /*
     * 유저 격자의 x, y 좌표의 Transform을 반환한다.
     * 반환된 Transform을 탄환 발사 시 이용할 수 있다.
     */
    public Transform getTransformOfUserTile(int x, int y)
    {
        //return userGrid[x, y].transform;
        return userGridCtrl[x, y].gameObject.transform;
    }

    /*
    * 인공지능 격자의 x, y 좌표의 Transform을 반환한다.
     */
    public Transform getTransformOfAITile(int x, int y)
    {
        //return aiGrid[x, y].transform;
        return aiGridCtrl[x, y].gameObject.transform;
    }

    //return occupied of ai grid
    public int getAIOcc(int x, int y)
    {
        //SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        //return aiGridCtrl[x, y].getOcc();
        //return sea.getOcc();
        return aiMap[x, y];
    }

    //return occupied of user grid
    public int getUserOcc(int x, int y)
    {
        //SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        //return userGridCtrl[x, y].getOcc();
        //return sea.getOcc();
        return userMap[x, y];
    }

    //setting occupied of ai grid
    public void setAIOcc(int x, int y, int occ)
    {
        //SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        //Debug.Log("set : " + x + "," + y);
        //sea.setOcc(occ);
        //aiGridCtrl[x, y].setOcc(occ);
        aiMap[x, y] = occ;
    }

    public void setUserOcc(int x, int y, int occ)
    {
        //SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        //Debug.Log("set : " + x + "," + y);
        //sea.setOcc(occ);
        //userGridCtrl[x, y].setOcc(occ);
        userMap[x, y] = occ;
    }

    //get occupied value from each map
    //public int getOccFromMap(float x, float y)
    //{
    //    int gridX, gridY;
    //    //change to grid x y
    //    if (x > 0)//user grid
    //    {
    //        /*
    //         * x : 1~10
    //         * z : -5~4
    //         */
    //        gridX = (int)x - 1;
    //        gridY = (int)y + 5;
    //        Debug.Log("USER, getOccFromMap : " + gridX + " " + gridY + " " + userMap[gridX, gridY]);
    //        return userMap[gridX, gridY];

    //    }
    //    else //ai grid
    //    {
    //        /*
    //         * x : -11~-2
    //         * z : -5~4
    //         */
    //        gridX = (int)x + 11;
    //        gridY = (int)y + 5;
    //        Debug.Log("aI, getOccFromMap : " + gridX + " " + gridY + " " + aiMap[gridX, gridY]);
    //        return aiMap[gridX, gridY];

    //    }
    //}


    //public int getOccFromUserMap(int x, int y)
    //{
    //    //Debug.Log(x + " real " + y + " " + userMap[x, y]);
    //    return userMap[x, y] ;
    //}

    //public int getOccFromAIMap(int x, int y)
    //{
    //    return aiMap[x, y];
    //}

    public void decUserOcc(int x, int y)
    {
        userMap[x, y]--;

    }

    public void decAiOcc(int x, int y)
    {
        aiMap[x, y]--;
    }

    //check all parts of ship is hitted
    public void checkShipHitted(Vector3 position)
    {
        int hitted = 5;
        //find which ship is hitted
        for (int i = 5; i < 10; i++)
        {
            if (shipObjs[i].transform.position == position)
            {
                hitted = i;
                break;
            }
        }
        //print(shipObjs[hitted].transform.position);
        
        int gridX = ships[hitted].x;
        int gridY = ships[hitted].y;

        //print(gridX + " " + gridY);

        //gridX = (int)shipObjs[hitted].transform.position.x + 11;
        //gridY = (int)shipObjs[hitted].transform.position.z + 5;

        //print(gridX + " " + gridY);

        //check all parts
        //if all parts is hitted, remove fogs
        int chk = 0;
        int size = ships[hitted].shipID / 10;
        bool allHitted = true;
        switch (ships[hitted].direction)
        {
            case EAST:
                //increase x
                for (chk = 0; chk < size; chk++)
                {
                    if (aiMap[gridX, gridY + chk] > 0)
                    {
                        allHitted = false;  //not all parts
                        break;
                    }
                }
                //all parts hitted, remove fogs
                if (allHitted == true)
                {
                    for (chk = 0; chk < size; chk++)
                    {
                        aiGridCtrl[gridX, gridY + chk].fogOff();
                    }
                }
                break;

            case WEST:
                //decrease x
                for (chk = 0; chk < size; chk++)
                {
                    if (aiMap[gridX, gridY - chk] > 0)
                    {
                        allHitted = false;  //not all parts
                        break;
                    }
                }
                //all parts hitted, remove fogs
                if (allHitted == true)
                {
                    for (chk = 0; chk < size; chk++)
                    {
                        aiGridCtrl[gridX, gridY - chk].fogOff();
                    }
                }
                break;

            case SOUTH:
                //increase y
                for (chk = 0; chk < size; chk++)
                {
                    if (aiMap[gridX + chk, gridY] > 0)
                    {
                        allHitted = false;  //not all parts
                        break;
                    }
                }
                //all parts hitted, remove fogs
                if (allHitted == true)
                {
                    for (chk = 0; chk < size; chk++)
                    {
                        print("all ai grid fog off");
                        aiGridCtrl[gridX + chk, gridY].fogOff();
                    }
                }
                break;

            case NORTH:
                //decrease y
                for (chk = 0; chk < size; chk++)
                {
                    if (aiMap[gridX - chk, gridY] > 0)
                    {
                        allHitted = false;  //not all parts
                        break;
                    }
                }
                //all parts hitted, remove fogs
                if (allHitted == true)
                {
                    for (chk = 0; chk < size; chk++)
                    {
                        aiGridCtrl[gridX - chk, gridY].fogOff();
                    }
                }
                break;
        }
    }
}
