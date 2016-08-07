using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



/**** https://www.youtube.com/watch?v=J6FfcJpbPXE&feature=youtu.be *****/


public class UserManager : MonoBehaviour {
    string path;
    public static UserManager control;
    public static List<ShipInfo> list = new List<ShipInfo>();   //보유중인 배 list

    /*
     * save/load controler
     * 골드 저장
     * 골드 사용/획득 함수 구현
     * 골드 보유 ui -> title, random scene
     * 
     */
    
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
        path = Application.persistentDataPath + "/shiplist.dat";

        //test data
        this.Save(new ShipInfo(1));
        this.Save(new ShipInfo(2));
        this.Save(new ShipInfo(3));

        this.Load();
        
    }

    public void Save(ShipInfo data)
    {
        //보유개수 설정, 저장
        list.Add(data);
        //정렬 

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);
        
        bf.Serialize(file, list);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(path, FileMode.Open);
            list = (List<ShipInfo>)bf.Deserialize(file);
            file.Close();
        }
    }
	
}

[Serializable]
public class ShipInfo
{
    /*
     * 고유번호만 저장
     */
    public int shipNum; //고유번호
    public int count;   //보유개수

    public ShipInfo(int n)
    {
        shipNum = n;
    }
}