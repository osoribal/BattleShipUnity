using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Ship : MonoBehaviour
{
    //direction info
    private const int EAST = 1;
    private const int WEST = 3;
    private const int SOUTH = 2;
    private const int NORTH = 0;

    public int shipID;      //ship number
    public int x, y;        //배 머리 위치
    public int direction;   //배 방향
    public PlaceShipCtrl placeCtrl;
    public GameObject rotateButPrefab;
    GameObject butCanvas;
    public int occ;  //occupied number
    /*
     * switch 문 2개 구현 : prefab, 특수능력
     * prefab은 swith문 안만들어도..?
     * 
     */


    //turn flags
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;
    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    // Use this for initialization
    void Start()
    {
        //size
        int size = shipID / 10;
        switch (size)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }

        //fuction
        int func = shipID % 10;
        switch (func)
        {
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator move()
    {
        //reset occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 0);

        //드래그 중
        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z));
        while (Input.GetMouseButton(0))
        {
            /*****move*****/
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace) + offset;

            //curPosition에서 배가 격자를 벗어나지 않는지 검사
            if (placeCtrl.inGrid(shipID / 10, direction, (int)curPosition.x, (int)curPosition.z))
            {
                //배의 위치가 중복되지 않도록 occupied 검사
                if (!placeCtrl.isOccupied(shipID / 10, direction, (int)curPosition.x, (int)curPosition.z))
                {
                    //배의 위치 변경. occupied 변경 
                    x = (int)curPosition.x;
                    y = (int)curPosition.z;
                    print(x + " " + y);
                    transform.position = placeCtrl.place(shipID / 10, direction, (int)curPosition.x, (int)curPosition.z);
                }

            }

            yield return null;
        }

        //드래그 끝
        //set occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 1);

        //Rotate 버튼 생성
        butCanvas = (GameObject)Instantiate(rotateButPrefab,
            new Vector3(transform.position.x, 1, transform.position.z),
            Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0)));
        //리스너 장착
        Button[] buts = butCanvas.GetComponentsInChildren<Button>();
        buts[0].onClick.AddListener(() => rotate());
        buts[1].onClick.AddListener(() => cancle());
    }

    //rotate 버튼 리스너
    void rotate()
    {
        //reset occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 0);
        
        //curPosition에서 배가 격자를 벗어나지 않는지 검사
        if (placeCtrl.inGrid(shipID / 10, (direction + 1) % 4, x, y))
        {
            //배의 위치가 중복되지 않도록 occupied 검사
            if (!placeCtrl.isOccupied(shipID / 10, (direction + 1) % 4, x, y))
            {
                //방향 변수 변경
                direction = (direction + 1) % 4;
                //회전
                transform.rotation = Quaternion.AngleAxis(direction * 90.0f, Vector3.up);
                //배의 위치 변경
                transform.position = placeCtrl.place(shipID / 10, direction, x, y);
            }

        }
        //set occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 1);

    }

    public void cancle()
    {
        Destroy(butCanvas);
    }

    void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.tag == "Bullet") Destroy(other.gameObject);
        //notify hit
        //OnNotify();
    }

    /*   void OnNotify()
       {
           //notify to game controller
           gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
           int whoseTurn = gameController.GetTurn();

           switch (whoseTurn)
           {
               case USER_TURN:
                   break;
               case AI_TURN:
                   break;
               case USER_BLOCK:
                   gameController.turn = USER_TURN;
                   break;
               case AI_BLOCK:
                   gameController.turn = AI_TURN;
                   break;

           }
       }*/


}
