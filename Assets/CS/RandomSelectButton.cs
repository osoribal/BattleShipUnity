using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class RandomSelectButton : MonoBehaviour {

    //ship arr
    public GameObject[] ShipPrefabs;
    public GameObject ship;
    ShipInfo newShipInfo;
    public Text textGold;
    public Text textResult;
    public Text textMessage;


    //user manager
    public UserManager userManager;

    //user gold info
    int gold;
    int index;
    int opnum;
    string option;
    int size;

    void Start() {
        //get current money
        
        PlayerPrefs.SetInt("gold", 10000);
        gold = PlayerPrefs.GetInt("gold");
        textGold.text = "보유 골드 : " + gold + "G";
        print("보유 골드 : " + gold + "G");

        //text result, message init to space
        textResult.text = " ";
        textMessage.text = " ";
        
    }

    public void OnStartButtonClicked()
    {
        //check current money - equal or greater than 1000 Gold
        if (gold >= 1000)
        {
            //create random ship
            CreateShip();
            //alert message
            //the new ship saving is success

            //gold decrease save
            //PlayerPrefs.SetInt("gold", gold);
            UserManager.updateGold(-1000);
            //get current money - update state
            gold = PlayerPrefs.GetInt("gold");
            alertSelectResult();
        }
        else {
            //alert message - you can't buy a ship.
            Destroy(ship);
            alertCantBuy();
        }
    }

    void alertSelectResult() {
        textGold.text = "보유 골드 : " + gold + "G";
        textResult.text = "축하합니다!";
        size = index + 1;
        switch (opnum)
        {
            case 2:
                option = "동귀어진";
                break;
            case 3:
                option = "두번쏘기";
                break;
            case 4:
                option = "골드보상 업";
                break;
            default:
                option = "없음";
                break;
        }
        textMessage.text = "특수능력 :" + option + " / 크기 : " + size;
    }

    void alertCantBuy() {
        textResult.text = "골드가 모자랍니다!";
        textMessage.text = "게임에서 승리하여 골드를 모아주세요.";
    }

    public void CreateShip()
    {
        //destroy randombox
        GameObject box = GameObject.FindGameObjectWithTag("RandomBox");
        Destroy(box);

        //create the ship
        ship = (GameObject)Instantiate(SelectShip(), new Vector3(120, 253, 0), Quaternion.identity);
        ship.tag = "RandomBox";
        //set scale
        Vector3 scale = ship.transform.localScale;
        scale.x *= 30;
        scale.y *= 30;
        scale.z *= 30;

        ship.transform.localScale = scale;
    }

    public GameObject SelectShip()
    {
        //create random number
        GameObject prefab = null;
        //random size
        index = Random.Range(0, ShipPrefabs.Length);
        size = index + 1;
        
        //random option
        opnum = Random.Range(0, 5);

        //get ship prefab at index
        prefab = ShipPrefabs[index];

        //save new ship info
        //ship number
        int shipNum = (10 * size) + opnum;
		newShipInfo = new ShipInfo(shipNum);

        //save the new ship
        userManager.Save(newShipInfo);

        return prefab;
    }



    void Update()
    {
       //freeze position
       //Rigidbody rigidShip = ship.GetComponent<Rigidbody>();
       //Destroy(rigidShip);
		if (Input.GetKey(KeyCode.Escape))
		{
			//Escape button codes
			SceneManager.LoadScene("Title");
		}
    }
}
