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
        OnNotify();
    }

    void OnNotify()
    {
        //notify to game controller
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControler>();
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
    