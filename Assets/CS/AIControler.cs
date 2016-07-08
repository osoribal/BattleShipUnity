using UnityEngine;
using System.Collections;

public class AIControler : MonoBehaviour {

    public GameObject[] shipPrefabs;
    // Use this for initialization
    void Start () {
        //create ai ships
        GameObject Aship1 = (GameObject)Instantiate(shipPrefabs[0], new Vector3(-10, 0, -4), Quaternion.identity);
        GameObject Aship2 = (GameObject)Instantiate(shipPrefabs[1], new Vector3(-10, 0, -2), Quaternion.identity);
        GameObject Aship3 = (GameObject)Instantiate(shipPrefabs[2], new Vector3(-10, 0, 0), Quaternion.identity);
        GameObject Aship4 = (GameObject)Instantiate(shipPrefabs[3], new Vector3(-10, 0, 2), Quaternion.identity);
        GameObject Aship5 = (GameObject)Instantiate(shipPrefabs[4], new Vector3(-10, 0, 4), Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
