using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour {
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
    }

    [SerializeField]
    Transform rocektPrefab = null;
    [SerializeField]
    Transform bulletPrefab = null;
    [SerializeField]
    Transform shootFlamePrefab = null;
    public enum ProjectileType
    {
        Nothing,
        BaseRocket,
        BaseBullet,
    }
    public Projectile GenProj(ProjectileType type)
    {
        Transform trProjectile = null;
        switch (type)
        {
            case ProjectileType.BaseRocket:
                trProjectile = Instantiate(this.rocektPrefab);
                break;
            case ProjectileType.BaseBullet:
                trProjectile = Instantiate(this.bulletPrefab);
                break;
        }

        Projectile prScrpit = trProjectile.GetComponent<Projectile>();
        return prScrpit;
    }
    
    public void ShowFlame(Vector2 position, Vector2 direction)
    {
        Transform trFlame = Instantiate(this.shootFlamePrefab);
        ShootFlame sfScprit = trFlame.GetComponent<ShootFlame>();
        sfScprit.Init(position, direction);
    }


    /// <summary>
    /// 保存相关
    /// </summary>
    /// <returns></returns>
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
