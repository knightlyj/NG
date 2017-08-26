using UnityEngine;
using System.Collections;

public class FastProjectile : Projectile {

    const float startScale = 0.5f;
    const float scaleTime = 0.5f;
    float totalTime = 0;
    void Start()
    {
        totalTime = 0;
        transform.localScale = new Vector3(1f, startScale, 1f);
    }
    
    [SerializeField]
    protected float baseSpeed = 0.2f;
    void Update()
    {
        CheckLifeTime();

        totalTime += Time.deltaTime;
        float rate = totalTime / scaleTime;
        if (rate > 1)
            rate = 1;
        transform.localScale = new Vector3(1f, startScale + rate * (1- startScale), 1f);

        //检查碰撞
        Vector2 step = direction * baseSpeed * speedScale;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, (Vector2)transform.position + step);
        if(hit.collider != null)
        {
            GameObject go = hit.collider.gameObject;
            if (((1 << go.layer) & hitLayerMask) != 0)
            {
                Entity e = go.gameObject.GetComponent<Entity>();
                if (e != null)
                    e.HitByOther(this, transform.position);
                Explode(hit.point);
            }
        }

        transform.position = transform.position + (Vector3)step;
    }
}
