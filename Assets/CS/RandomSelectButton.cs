using UnityEngine;
using System.Collections;

public class RandomSelectButton : MonoBehaviour {

    //ship arr
    public GameObject[] ShipPrefabs;
    public GameObject ship;
    ShipInfo newShipInfo;

    //user manager
    public UserManager userManager;

    //user gold info
    int gold;

	public void OnStartButtonClicked()
    {
        //get current money
		gold = PlayerPrefs.GetInt("gold");

        //check current money - equal or greater than 1000 Gold
        if (gold >= 1000)
        {
            //create random ship
            CreateShip();
            //decrease gold
            gold = gold - 1000;
            //save gold
            //save the new ship

            userManager.Save(newShipInfo);

            //alert message
            //the new ship saving is success

			//gold decrease save
			PlayerPrefs.SetInt("gold");
        }
        else {
            //alert message - you can't buy a ship.
        }
    }

    public GameObject SelectShip()
    {
        //create random number
        GameObject prefab = null;
        int index = Random.Range(0, ShipPrefabs.Length);
        //get ship prefab at index
        prefab = ShipPrefabs[index];

        //save new ship info
        //////////change!!!!! - index -> prefabs.shipID
		newShipInfo = new ShipInfo(index);

        return prefab;
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

	void Start()
	{
		
	}

    void Update()
    {
       //freeze position
       //Rigidbody rigidShip = ship.GetComponent<Rigidbody>();
       //Destroy(rigidShip);
    }
}
