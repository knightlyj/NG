using UnityEngine;
using System.Collections;

public class ProjectileSpawner : MonoBehaviour {
    public Transform rocektPrefab;

    public enum ProjectileType
    {
        None,
        Rocket,
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public void Shoot(ProjectileType type, EntityProperties ownerProp, Vector2 position, Vector2 target, float speedScale, int hitLayerMask)
    {
        Transform trProjectile = null;
        switch (type)
        {
            case ProjectileType.Rocket:
                trProjectile = Instantiate(this.rocektPrefab);
                break;
        }

        Projectile prScrpit = trProjectile.GetComponent<Projectile>();
        prScrpit.Shoot(ownerProp, position, target, speedScale, hitLayerMask);
    }
}
