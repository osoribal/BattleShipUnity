using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * 배의 수는 5개여야 하며 배 5척으로 이루어진 칸의 수는 최소 10칸 최대 20칸을 넘을 수 없다
 */

public class SelectShipCtrl : MonoBehaviour {
    public GameObject content;
    public GameObject elemPrefab;
    public Sprite shipImage;
    public int userLife; //최소 10칸 최대 20칸
    public int selectedShipCount;    //배 선택 시 선택된 배의 개수
    public int[] selectedShipArr;   //선택된 배의 고유번호 저장
    
    public void OnNextClicked()
    {
        if (selectedShipCount != 5)
            return;
        if (userLife < 10  || userLife > 20)
            return;

        PlayerPrefs.SetInt("userLife", userLife);
        
        for (int i = 0; i < 5; i++)
        {
            UserManager.userShips[i].shipNum = selectedShipArr[i];
        }
        
        SceneManager.LoadScene("PlaceShip");
    }

    // Use this for initialization
    void Start()
    {
        //초기화
        selectedShipCount = 0;
        selectedShipArr = new int[5];
        userLife = 0;

        //화면에 배들을 버튼으로 출력
        for (int i = 0; i < UserManager.list.Count; i++)
        {
            //보유개수 만큼 출력
            for (int j = 1; j <= UserManager.list[i].count; j++)
            {
                //오브젝트 생성
                GameObject elem = Instantiate(elemPrefab) as GameObject;
                //스냅샷 
                elem.GetComponentsInChildren<Image>()[1].overrideSprite = shipImage;
                //배의 정보를 항목에 전달
                elem.GetComponent<ElemCtrl>().info = UserManager.list[i];
                elem.GetComponent<ElemCtrl>().selectCtrl = this;
                //리스트에 추가
                elem.transform.SetParent(content.transform, false);
            }
            
        }
    }

    //선택된 배의 개수 변화를 ui에 반영
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            //Escape button codes
            SceneManager.LoadScene("Title");
        }
        this.GetComponentsInChildren<Text>()[1].text = 
            "배 수 : " + selectedShipCount + " / 5\n" +
            "칸 수 : " + userLife + " 칸";
    }
}
