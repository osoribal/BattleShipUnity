using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**** https://www.youtube.com/watch?v=J6FfcJpbPXE&feature=youtu.be *****/

//save and load datas
public class UserManager : MonoBehaviour {
    string path;
    public static UserManager control;
    public static List<ShipInfo> list = new List<ShipInfo>();   //보유중인 배 list

    public static ShipInfo[] userShips = new ShipInfo[5];   //선택된 배의 오브젝트
    public static OptionInfo opInfo = new OptionInfo(); //option information
    //option string
    public const string EFFECT = "Effect";
    public const string BACKGROUND = "Back";


    //씬이 변경될 때 UserManager가 유일하도록 유지
    void Awake()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        path = Application.persistentDataPath + "/userdata.dat";

        //test data
        Save(new ShipInfo(11));
        Save(new ShipInfo(21));
        Save(new ShipInfo(13));
        Save(new ShipInfo(31));
        Save(new ShipInfo(14));
        Save(new ShipInfo(41));
        Save(new ShipInfo(51));
        Save(new ShipInfo(41));
        Save(new ShipInfo(51));

        //데이터 불러오기
        this.Load();

        //PlayerPrefs 초기화
        //PlayerPrefs.DeleteAll();


        //userShips 초기화
        userShips = new ShipInfo[5];
        for (int i = 0; i < 5; i++)
        {
            userShips[i] = new ShipInfo(0);
        }

        //shipInfo test data
        userShips[0] = new ShipInfo(11);
        userShips[0].x = 0;
        userShips[0].y = 0;
        userShips[0].direction = 2;

        userShips[1] = new ShipInfo(22);
        userShips[1].x = 0;
        userShips[1].y = 1;
        userShips[1].direction = 2;

        userShips[2] = new ShipInfo(33);
        userShips[2].x = 0;
        userShips[2].y = 2;
        userShips[2].direction = 2;

        userShips[3] = new ShipInfo(44);
        userShips[3].x = 0;
        userShips[3].y = 3;
        userShips[3].direction = 2;

        userShips[4] = new ShipInfo(55);
        userShips[4].x = 0;
        userShips[4].y = 4;
        userShips[4].direction = 2;

        //option
        opInfo.effect = "on";

    }
    
    //골드 획득/사용 시 호출
    //파라미터 : 골드 획득/사용 시 변동 값
    public void updateGold(int g)
    {
        
        int gold = PlayerPrefs.GetInt("gold");
        gold += g;
        print("gold : " + gold + " g: " + g);
        PlayerPrefs.SetInt("gold", gold);   //gold 값 파일로 저장
        print("gold : " + gold);
        //다른 씬에서는 PlayerPrefs.GetInt("gold")를 통해 gold 값을 불러올 수 있다.
    }

    //새로운 배를 저장하는 함수
    public void Save(ShipInfo data)
    {
        //보유중인 배인지 Find 함수를 통해 알아낸다.
        ShipInfo findResult = list.Find(x => x.shipNum.Equals(data.shipNum));
        if (findResult == null) //미보유
        {
            list.Add(data); //추가
            list.Sort();    //정렬
        }
        else
        {
            //보유중 : 보유개수만 증가
            findResult.count++;
        }

        //파일로 저장
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, list);
        file.Close();
    }

    //파일로 저장된 배 정보들을 불러오는 함수
    void Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            list = (List<ShipInfo>)bf.Deserialize(file);
            file.Close();
        }
    }

    //set option information data
    public void setOptionInfo(OptionInfo option)
    {
        PlayerPrefs.SetString(EFFECT, option.effect);
        PlayerPrefs.SetString(BACKGROUND, option.back);
    }

    //get option information data
    public OptionInfo getOptionInfo()
    {
        OptionInfo info = new OptionInfo();
        info.effect = PlayerPrefs.GetString(EFFECT);
        info.back = PlayerPrefs.GetString(BACKGROUND);
        return info;
    }
	
}

[Serializable]
public class ShipInfo : IComparable<ShipInfo>
{
    public int shipNum; //고유번호
    public int count;   //보유개수
    public int x, y;        //배 머리 위치
    public int direction;   //배 방향

    public ShipInfo(int n)
    {
        shipNum = n;
        count = 1;
    }

    //sort 시 비교함수
    public int CompareTo(ShipInfo comparePart)
    {
        // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            return this.shipNum.CompareTo(comparePart.shipNum);
    }

}

//option info class
public class OptionInfo
{
    //option values
    public string effect;
    public string back;
}