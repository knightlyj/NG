using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemTypeTable : ScriptableObject
{
    public ItemType[] table;

    public ItemTypeTable()
    {
        Debug.Log("constructor");
        table = new ItemType[(int)ItemId.Max];
        for (int i = 0; i < table.Length; i++)
        {
            table[i] = new ItemType();
        }
    }
    public ItemType GetItemType(ItemId id)
    {
        ItemType type = table[(int)id];

        return type;
    }
}
