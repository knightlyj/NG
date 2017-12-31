using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LocalPlayer : Player
{
    public LocalPlayer()
    {
        ItemInit();
    }

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
        this._equip.BindPlayer(this); //属性重新计算
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



    /// <summary>
    /// 背包初始化
    /// </summary>
    void ItemInit()
    {
        _bag = new PlayerBag(itemPackSize);
        _equip = new PlayerEquipment(this);
    }

    public const int itemPackSize = 40;
    protected PlayerBag _bag = null;
    public PlayerBag bag { get { return this._bag; } }

    //属性改变事件
    //public delegate void PropChanged(Player p);
    //public event PropChanged PropChangedEvent = null;
    //void RaisePropChanged()
    //{
    //    if (this.PropChangedEvent != null)
    //        PropChangedEvent(this);
    //}

    protected PlayerEquipment _equip = null;
    public PlayerEquipment equipment { get { return this._equip; } }

    //******************************************
    //操作相关
    protected override void Update()
    {
        base.Update();
        CheckShoot();
    }

    void CheckShoot()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    _aiming = true;
        //}

        //if (Input.GetMouseButtonUp(1))
        //{
        //    _aiming = false;
        //}


        
        if (Input.GetMouseButtonDown(0))
        {
            
            this.Shoot();
        }
    }
}
