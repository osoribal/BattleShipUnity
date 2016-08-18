using UnityEngine;
using System.Collections;

public class UserControler : MonoBehaviour {
    int turn;
    Vector3 beforePosition;
    Quaternion beforeLookAt;
    GameControler gc;
    public GameObject bulletPrefab;
    public GameObject arrowPrefab;  //맞을 지점을 표시할 프리팹

    // Use this for initialization
    void Start () {
        turn = 0;
        gc = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == 0)
        {
            //카메라 정보 저장
            beforePosition = Camera.main.transform.position;
            beforeLookAt = Camera.main.transform.rotation;

            //카메라 시점 이동
            //Camera.main.transform.position = new Vector3(11, 10, 0);
            //Camera.main.transform.LookAt(new Vector3(-11, -5, 0));

            gc.turn = -1;   //block
            
        }

        //aim
        if (gc.turn == -1 && Input.GetButtonDown("Fire1"))
        {
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

        }

        //카메라 원위치
        Camera.main.transform.position = beforePosition;
        Camera.main.transform.rotation = beforeLookAt;

    }

    public void changeTurn()
    {
        turn = (turn + 1) % 5;
    }
}
