using UnityEngine;
using System.Collections;

public class GameControler : MonoBehaviour {
    public int turn;
    
    // Use this for initialization
    void Start () {
        turn = 0;
        
    }
	
	// Update is called once per frame
	void Update () {
        switch(turn)
        {
            case 0: //user turn
                break;
            case 1: //ai turn
                turn = 0;
                break;
            case 2: //user win
                break;
            case 3: //ai win
                break;
            default:
                break;
        }
	
	}
}
