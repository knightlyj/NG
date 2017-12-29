using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum BuffId
{
    None,
    Invincible,
}

public class BuffModule{
    Dictionary<BuffId, BaseBuff> buffDict = new Dictionary<BuffId, BaseBuff>();
    //同id的buff不能叠加,只能覆盖
	public bool AddBuff(Creature target, BuffId id)
    {
        BaseBuff newBuff = null;
        switch (id)
        {
            case BuffId.Invincible:
                newBuff = new InvincibleBuff();
                break;
        }
        if (newBuff != null)
        {
            if (buffDict.ContainsKey(id))
            {   //移除原来的buff
                if(buffDict[id] != null)
                    buffDict[id].Stop();
                buffDict[id] = newBuff;
            }
            else
            {
                buffDict.Add(id, newBuff);
            }
            
            newBuff.Start(target);
        }
        

        return true;
    }


    public bool RemoveBuff(BuffId id)
    {
        if(buffDict.ContainsKey(id))
        {
            if(buffDict[id] != null)
                buffDict[id].Stop();
            buffDict.Remove(id);
        }
        else
        {   //not exist
            return false;
        }
        return true;
    }

    public void ClearBuff()
    {
        foreach(KeyValuePair<BuffId, BaseBuff> pair in buffDict)
        {
            pair.Value.Stop();
        }
        buffDict.Clear();
    }

    List<BuffId> deleteBuffList = new List<BuffId>();
    public void UpateBuff(float deltaTime)
    {
        foreach(KeyValuePair<BuffId, BaseBuff> pair in buffDict)
        {
            if (pair.Value.Update(deltaTime)) //返回true表示到时间了,要删除这个buff
            {
                pair.Value.Stop();
                deleteBuffList.Add(pair.Key);
            }
        }

        foreach (var item in deleteBuffList)
        {
            buffDict.Remove(item);
        }
        deleteBuffList.Clear();
    }

    
}
