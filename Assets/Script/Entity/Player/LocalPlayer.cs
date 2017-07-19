using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalPlayer : Player
{
    public bool Save(string path)
    {
        //try
        //{
        string file = path + "/" + this._playerName;
        Debug.Log(file);
        IFormatter formatter = new BinaryFormatter();
        FileStream s = new FileStream(file, FileMode.Create);
        formatter.Serialize(s, this._playerName); //名字
        formatter.Serialize(s, this._bag); //背包
        formatter.Serialize(s, this._equip); //装备

        s.Close();
        //}
        //catch
        //{   //反正是保存失败了
        //    return false;
        //}
        return true;
    }

    public bool Load(string file)
    {
        //try
        //{
        IFormatter formatter = new BinaryFormatter();
        FileStream s = new FileStream(file, FileMode.Open);
        this._playerName = (string)formatter.Deserialize(s);
        this._bag = (PlayerBag)formatter.Deserialize(s);
        this._equip = (PlayerEquipment)formatter.Deserialize(s);
        this._equip.BindProperties(this.Properties); //属性重新计算
        this._equip.RecalcProperties();
        EventManager.RaiseEvent(EventId.LocalPlayerLoad, this);

        s.Close();
        //}
        //catch
        //{   //反正是读取失败
        //    return false;
        //}
        return true;
    }
}
