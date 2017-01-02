using UnityEngine;
using System.Collections;
using System;

public class BaseMonster : MonoBehaviour {
    protected CharacterProperties properties = new CharacterProperties();
    protected void Start()
    {
        properties.OnHpEmpty += this.OnHpEmpty;
    } 

    protected void Update()
    {
        MonsterAI();
    }

    public virtual void HitByOther(DamageDate damage, Vector2 pos)
    {
        this.properties.DealDamage(damage);
    }

    public virtual void OnHpEmpty(object sender, EventArgs e)
    {
        Destroy(this.gameObject);
    }

    protected virtual void MonsterAI()
    {

    }
}
