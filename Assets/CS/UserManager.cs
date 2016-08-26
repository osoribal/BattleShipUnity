using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/**** https://www.youtube.com/watch?v=J6FfcJpbPXE&feature=youtu.be *****/

//save and load datas
public class UserManager : MonoBehaviour {
    static string path;
    public static UserManager control;
    public static List<ShipInfo> list = new List<ShipInfo>();   //보유중인 배 list

    public static ShipInfo[] userShips = new ShipInfo[5];   //선택된 배의 오브젝트
    public static OptionInfo opInfo = new OptionInfo(); //option information
    //option string
    public const string EFFECT = "Effect";
    public const string BACKGROUND = "Back";
    const string ON = "on";


    //첫 앱 실행 시 골드 1000, 배 다섯 대
    void firstLaunch()
    {
        if (PlayerPrefs.GetInt("firstLaunch") == 0)
        {
            PlayerPrefs.SetInt("gold", 1000);
            Save(new ShipInfo(11));
            Save(new ShipInfo(21));
            Save(new ShipInfo(31));
            Save(new ShipInfo(41));
            Save(new ShipInfo(51));
            PlayerPrefs.SetInt("firstLaunch", 1);
        }

        //option all on
        PlayerPrefs.SetString(EFFECT, ON);
        PlayerPrefs.SetString(BACKGROUND, ON);
    }


    //씬이 변경될 때 UserManager가 유일하도록 유지
    void Awake()
    {
        path = Application.persistentDataPath + "/userdata.dat";
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
            firstLaunch();
        }
        else if (control != null)
        {
            Destroy(gameObject);
        }

    }

    void Start()
    {
        //데이터 불러오기
        this.Load();

        //option
        opInfo.effect = "on";

    }
    
    //골드 획득/사용 시 호출
    //파라미터 : 골드 획득/사용 시 변동 값
    public static void updateGold(int g)
    {
        
        int gold = PlayerPrefs.GetInt("gold");
        gold += g;
        PlayerPrefs.SetInt("gold", gold);   //gold 값 파일로 저장
        //다른 씬에서는 PlayerPrefs.GetInt("gold")를 통해 gold 값을 불러올 수 있다.
    }

    //배를 삭제하는 함수
    public static void removeShip(int shipNum)
    {
        ShipInfo findResult = list.Find(x => x.shipNum.Equals(shipNum));
        
        findResult.count--;
        if (findResult.count == 0)
        {
            list.Remove(findResult);
        }

        //파일로 저장
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        bf.Serialize(file, list);
        file.Close();
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