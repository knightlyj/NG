using UnityEngine;
using System.Collections;

/// <summary>
/// 这里使用枚举修改武器外观和射击效果,武器具体属性由装备确定,与外观独立开
/// </summary>
public class Weapon : MonoBehaviour {
    //外观
    public enum AppearanceType
    {
        TestGun,
    }

    public AppearanceType apprearance;

    //射击效果
    public enum ShotType
    {
        Pistol, //一发子弹,瞬间到达
        ShotGun, //霰弹枪,暂时没做
        Projectile, //抛射物
    }
    public ShotType shotType;

    
    public Spawner.ProjectileType projectile; //仅在射击类型为抛射物时有效

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Shoot(EntityProperties ownerProp, Vector2 target, int hitLayerMask, float speedScale = 1.0f)
    {
        Vector3 muzzlePos = transform.FindChild("Muzzle").position;
        switch (shotType)
        {
            case ShotType.Pistol:
                FirePistol(ownerProp, muzzlePos, target, hitLayerMask);
                break;
            case ShotType.ShotGun:
                break;
            case ShotType.Projectile:
                FireProjectile(projectile, ownerProp, muzzlePos, target, speedScale, hitLayerMask);
                break;
        }
    }

    void FirePistol(EntityProperties ownerProp, Vector2 start, Vector2 target, int hitLayerMask)
    {
        Vector2 dir = target - start;
        dir.Normalize();
        RaycastHit2D hit = Physics2D.Raycast(start, dir, 5, hitLayerMask);
        if(hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster") ||
                hit.collider.gameObject.layer == LayerMask.NameToLayer("NPC") ||
                hit.collider.gameObject.tag == "Player")
            {   //击中单位
                Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
                spawner.ShootBullet(Spawner.BulletType.BaseBullet, Spawner.HitType.Ground, start, hit.point);

                Entity e = hit.collider.GetComponent<Entity>();
                e.HitByOther(ownerProp);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {   //击中地面
                Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
                spawner.ShootBullet(Spawner.BulletType.BaseBullet, Spawner.HitType.Ground, start, hit.point);
            }
        }
        else
        { //没击中任何碰撞体,不显示击中效果
            Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
            spawner.ShootBullet(Spawner.BulletType.BaseBullet, Spawner.HitType.Nothing, start, start+dir*5);
        }
    }

    void FireShotGun()
    {

    }

    void FireProjectile(Spawner.ProjectileType type, EntityProperties ownerProp, Vector2 start, Vector2 target, float speedScale, int hitLayerMask)
    {
        Spawner spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        spawner.ShootProjectile(type, ownerProp, start, target, 1, hitLayerMask);
    }
}
