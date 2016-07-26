using UnityEngine;
using System.Collections;

public class UserControler : MonoBehaviour {
    int turn;
    public GameControler gc;
    public Camera camera;
    public GameObject bulletPrefab;
    public GameObject arrowPrefab;  //맞을 지점을 표시할 프리팹
    public float throwPower;

    // Use this for initialization
    void Start () {
        this.turn = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == 0)
        {
            gc.turn = -1;   //block
            StartCoroutine("shot");
        }
	}

    public IEnumerator shot()
    {
        //카메라 정보 저장
        Vector3 beforePosition = camera.transform.position;
        Quaternion beforeLookAt = camera.transform.rotation;

        //카메라 시점 이동
        //camera.transform.position = new Vector3(2, 2, 0);
        //camera.transform.LookAt(new Vector3(-50, 1, 0));
        
        
        Ray ray;
        RaycastHit rayHit;
        float rayLength = 100f;
        bool isButtonDown = false;
        GameObject arrow = null;    //맞을 지점을 표시할 오브젝트
        while (true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //버튼을 누르고 있는 상태
                isButtonDown = true;
            }
            if (isButtonDown == true)
            {
                
                /******Aim******/
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out rayHit, rayLength))
                {
                    //맞을 지점 표시
                    if (arrow != null)
                    {
                        //기존 지점의 오브젝트 삭제
                        Destroy(arrow);
                    }
                    //맞을 지점의 좌표
                    Vector3 position = rayHit.transform.gameObject.transform.position;
                    position.y = 0.5f;
                    //좌표에 오브젝트 생성
                    arrow = (GameObject)Instantiate(arrowPrefab, position, Quaternion.identity);
                    
                }

                /******Drop******/
                if (Input.GetButtonUp("Fire1"))
                {
                    isButtonDown = false;

                    if (Physics.Raycast(ray, out rayHit, rayLength))
                    {
                        //탄환 생성
                        GameObject bullet = (GameObject)Instantiate( bulletPrefab, new Vector3(2, 1, 0), Quaternion.identity);
                        //탄환 코드에 변수값 전달
                        BulletControler bc = bullet.GetComponent<BulletControler>();
                        bc.from = bullet.transform;
                        bc.to = rayHit.transform.gameObject.transform;
                    }

                    //카메라 원위치
                    camera.transform.position = beforePosition;
                    camera.transform.rotation = beforeLookAt;
                    gc.turn = 1;
                    break;
                }
            }

            yield return null;
        }

    }
}
