using UnityEngine;
using System.Collections;

public class BulletDestroyer : MonoBehaviour
{




    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //destroy bullet
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet") Destroy(other.gameObject);
       
    }


}
    