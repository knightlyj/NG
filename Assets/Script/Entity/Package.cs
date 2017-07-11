using UnityEngine;
using System.Collections;
using System;

public class ItemPackage
{
    //物品栏
    public int packSize = 4 * 10;
    Item[] _content = null;
    public Item[] content { get { return this._content; } }

    public ItemPackage(int size)
    {
        packSize = size;
        _content = new Item[packSize];
    }

    //-----------------捡东西的逻辑,先看是否可以叠加,不能叠加则放入新的空位
    public bool PickUpItem(Item item)
    {
        if (item.Type.CanStack)
        {
            for (int i = 0; i < packSize; i++)
            {
                if (content[i] != null && content[i].Type.id == item.Type.id)
                {
                    content[i].amount += item.amount;
                    RaisePackChanged();
                    return true;
                }
            }
        }

        //到这里,要么没找到堆叠物品,要么不可以堆叠,则找个空位放入
        for (int i = 0; i < packSize; i++)
        {
            if (content[i] == null)
            {
                content[i] = item;
                RaisePackChanged();
                return true;
            }
        }

        return false; //背包满了
    }

    //把物品放入指定格子,如果之前这个格子有物品,则在输出参数preItem中范围,如果可以堆叠,则堆叠
    public bool PutInItem(Item item, int idx, out Item preItem)
    {
        preItem = null;
        if (idx < 0 || idx >= packSize)
            return false;

        if (content[idx] != null) //原来格子不是空的
        {
            if (item.Type.id == content[idx].Type.id && item.Type.CanStack && content[idx].Type.CanStack)
            { //可以堆叠到一起
                content[idx].amount += item.amount;
            }
            else
            {  //不能堆叠,取出原来格子里的物品,并放入新物品
                preItem = content[idx];
                content[idx] = item;
            }
        }
        else //原来格子是空的
        { 
            content[idx] = item;
        }
        
        RaisePackChanged();
        return true;
    }

    //根据索引取得并移除物品
    public Item TakeItem(int idx)
    {
        Item item = FindItem(idx);
        RemoveItem(idx);
        return item;
    }

    //根据索引移除物品 
    public bool RemoveItem(int idx)
    {
        if (idx < 0 || idx >= packSize)
            return false;

        content[idx] = null;
        RaisePackChanged();
        return true;
    }

    //--------------根据item id和数量移除物品,材料和消耗品,才可以用这个接口
    public bool RemoveAmount(int id, uint amount)
    {
        int[] idcs;
        uint total = ItemAmount(id, out idcs);
        if (total < amount) //物品不够
            return false;

        //物品数量足够,挨个减去移除量
        uint a = amount;
        foreach (int idx in idcs)
        {
            if (idx >= 0 && a > 0)
            {
                if (content[idx].amount <= a)
                {
                    a -= content[idx].amount; //这个格子里数量不够或者刚好够,则删除这个格子里的物品,并减少相应的总数量
                    content[idx] = null;
                }
                else
                {
                    content[idx].amount -= a; //这个格子里的数量已经大于剩下要扣除的数量了,减去就行
                }
            }
            else if (idx < 0 && a > 0)
            {   //之前有判断数量是足够的,不应该运行到这里,报错,并假装已扣除足够数量,继续运行
                //Debug.LogError("PackageRemoveAmount >> amount error");
                RaisePackChanged();
                return true;
            }
            else //这里只有 a <= 0的情况了,说明已扣除足够数量,跳出循环即可
            {
                break;
            }
        }
        RaisePackChanged();
        return true;
    }

    public bool ItemEnough(int id, uint amount)
    {
        int[] idcs;
        uint total = ItemAmount(id, out idcs);
        return total >= amount;
    }

    //获取物品数量,会输出所有物品的索引
    public uint ItemAmount(int id, out int[] indices)
    {
        int[] idcs = PackageFindIdcs(id);
        indices = idcs;
        if (idcs[0] < 0) //没找到,则是0个
            return 0;

        uint a = 0;
        for (int i = 0; i < idcs.Length; i++)
        {
            if (idcs[i] < 0)
                break;
            a += content[idcs[i]].amount;
        }

        return a;
    }

    //-------------------根据type找到物品的全部索引
    public int[] PackageFindIdcs(int id)
    {
        int[] found = new int[packSize];
        for (int i = 0; i < packSize; i++)
            found[i] = -1;
        int cnt = 0;
        for (int i = 0; i < packSize; i++)
        {
            if (content[i] != null)
            {
                if (content[i].Type == null) {
                    Debug.Log("PackageFindIdcs 物品类型为空");
                }
                else if (content[i].Type.id == id)
                {
                    found[cnt++] = i;
                }
            }
        }
        return found;
    }

    //-----------------根据索引找到物品信息
    public Item FindItem(int idx)
    {
        if (idx < 0 || idx >= packSize)
            return null;

        return content[idx];
    }
    
    //背包改变事件
    public delegate void OnPackChanged();
    public event OnPackChanged PackChangedEvent;
    void RaisePackChanged()
    {
        if (PackChangedEvent != null)
            PackChangedEvent();
    }

    //背包序列化
    public byte[] Serialize()
    {
        byte[] data = new byte[512];
        return data;
    }

    public bool Deserialize(byte[] data)
    {
        return true;
    }
}

//垃圾箱
public class Trash
{
    Item _item = null;
    public Item item { get { return this._item; } }
    //是否有物品
    public bool hasItem { get { return _item != null; } }

    //放入物品
    public void PutItem(Item it)
    {
        _item = it;
    }

    //取出物品
    public Item TakeItem()
    {
        Item it = _item;
        _item = null;
        return it;
    }
    
}

//鼠标格子
public class MouseItem
{
    Item _item = null;
    public Item item { get { return this._item; } }
    //是否有物品
    public bool hasItem { get { return _item != null; } }

    //放入物品,会返回原来的物品
    public Item PutItem(Item it)
    {
        Item temp = this._item;
        this._item = it;
        EventManager.RaiseEvent(EventId.MouseItemChange, this);
        return temp;
    }

    //取出物品
    public Item TakeItem()
    {
        Item it = _item;
        _item = null;
        EventManager.RaiseEvent(EventId.MouseItemChange, this);
        return it;
    }
}

//这里包括背包的格子,垃圾箱,以及鼠标拖拽的物品
public class PlayerBag
{
    /// <summary>
    /// 物品包
    /// </summary>
    ItemPackage _itemPack;
    public ItemPackage itemPack { get { return this._itemPack; } }

    /// <summary>
    /// 垃圾箱
    /// </summary>
    Trash _trash;
    public Trash trash { get { return this._trash; } }

    /// <summary>
    /// 鼠标物品
    /// </summary>
    MouseItem _mouseItem;
    public MouseItem mouseItem { get { return this._mouseItem; } }

    /// <summary>
    /// 钱
    /// </summary>
    public int money = 0;

    public PlayerBag(int size)
    {
        _itemPack = new ItemPackage(size);
        _trash = new Trash();
        _mouseItem = new MouseItem();
    }
}
