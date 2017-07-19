using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public partial class Player : Entity
{
    void ItemInit()
    {
        _bag = new PlayerBag(itemPackSize);
        _equip = new PlayerEquipment(this.Properties);
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
    public PlayerEquipment equipment {  get { return this._equip; } }
}
