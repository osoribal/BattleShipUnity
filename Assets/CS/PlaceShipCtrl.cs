using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlaceShipCtrl : MonoBehaviour {
    int[,] occupied = new int[10, 10];
    GameObject[] ships = new GameObject[5];
    public GameObject tilePrefab;
    public GameObject[] shipPrefab; //배의 프리팹 5종 저장
    public Button rotButton;

    //direction info
    private const int EAST = 1;
    private const int WEST = 3;
    private const int SOUTH = 2;
    private const int NORTH = 0;
    
    public void OnNextClicked()
    {
        for(int i = 0; i < 5; i++)
        {
            Ship ctrl = ships[i].GetComponent<Ship>();
            UserManager.userShips[i].x = ctrl.x;
            UserManager.userShips[i].y = ctrl.y;
            UserManager.userShips[i].direction = ctrl.direction;
        }
        SceneManager.LoadScene("Battle");
    }

    // Use this for initialization
    void Start()
    {
        //격자 생성
        Vector3 userzero = new Vector3(0, 0, 0);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject tile = (GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity);
                userzero.z++;
            }
            userzero.z = 0;
            userzero.x++;
        }

        //기본 위치에 user ships 오브젝트 생성
        //정확한 위치, 방향은 ship.cs에서 진행
        for (int i = 0; i < 5; i++)
        {
            //배의 고유 번호로부터 배 길이 추출
            int shipLength = UserManager.userShips[i].shipNum / 10;   //UserManager.userShips[i]

            //배의 기본 위치 설정
            Vector3 position = new Vector3(0, 0, i) ;
            position.x += 0.5f * (shipLength - 1);  //배의 머리가 해당 타일에 위치하도록 배를 이동
            setOccupied(shipLength, SOUTH, 0, i, 1);

            //배 오브젝트 생성

            ships[i] = (GameObject)Instantiate(
                        shipPrefab[shipLength - 1],
                        position,
                        Quaternion.identity);

            //ship에 기본 정보 저장
            Ship ctrl = ships[i].GetComponent<Ship>();
            ctrl.placeCtrl = this;
            ctrl.shipID = UserManager.userShips[i].shipNum;
            ctrl.x = 0;
            ctrl.y = i;
            ctrl.direction = SOUTH;

        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Escape button codes
            SceneManager.LoadScene("SelectShip");
        }
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray;
            RaycastHit rayHit;
            float rayLength = 100f;

            //어디를 터치했느냐
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out rayHit, rayLength))
            {
                print("touch");
                //배를 터치했다면
                if (rayHit.transform.gameObject.tag == "Ship")
                {
                    print("touch ship");
                    rotButton.onClick.RemoveAllListeners();
                    rotButton.onClick.AddListener(() => rayHit.transform.gameObject.GetComponent<Ship>().rotate());
                    //배의 위치 이동 및 회전을 담당하는 코루틴 호출
                    StartCoroutine(rayHit.transform.gameObject.GetComponent<Ship>().move());
                }
            }
            
        }

        /**** touch event : http://mrhook.co.kr/208 *****/
    }

    //x, y에 배 머리가 오도록 배치할 좌표 반환
    public Vector3 place(int length, int dir, int x, int y)
    {
        float dot5 = 0.5f * (length - 1);
        switch (dir)
        {
            case EAST:  //y+
                return new Vector3(x, 0, y + dot5);
            case WEST:  //y-
                return new Vector3(x, 0, y - dot5);
            case SOUTH: //x+
                return new Vector3(x + dot5, 0, y);
            case NORTH: //x-
                return new Vector3(x - dot5, 0, y);
        }
        return new Vector3(0, 0, 0);
    }

    //x, y 위치에 길이가 length이고 방향이 dir인 배를 놓을 때
    //배가 격자 내에 들어오면 true.
    public bool inGrid(int length, int dir, int x, int y)
    {
        switch (dir)
        {
            case EAST:  //y+
                if (y >= 0 && y <= 10 - length && x >=0 && x <= 9)
                    return true;
            break;
            case WEST:  //y-
                if (y >= length - 1 && y < 10 && x >= 0 && x <= 9)
                    return true;
            break;
            case SOUTH: //x+
                if (x >= 0 && x <= 10 - length && y >= 0 && y <= 9)
                    return true;
            break;
            case NORTH: //x-
                if (x >= length - 1 && x < 10 && y >= 0 && y <= 9)
                    return true;
            break;
        }
        return false;
    }

    public bool isOccupied(int length, int dir, int x, int y)
    {
        switch (dir)
        {
            case EAST:  //y+
                for (int i = y; i < y + length; i++)
                {
                    if (occupied[x, i] == 1)
                        return true;
                }
                break;
            case WEST:  //y-
                for (int i = y; i > y - length; i--)
                {
                    if (occupied[x, i] == 1)
                        return true;
                }
                break;
            case SOUTH: //x+
                for (int i = x; i < x + length; i++)
                {
                    if (occupied[i, y] == 1)
                        return true;
                }
                break;
            case NORTH: //x-
                for (int i = x; i > x - length; i--)
                {
                    if (occupied[i, y] == 1)
                        return true;
                }
                break;
        }
        return false;
    }

    public void setOccupied(int length, int dir, int x, int y, int value)
    {
        switch (dir)
        {
            case EAST:  //y+
                for (int i = y; i < y + length; i++)
                {
                    occupied[x, i] = value;
                }
                break;
            case WEST:  //y-
                for (int i = y; i > y - length; i--)
                {
                    occupied[x, i] = value;
                }
                break;
            case SOUTH: //x+
                for (int i = x; i < x + length; i++)
                {
                    occupied[i, y] = value;
                }
                break;
            case NORTH: //x-
                for (int i = x; i > x - length; i--)
                {
                    occupied[i, y] = value;
                }
                break;
        }
    }
}
