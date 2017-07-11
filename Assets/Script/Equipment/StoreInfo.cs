using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class Commodity
{
    public Item item;

    public Commodity(Item item)
    {
        this.item = item;
    }
}

public class StoreInfoEnum : IEnumerator<Commodity>
{
    List<Commodity> _commodities = null;
    int idx = -1;


    public Commodity Current { get { return _commodities[idx]; } }

    public void Dispose() { }

    object IEnumerator.Current { get { return this.Current; } }

    public bool MoveNext()
    {
        if (++idx >= _commodities.Count)
            return false;
        else
            return true;
    }

    public void Reset()
    {
        idx = -1;
    }

   
    public StoreInfoEnum(List<Commodity> comm)
    {
        _commodities = comm;
        idx = -1;
    }
}

public class StoreInfo : IEnumerable<Commodity>
{
    List<Commodity> _commodities = new List<Commodity>(20);
    public List<Commodity> commodities { get { return this._commodities; } }

    public IEnumerator<Commodity> GetEnumerator()
    {
        return new StoreInfoEnum(this._commodities);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public void AddCommodity(Commodity comm)
    {
        _commodities.Add(comm);
    }

    public int Count { get { return this._commodities.Count; } }

    public void Clear()
    {
        _commodities.Clear();
    }

    public Commodity this[int idx]{
        get { return _commodities[idx]; }
    }

}
