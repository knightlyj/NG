using UnityEngine;
using System.Collections;

public class Bullet : Arrow {
    public Transform ExplosionPrefab;
    override protected void WhenHit()
    {
        Transform trExplosion = Instantiate(ExplosionPrefab, transform.position, Quaternion.identity) as Transform;
        ParticleSystem ps = trExplosion.GetComponent<ParticleSystem>();
        Destroy(trExplosion.gameObject, ps.startLifetime);
        Destroy(this.gameObject);
    }
}
