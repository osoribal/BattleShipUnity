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
    public int occ;
    public PlaceShipCtrl placeCtrl;

    //turn flags
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;
    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;
    

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
                    transform.position = placeCtrl.place(shipID / 10, direction, (int)curPosition.x, (int)curPosition.z);
                }

            }

            yield return null;
        }

        //드래그 끝
        //set occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 1);
    }

    //rotate 버튼 리스너
    public void rotate()
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
            else
            {
                DialogCtrl dialog = Instantiate(placeCtrl.DialogPrefab).GetComponent<DialogCtrl>();
                dialog.setLifetime(2.0f);
                dialog.setText("다른 배가 있어 회전할 수 없습니다.");
            }

        }
        else
        {
            DialogCtrl dialog = Instantiate(placeCtrl.DialogPrefab).GetComponent<DialogCtrl>();
            dialog.setLifetime(2.0f);
            dialog.setText("격자 밖으로 벗어날 수 있어\n회전할 수 없습니다.");
        }
        //set occupied
        placeCtrl.setOccupied(shipID / 10, direction, x, y, 1);

    }
}
