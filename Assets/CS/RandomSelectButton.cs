using UnityEngine;
using System.Collections;

public class RandomSelectButton : MonoBehaviour {

    //ship arr
    public GameObject[] ShipPrefabs;
    public GameObject ship;

	public void OnStartButtonClicked()
    {
        CreateShip();
    }

    public GameObject SelectShip()
    {
        GameObject prefab = null;
        int index = Random.Range(0, ShipPrefabs.Length);
        prefab = ShipPrefabs[index];

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

    void Update()
    {
       //freeze position
       Rigidbody rigidShip = ship.GetComponent<Rigidbody>();
       Destroy(rigidShip);
    }
}
