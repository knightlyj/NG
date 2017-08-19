using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
    [HideInInspector]
    public int minAttack = 1;
    [HideInInspector]
    public int maxAttack = 2;
    [HideInInspector]
    public float criticalChance = 0;
    [HideInInspector]
    public float criticalRate = 1;
    [HideInInspector]
    public float knockBack; //击退

    [SerializeField]
    protected float lifeTime = 5; //生命期
    protected void CheckLifeTime()
    {
        //life time
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    protected float speedScale = 1.0f; //速度scale
    protected Entity owner = null; 
    protected int hitLayerMask;
    protected Vector2 direction;
    public virtual void Shoot(Entity owner, Vector2 position, Vector2 direction, float speedScale, int hitLayerMask)
    {
        //保存参数
        this.owner = owner;
        this.speedScale = speedScale;
        this.hitLayerMask = hitLayerMask;
        this.direction = direction;
        //position direction speed
        transform.position = position;
        float angle = Mathf.Acos(direction.x) / Mathf.PI * 180;
        if (direction.y < 0)
            angle = -angle;
        angle -= 90;
        transform.Rotate(new Vector3(0, 0, 1), angle); //设置初始角度
    }

    [SerializeField]
    protected Transform explodePrefab;
    protected void Explode(Vector3 position)
    {
        Transform trExplode = Instantiate(explodePrefab, position, Quaternion.identity) as Transform;
        ParticleSystem ps = trExplode.GetComponent<ParticleSystem>();
        Destroy(trExplode.gameObject, ps.startLifetime);
        Destroy(this.gameObject);
    }
}
