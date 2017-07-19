using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public Transform rocektPrefab;
    public enum ProjectileType
    {
        Nothing,
        BaseRocket,
    }
    public void ShootProjectile(ProjectileType type, PlayerProperties ownerProp, Vector2 position, Vector2 target, float speedScale, int hitLayerMask)
    {
        Transform trProjectile = null;
        switch (type)
        {
            case ProjectileType.BaseRocket:
                trProjectile = Instantiate(this.rocektPrefab);
                break;
        }

        Projectile prScrpit = trProjectile.GetComponent<Projectile>();
        prScrpit.Shoot(ownerProp, position, target, speedScale, hitLayerMask);
    }


    public Transform baseBulletPrefab;
    public enum BulletType
    {
        Nothing,
        BaseBullet,
    }

    public Transform hitGroundPrefab;
    public enum HitType
    {
        Nothing,
        Ground,
    }
    public void ShootBullet(BulletType bulletType, HitType hitType, Vector2 start, Vector2 end)
    {
        //子弹射击效果
        Transform trBullet = null;
        switch (bulletType)
        {
            case BulletType.BaseBullet:
                trBullet = Instantiate(baseBulletPrefab);
                break;
        }
        if(trBullet != null)
        {
            ShootTrail trail = trBullet.GetComponent<ShootTrail>();
            trail.ShowTtrail(start, end);
        }

        //子弹命中效果
        Transform trHit = null;
        switch (hitType)
        {
            case HitType.Ground:
                trHit = Instantiate(hitGroundPrefab);
                break;  
        }
        if(trHit != null)
        {
            trHit.position = end;
            ParticleSystem ps = trHit.GetComponent<ParticleSystem>();
            Destroy(trHit.gameObject, ps.startLifetime);
        }
    }
    
    string GetSavePath()
    {
        return Application.persistentDataPath;
    }

    //保存游戏,总是以最新版本保存
    public bool SaveGame()
    {
        //本地玩家信息保存
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if(localPlayer != null)
        {
            string path = GetSavePath();
            localPlayer.Save(path);
        }
        return true;
    }

    //根据角色名加载游戏,根据文件头判断保存的版本
    public bool LoadGame(string playerName, string worldName)
    {
        //本地玩家信息保存
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            string file = GetSavePath() + "/" + playerName;
            localPlayer.Load(file);
        }
        return true;
    }

}
