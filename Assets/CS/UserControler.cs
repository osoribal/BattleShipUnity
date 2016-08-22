using UnityEngine;
using System.Collections;

public class UserControler : MonoBehaviour {
    int turn;
    GameControler gc;
    public GameObject bulletPrefab;
    public GameObject arrowPrefab;  //맞을 지점을 표시할 프리팹
    bool firstHit;

    // Use this for initialization
    void Start () {
        turn = 0;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
        firstHit = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == 0 && Input.GetButtonDown("Fire1"))
        {
            gc.turn = -1;   //block
            StartCoroutine("shot");

        }
    }
    
    public IEnumerator shot()
    {
        //드래그 중
        GameObject arrow = null;    //맞을 지점을 표시할 오브젝트
        Vector3 scrSpace = Camera.main.WorldToScreenPoint(transform.position);
        while (Input.GetMouseButton(0))
        {
            /******Aim******/
            Vector3 curScreenSpace = new Vector3(Input.mousePosition.x, Input.mousePosition.y, scrSpace.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenSpace);
            //맞을 지점의 좌표
            Vector3 position = new Vector3((int)curPosition.x, 2.0f, (int)curPosition.z);
            //ai 격자 내에만 가능하도록 범위 제한
            if ( position.x >= -11 && position.x < -1 && position.z >= -5 && position.z < 5)
            {
                if(arrow != null)
                {
                    //기존 지점의 오브젝트 삭제
                    Destroy(arrow);
                }
                //좌표에 arrow 오브젝트 생성
                arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);
            }

            yield return null;

        }

        if (arrow != null)
        {
            //탄환 생성
            GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(2, 1, 0), Quaternion.identity);
            //탄환 코드에 변수값 전달 -> 탄환 스스로 발사
            Bullet bc = bullet.GetComponent<Bullet>();
            Vector3 buf = gc.shipObjs[turn].transform.position;
            buf.y += 1;
            bc.from = buf;
            bc.to = arrow.transform.position;
            //두 발 쏘는 특수기능 처리
            if (gc.ships[turn].shipID % 10 == 3)   //두 발 쏘기
            {
                if (firstHit == true)
                {
                    firstHit = false;
                    bc.turnChange = false;
                }
                else
                {
                    firstHit = true;
                    bc.turnChange = true;
                    changeTurn();
                }
            }
            else
            {
                bc.turnChange = true;
                changeTurn();
            }
        }

    }

    void changeTurn()
    {
        turn = (turn + 1) % 5;
    }
}
