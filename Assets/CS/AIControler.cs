using UnityEngine;
using System.Collections;

public class AIControler : MonoBehaviour {

    //turn
    int turn;

    //shot bullet
    public GameControler gc;
    public Camera camera;
    public GameObject bulletPrefab;
    public GameObject arrowPrefab;  //맞을 지점을 표시할 프리팹

    //ai turn
    public const int AI_TURN = 1;
    public const int USER_TURN = 0;
    public const int AI_BLOCK = -2;

    //my ships
    public GameObject[] shipPrefabs;

    // Use this for initialization
    void Start () {
        //my turn
        this.turn = AI_TURN;

        //create ai ships
        GameObject Aship1 = (GameObject)Instantiate(shipPrefabs[0], new Vector3(-10, 0, -4), Quaternion.identity);
        GameObject Aship2 = (GameObject)Instantiate(shipPrefabs[1], new Vector3(-10, 0, -2), Quaternion.identity);
        GameObject Aship3 = (GameObject)Instantiate(shipPrefabs[2], new Vector3(-10, 0, 0), Quaternion.identity);
        GameObject Aship4 = (GameObject)Instantiate(shipPrefabs[3], new Vector3(-10, 0, 2), Quaternion.identity);
        GameObject Aship5 = (GameObject)Instantiate(shipPrefabs[4], new Vector3(-10, 0, 4), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
        if (gc.turn == AI_TURN)
        {
            gc.turn = AI_BLOCK;    //block turn
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
                    position.y = 2.0f;
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
                        GameObject bullet = (GameObject)Instantiate(bulletPrefab, new Vector3(2, 1, 0), Quaternion.identity);
                        //탄환 코드에 변수값 전달 -> 탄환 스스로 발사
                        Bullet bc = bullet.GetComponent<Bullet>();
                        bc.from = bullet.transform;
                        bc.to = rayHit.transform.gameObject.transform;
                    }

                    //카메라 원위치
                    camera.transform.position = beforePosition;
                    camera.transform.rotation = beforeLookAt;
                    
                    break;
                }
            }

            yield return null;
        }

        /*
        //카메라 정보 저장
        Vector3 beforePosition = camera.transform.position;
        Quaternion beforeLookAt = camera.transform.rotation;

        //카메라 시점 이동
        camera.transform.position = new Vector3(0, 2, 0);
        camera.transform.LookAt(new Vector3(50, 1, 0));



        Ray ray;
        RaycastHit rayHit;
        float rayLength = 500f;
        int floorMask = LayerMask.GetMask("Floor");
        bool isButtonDown = false;
        while (true)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                //탄환 생성
                bullet = (GameObject)Instantiate(
                   bulletPrefab,
                   camera.transform.position,
                   Quaternion.identity);
                isButtonDown = true;
            }
            if (isButtonDown == true)
            {
            
                ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out rayHit, rayLength, floorMask))
                {
                    Vector3 playerToMouse = rayHit.point - bullet.transform.position;

                    Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                    bullet.GetComponent<Rigidbody>().MoveRotation(newRotation);
                }
                
                if (Input.GetButtonUp("Fire1"))
                {
                    isButtonDown = false;
                    Vector3 throwAngle;

                    if (Physics.Raycast(ray, out rayHit, rayLength, floorMask))
                    {
                        throwAngle = rayHit.point - bullet.transform.position;
                    }
                    else
                    {
                        throwAngle = bullet.transform.forward * 50f;
                    }

                    throwAngle.y = 25f;
                    bullet.GetComponent<Rigidbody>().AddForce(throwAngle * throwPower);

                    //카메라 원위치
                    camera.transform.position = beforePosition;
                    camera.transform.rotation = beforeLookAt;
                    break;
                }
            }

            yield return null;
        }
        */
    }
}
