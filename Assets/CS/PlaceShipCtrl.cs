using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlaceShipCtrl : MonoBehaviour {
    
    public GameObject[,] userGrid = new GameObject[10, 10];
    public GameObject tilePrefab;  
    public GameObject[] shipPrefab; //배의 프리팹 5종 저장

    public void OnBackClicked()
    {
        SceneManager.LoadScene("SelectShip");
    }
    public void OnNextClicked()
    {
        SceneManager.LoadScene("Battle");
    }

    // Use this for initialization
    void Start () {
        //격자 생성
        Vector3 userzero = new Vector3(0, 0, 0);
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                userGrid[i, j] = (GameObject)Instantiate(tilePrefab, userzero, Quaternion.identity);
                userGrid[i, j].GetComponent<SeaControler>().x = i;
                userGrid[i, j].GetComponent<SeaControler>().y = j;
                userzero.z--;
            }
            userzero.z = 0;
            userzero.x++;
        }

        //기본 위치에 user ships 오브젝트 생성
        //정확한 위치, 방향은 ship.cs에서 진행
        for (int i = 0; i < 5; i++)
        {
            //배의 고유 번호로부터 배 길이 추출
            int shipLength = UserManager.selectedShipArr[i] / 10;

            //배의 기본 위치 설정
            Vector3 position = userGrid[0, i].transform.position;
            position.x += 0.5f * (shipLength - 1);  //배의 머리가 해당 타일에 위치하도록 배를 이동

            //배 오브젝트 생성
            UserManager.userShips[i] = (GameObject)Instantiate(
                        shipPrefab[shipLength - 1],
                        position,
                        Quaternion.identity);

            //ship에 기본 위치 정보 저장
            Ship ctrl = UserManager.userShips[i].GetComponent<Ship>();
            ctrl.shipID = UserManager.selectedShipArr[i];
            ctrl.x = 0;
            ctrl.y = i;

            //ship에 기본 방향 정보 저장
            ctrl.direction = 0;            
        }
    }
	
}
