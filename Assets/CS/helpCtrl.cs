using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class helpCtrl : MonoBehaviour {
    public Texture[] images;
    int now;

	// Use this for initialization
	void Start () {
        now = 0;
        gameObject.GetComponent<RawImage>().texture = images[now];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Input.mousePosition.x < Screen.width / 2)
            {
                //화면 왼 쪽 터치
                now--;
            }
            else
            {
                //화면 오른 쪽 터치
                now++;
            }

            if (now < 0 || now >= images.Length)
            {
                //첫 사진에서 왼쪽을 터치하거나
                //마지막 사진에서 오른쪽을 터치하면
                //홈으로 돌아가기
                Destroy(this.gameObject);
            }
            else
            {
                gameObject.GetComponent<RawImage>().texture = images[now];  //화면 갱신
            }
            
            
        }
    }
}
