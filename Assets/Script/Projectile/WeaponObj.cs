using UnityEngine;
using System.Collections;


//******************************************
//可以装备多个武器,武器属性与人物属性独立,计算时用人物属性加上武器属性
//*******************************************
public class WeaponObj : MonoBehaviour {
    
    public enum ShotEffect
    {
        Pistol, //一发子弹,瞬间到达
        ShotGun, //霰弹枪,暂时没做
        Projectile, //抛射物
    }
    [SerializeField]
    private ShotEffect shotEffect; //射击效果
    [SerializeField]
    private GameManager.ProjectileType projectile; //仅在射击类型为抛射物时有效
    //通过设置以上属性和sprite,决定武器原型,但不影响武器属性                                           


    Item weaponItem = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetWeaponItem(Item w)
    {
        this.weaponItem = w;
    }

    public Item GetWeaponItem()
    {
        return this.weaponItem;
    }

    public void Shoot(PlayerProperties ownerProp, Vector2 target, int hitLayerMask, float speedScale = 1.0f)
    {
        Vector3 muzzlePos = transform.FindChild("Muzzle").position;
        switch (shotEffect)
        {
            case ShotEffect.Pistol:
                FirePistol(ownerProp, muzzlePos, target, hitLayerMask);
                break;
            case ShotEffect.ShotGun:
                break;
            case ShotEffect.Projectile:
                FireProjectile(projectile, ownerProp, muzzlePos, target, speedScale, hitLayerMask);
                break;
        }
    }

    void FirePistol(PlayerProperties ownerProp, Vector2 start, Vector2 target, int hitLayerMask)
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
                GameManager spawner = GameObject.FindWithTag(TextResources.gameManager).GetComponent<GameManager>();
                spawner.ShootBullet(GameManager.BulletType.BaseBullet, GameManager.HitType.Ground, start, hit.point);

                Entity e = hit.collider.GetComponent<Entity>();
                e.HitByOther(ownerProp);
            }
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {   //击中地面
                GameManager spawner = GameObject.FindWithTag(TextResources.gameManager).GetComponent<GameManager>();
                spawner.ShootBullet(GameManager.BulletType.BaseBullet, GameManager.HitType.Ground, start, hit.point);
            }
        }
        else
        { //没击中任何碰撞体,不显示击中效果
            GameManager spawner = GameObject.FindWithTag(TextResources.gameManager).GetComponent<GameManager>();
            spawner.ShootBullet(GameManager.BulletType.BaseBullet, GameManager.HitType.Nothing, start, start+dir*5);
        }
    }

    void FireShotGun()
    {

    }

    void FireProjectile(GameManager.ProjectileType type, PlayerProperties ownerProp, Vector2 start, Vector2 target, float speedScale, int hitLayerMask)
    {
        GameManager spawner = GameObject.FindWithTag(TextResources.gameManager).GetComponent<GameManager>();
        spawner.ShootProjectile(type, ownerProp, start, target, 1, hitLayerMask);
    }
}
