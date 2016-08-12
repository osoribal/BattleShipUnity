using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Ship : MonoBehaviour
{
    public int shipID;      //ship number
    public int x, y;        //배 머리 위치
    public int direction;   //배 방향
    public GameObject rotateButPrefab;
    GameObject rotateBut;
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
        Ray ray;
        RaycastHit rayHit;
        float rayLength = 100f;
        Vector3 pos;
        while (true)    //드래그 하는 동안
        {
            if (Input.GetButtonUp("Fire1")) //드래그 끝
            {
                break;
            }

            /******move******/
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out rayHit, rayLength))
            {
                pos = rayHit.transform.position;
                //x, y 설정
                x = (int)pos.x;
                y = (int)pos.z;
                this.gameObject.transform.position = pos;

            }
            yield return null;
        }

        //드래그 끝
        /******Rotate******/
        pos = transform.position;
        pos.y = 1;
        rotateBut = (GameObject)Instantiate(rotateButPrefab,
            pos,
            Quaternion.AngleAxis(90.0f, new Vector3(1, 0, 0)));
        Button but = rotateBut.GetComponentInChildren<Button>();
        but.onClick.AddListener(() => rotate());
    }

    void rotate()
    {
        direction = (++direction) % 4;
        transform.rotation = Quaternion.AngleAxis(direction * 90.0f, Vector3.up);
        Destroy(rotateBut);
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
