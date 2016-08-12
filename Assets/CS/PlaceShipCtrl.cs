using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaceShipCtrl : MonoBehaviour {
    GameObject[,] userGrid = new GameObject[10, 10];
    GameObject[] ships = new GameObject[5];
    public GameObject tilePrefab;
    public GameObject[] shipPrefab; //배의 프리팹 5종 저장

    //direction info
    private const int EAST = 0;
    private const int WEST = 1;
    private const int SOUTH = 2;
    private const int NORTH = 3;

    public void OnBackClicked()
    {
        SceneManager.LoadScene("SelectShip");
    }
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
                userGrid[i, j] = (GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity);
                userGrid[i, j].GetComponent<SeaControler>().x = i;
                userGrid[i, j].GetComponent<SeaControler>().y = j;
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
            int shipLength = PlayerPrefs.GetInt("ship" + i) / 10;

            //배의 기본 위치 설정
            Vector3 position = userGrid[0, i].transform.position;
            position.x += 0.5f * (shipLength - 1);  //배의 머리가 해당 타일에 위치하도록 배를 이동
            setOccupied(shipLength, SOUTH, 0, i, 1);

            //배 오브젝트 생성
            ships[i] = (GameObject)Instantiate(
                        shipPrefab[shipLength - 1],
                        position,
                        Quaternion.identity);

            //ship에 기본 위치 정보 저장
            Ship ctrl = ships[i].GetComponent<Ship>();
            ctrl.placeCtrl = this;
            ctrl.shipID = PlayerPrefs.GetInt("ship" + i);
            ctrl.x = 0;
            ctrl.y = i;
            ctrl.direction = 2;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray;
            RaycastHit rayHit;
            float rayLength = 100f;

            //어디를 터치했느냐
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out rayHit, rayLength))
            {
                //배를 터치했다면
                if (rayHit.transform.gameObject.tag == "Ship")
                {
                    //배의 위치 이동 및 회전을 담당하는 코루틴 호출
                    StartCoroutine(rayHit.transform.gameObject.GetComponent<Ship>().move());
                }
            }
            
        }

        /**** touch event : http://mrhook.co.kr/208 *****/
    }

    public bool isOccupied(int length, int dir, int x, int y)
    {
        switch (dir)
        {
            case EAST:
                for (int i = y; i < y + length; i++)
                {
                    if (userGrid[x, i].GetComponent<SeaControler>().occpied == 1)
                        return true;
                }
                break;
            case WEST:
                for (int i = y; i > y - length; i--)
                {
                    if (userGrid[x, i].GetComponent<SeaControler>().occpied == 1)
                        return true;
                }
                break;
            case SOUTH:
                for (int i = x; i < x + length; i++)
                {
                    print(i + " , " + y + " : " + userGrid[i, y].GetComponent<SeaControler>().occpied);
                    if (userGrid[i, y].GetComponent<SeaControler>().occpied == 1)
                        return true;
                }
                break;
            case NORTH:
                for (int i = x; i > x - length; i--)
                {
                    if (userGrid[i, y].GetComponent<SeaControler>().occpied == 1)
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
            case EAST:
                for (int i = y; i < y + length; i++)
                {
                    userGrid[x, i].GetComponent<SeaControler>().occpied = value;
                }
                break;
            case WEST:
                for (int i = y; i > y - length; i--)
                {
                    userGrid[x, i].GetComponent<SeaControler>().occpied = value;
                }
                break;
            case SOUTH:
                for (int i = x; i < x + length; i++)
                {
                    userGrid[i, y].GetComponent<SeaControler>().occpied = value;
                    print(i + " , " + y + " : " + userGrid[i, y].GetComponent<SeaControler>().occpied);
                }
                break;
            case NORTH:
                for (int i = x; i > x - length; i--)
                {
                    userGrid[i, y].GetComponent<SeaControler>().occpied = value;
                }
                break;
        }
    }
}
