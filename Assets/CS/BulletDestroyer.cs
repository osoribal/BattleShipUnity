using UnityEngine;
using System.Collections;

public class BulletDestroyer : MonoBehaviour
{

    public GameControler gameController;

    //ai turn
    public const int USER_TURN = 0;
    public const int AI_TURN = 1;

    public const int USER_BLOCK = -1;
    public const int AI_BLOCK = -2;

    public Transform from;
    public Transform to;
    public float value = 10.0F;
    private float startTime;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //탄환이 포물선을 그리며 이동
        Vector3 center = (from.position + to.position) * 0.5F;
        center -= new Vector3(0, 1, 0);
        Vector3 riseRelCenter = from.position - center;
        Vector3 setRelCenter = to.position - center;
        float fracComplete = (Time.time - startTime) / value;
        transform.position = Vector3.Slerp(riseRelCenter, setRelCenter, fracComplete);
        transform.position += center;
    }

    //destroy bullet
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Arrow")
            Destroy(other.gameObject);
        if (other.gameObject.tag == "Bullet") Destroy(other.gameObject);
        OnNotify();
    }

    void OnNotify()
    {
        //notify to game controller
        //gameController = GameObject.FindWithTag("GameObject").GetComponent<GameControler>();
        int whoseTurn = gameController.GetTurn();

        switch (whoseTurn)
        {
            case USER_TURN:
                break;
            case AI_TURN:
                break;
            case USER_BLOCK:
                gameController.turn = AI_TURN;
                break;
            case AI_BLOCK:
                gameController.turn = USER_TURN;
                break;

        }
    }
}
    