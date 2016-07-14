using UnityEngine;
using System.Collections;

public class UserControler : MonoBehaviour {
    int turn;
    public GameControler gc;
    public Camera camera;
    public GameObject bulletPrefab;
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
        //camera.transform.position = new Vector3(0, 2, 0);
        //camera.transform.LookAt(new Vector3(-50, 1, 0));

        //탄환 생성
        GameObject bullet = (GameObject)Instantiate(
            bulletPrefab,
            new Vector3(0, 0, 0),
            Quaternion.identity);
        
        Ray ray;
        RaycastHit rayHit;
        float rayLength = 500f;
        int floorMask = LayerMask.GetMask("Floor");
        bool isButtonDown = false;
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

                if (Physics.Raycast(ray, out rayHit, rayLength, floorMask))
                {
                    Vector3 playerToMouse = rayHit.point - bullet.transform.position;

                    Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
                    bullet.GetComponent<Rigidbody>().MoveRotation(newRotation);
                }

                /******Drop******/
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

                    //throwAngle.y = 50f;
                    bullet.GetComponent<Rigidbody>().AddForce(throwAngle * throwPower);

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
