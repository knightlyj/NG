using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {
    public Transform arrowPrefab;
    public Transform bulletPrefab;

    public enum ProjectileType
    {
        None,
        Arrow,
        Bullet,
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shoot(ProjectileType type, GameObject owner, Vector2 position, Vector2 target, float speedScale, DamageDate damage)
    {
        Transform trProjectile = null;
        switch (type)
        {
            case ProjectileType.Arrow:
                trProjectile = Instantiate(this.arrowPrefab);
                break;
            case ProjectileType.Bullet:
                trProjectile = Instantiate(this.bulletPrefab);
                break;  
        }

        Projectile prScrpit = trProjectile.GetComponent<Projectile>();
        prScrpit.Shoot(owner, position, target, speedScale, damage);
    }
}
