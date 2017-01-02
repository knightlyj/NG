using UnityEngine;
using System.Collections;
using System;

public class CharacterProperties {
    public int hp;
    public int mp;
    public int damage;
    public int defense;


    public void DealDamage(DamageDate damage)
    {
        this.hp -= damage.physicalDamage;
        if(this.hp <= 0)
        {
            EventHandler<EventArgs> temp = OnHpEmpty;
            if (temp != null)
                temp(this, null);
        }

    }
    public event EventHandler<EventArgs> OnHpEmpty;
}

public struct DamageDate
{
    public int physicalDamage;
    public int knockBack;
}

