using UnityEngine;
using System.Collections;

public class GameControler : MonoBehaviour {
    public int turn;
    public GameObject tilePrefab;
    public SeaControler[,] userGrid = new SeaControler[10,10];
    public SeaControler[,] aiGrid = new SeaControler[10, 10];

    public GameObject[] shipPrefabs;
    Ship[] ships = new Ship[10];

    //direction info
    private const int EAST = 0;
    private const int WEST = 1;
    private const int SOUTH = 2;
    private const int NORTH = 3;
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
    public int aiLife;
    
    // Use this for initialization
    void Start () {
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
                //userGrid[i, j] = (GameObject)Instantiate( tilePrefab, userzero, Quaternion.identity);
                //aiGrid[i, j] = (GameObject)Instantiate(tilePrefab, aizero, Quaternion.identity);
                GameObject tile = (GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity);
                userGrid[i, j] = tile.GetComponent<SeaControler>();
                aiGrid[i, j] = tile.GetComponent<SeaControler>();

                //ai 격자 전체에 안개 씌우기
                SeaControler fg = aiGrid[i, j].GetComponent<SeaControler>();
                fg.fogOn();

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
            userLife = 1;
            //set occpied value
            ships[i].occ = 1;
            //set position
            ships[i].x = userShip.x;
            ships[i].y = userShip.y;
            ships[i].direction = userShip.direction;
            //create ship
            createUserShip(ships[i]);
        }

        //select ai ships
        shipID[5] = 33;
        shipID[6] = 21;
        shipID[7] = 31;
        shipID[8] = 41;
        shipID[9] = 51;

        //ships - initialize
        for (int s = 5; s < 10; s++)
        {
            ships[s] = new Ship();
            //set id
            ships[s].shipID = shipID[s];
            //get size - set life
            aiLife += ships[s].shipID / 10;
            //set occpied value
            ships[s].occ = 1;
            //select random ai ships location
            location(ships[s]);
        }
        
    }


    void location(Ship ship)
    {
        //select random location according to each direction
          //check is it occupied?
          //occupied - select again
        do{
            selectRandomLocation(ship);
        } while (checkOccpied(ship) == false);

        //not occupied - set occupied value
        setOccupied(ship);

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
                Gx = Random.Range(0, 11-size);
                Gy = Random.Range(0, 10);
                break;
            case WEST:
                //x : (s-1) ~ 9, y: -
                Gx = Random.Range(size-1, 10);
                Gy = Random.Range(0, 10);
                break;
            case SOUTH:
                //x :-, y: 0 ~ (10-s)
                Gx = Random.Range(0, 10);
                Gy = Random.Range(0, 11-size);
                break;
            case NORTH:
                //x : -, y: (s-1) ~ 0
                Gx = Random.Range(0, 10);
                Gy = Random.Range(size-1, 10);
                break;
        }
        //set the position info
        ship.x = Gx;
        ship.y = Gy;
        ship.direction = direction;
        Debug.Log(" randomselecLoc " + ship.shipID + ":" + Gx + " " + Gy + " " + direction);
    }

    bool checkOccpied(Ship ship)
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
                    if (getGridOcc(x + chk, z) != 0)
                        return false;
                }
                break;
            case WEST:
                for (chk = 0; chk < size; chk++)
                {
                    if (getGridOcc(x-chk, z) != 0)
                        return false;
                }
                break;
            case SOUTH:
                for (chk = 0; chk < size; chk++)
                {
                    if (getGridOcc(x, z+chk) != 0)
                        return false;
                }
                break;
            case NORTH:
                for (chk = 0; chk < size; chk++)
                {
                    if (getGridOcc(x, z-chk) != 0)
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
        int x = ship.y;
        int z = ship.x;

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
        GameObject newShip = (GameObject)Instantiate(shipPrefabs[size-1], pos, Quaternion.Euler(rot));
    }

    void createUserShip(Ship ship)
    {
        //get size and func from ship number
        int size = ship.shipID / 10;
        int dir = ship.direction;
        int x = ship.y;
        int z = ship.x;

        //prefab location
        Vector3 pos = new Vector3(0, 0, 0);
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
        GameObject newShip = (GameObject)Instantiate(shipPrefabs[size - 1], pos, Quaternion.Euler(rot));
    }

    //set ship with direction  - occupied
    void setOccupied(Ship ship)
    {
        //get info from ship
        int size = ship.shipID / 10;
        int direction = ship.direction;
        int GridX = ship.x;
        int GridY = ship.y;

        int chk = 0;
        Debug.Log(" setOcc " + ship.shipID + ":" + GridX + " " + GridY + " " + direction);
        switch (direction)
        {
            case EAST:
                //increase x
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX + chk, GridY, ship.occ);
                }
                break;

            case WEST:
                //decrease x
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX - chk, GridY, ship.occ);
                }
                break;

            case SOUTH:
                //increase y
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX, GridY + chk, ship.occ);
                }
                break;

            case NORTH:
                //decrease y
                for (chk = 0; chk < size; chk++)
                {
                    setAIOcc(GridX, GridY - chk, ship.occ);
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
        switch(turn)
        {
            case 0: //user turn
                break;
            case 1: //ai turn
               // turn = 0;
                break;
            case 2: //user win
                //게임이 끝나면 골드 획득
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

    /*
    * 인공지능 격자의 x, y 좌표의 Transform을 반환한다.
     */
    public Transform getTransformOfAITile(int x, int y)
    {
        return aiGrid[x, y].transform;
    }

    //return occupied of grid
    public int getGridOcc(int x, int y)
    {
        SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        Debug.Log("getGridOcc : " + x + "," + y + " " + sea.getOcc());
        return sea.getOcc();
    }

    //setting occupied of ai grid
    public void setAIOcc(int x, int y, int occ)
    {
        SeaControler sea = aiGrid[x, y].GetComponent<SeaControler>();
        Debug.Log("set : " + x + "," + y);
        sea.setOcc(occ);
    }

}
