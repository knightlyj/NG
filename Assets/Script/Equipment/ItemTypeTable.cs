using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemTypeTable : ScriptableObject
{
    public ItemType[] table = new ItemType[(int)ItemId.Max];
    
    public ItemType GetItemType(ItemId id)
    {
        ItemType type = table[(int)id];

        return type;
    }
}
