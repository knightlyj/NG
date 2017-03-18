using UnityEngine;
using System.Collections;

public struct EntityProperties
{
    public int hp;
}

public class Entity : MonoBehaviour {
    public bool isLocal = true;
    public Rigidbody2D rb = null;
	// Use this for initialization
	protected virtual void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update () {
        this.buffModule.UpateBuff(Time.deltaTime);
    }

    EntityProperties _properties;
    public EntityProperties Properties { get { return _properties;} }

    public BuffModule buffModule = new BuffModule();
    public virtual void SetAlpha(float a)
    {

    }
}
