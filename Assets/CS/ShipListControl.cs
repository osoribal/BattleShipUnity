using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;



/**** https://www.youtube.com/watch?v=J6FfcJpbPXE&feature=youtu.be *****/


public class ShipListControl : MonoBehaviour {
    string path;
    public static ShipListControl control;
    public static List<ShipInfo> list = new List<ShipInfo>();
    
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
        this.Save(new ShipInfo(1, 2));
        this.Save(new ShipInfo(2, 3));
        this.Save(new ShipInfo(3, 4));

        this.Load();
        
    }

    public void Save(ShipInfo data)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(path);

        list.Add(data);

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
    //임시 변수들
    public int size;
    public int ability;

    public ShipInfo(int s, int a)
    {
        size = s;
        ability = a;
    }
}