using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    public Transform from;
    public Transform to;
    public float value = 10.0F;
    private float startTime;

    // Use this for initialization
    void Start () {
        startTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        //탄환이 포물선을 그리며 이동
        if (!transform.position.Equals(to))
        {
            Vector3 center = (from.position + to.position) * 0.5F;
            center -= new Vector3(0, 1, 0);
            Vector3 riseRelCenter = from.position - center;
            Vector3 setRelCenter = to.position - center;
            float fracComplete = (Time.time - startTime) / value;
            transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
            transform.position += center;
        }

    }

    //destroy bullet
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
            Destroy(other.gameObject);
        if (other.gameObject.tag == "Tile")
        {
            //remove bullet
            Destroy(this.gameObject);
            //test : remove fog
            FogControler fg = other.GetComponent<FogControler>();
            fg.fogOff();
        }
    }
}
