using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public partial class Player : Entity
{
    void PlayerItemInit()
    {
        _playerBag = new PlayerBag(itemPackSize);
        _equip = new PlayerEquipment();
    }

    public const int itemPackSize = 40;
    PlayerBag _playerBag = null;
    public PlayerBag bag { get { return this._playerBag; } }

    //属性改变事件
    //public delegate void PropChanged(Player p);
    //public event PropChanged PropChangedEvent = null;
    //void RaisePropChanged()
    //{
    //    if (this.PropChangedEvent != null)
    //        PropChangedEvent(this);
    //}

    PlayerEquipment _equip = null;
    public PlayerEquipment playerEquipment {  get { return this._equip; } }
}
