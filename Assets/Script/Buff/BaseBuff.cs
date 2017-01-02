﻿using UnityEngine;
using System.Collections;

public class BaseBuff
{
    public enum EntityType
    {
        None,
        Player,
        Monster,
    }
    protected EntityType entityType;
    protected object buffTarget = null;
    protected float duration = 0;
    // Use this for initialization
    public virtual void Start (object target)
    {
	    if(target.GetType() == typeof(Player))
        {
            entityType = EntityType.Player;
        }

        buffTarget = target;
    }
	
	public virtual bool Update(float deltaTime)
    {
        return false;   
	}
    
    public virtual void Stop()
    {

    }
    
    float Duration { get { return this.duration; } }
}
