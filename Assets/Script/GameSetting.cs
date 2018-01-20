using UnityEngine;
using System.Collections;

public static class GameSetting
{
    //******************玩家信息相关参数**************************
    static public string name;
    static public bool isMultiPlayer = false;
    static public bool isHost = true;
    static public string remoteServerIp = "127.0.0.1";
    static public int remoteServerPort = 8888;

    public static KeyCode up = KeyCode.W;
    public static KeyCode down = KeyCode.S;
    public static KeyCode left = KeyCode.A;
    public static KeyCode right = KeyCode.D;
    public static KeyCode jump = KeyCode.Space;


    public static double CPInterval = 500; //穿过platform的持续时间
    public static float SellPriceRate = 0.33f; //卖出价格比例
        
    public static KeyCode openCraftWnd = KeyCode.K;
    public static KeyCode openBagWnd = KeyCode.B;
    public static KeyCode openEquipWnd = KeyCode.C;
    public static KeyCode openConsole = KeyCode.F10;

    //****************屏幕分辨率相关****************************
    public const float defaultScreenwidth = 1280;
    public const float defaultScreenheight = 720;

    public const float damageFontSize = 26f;
    public const int textFontSize = 25;
    
    public const float touchThreshold = 0.01f;  //物体接触的阈值,根据整体缩放尺寸调整

    //****************控制台开关***********************
    public const bool enableConsole = true;
}


public static class GlobalVariable
{
    public static bool mouseOnUI;  //鼠标停留在UI上
    public static bool inputFieldActive; //在输入文字状态
    
}

public static class TextResources
{
    //layers
    public const string creatureLayer = "Creature";
    public const string ccpLayer = "CCP";  //creature cross platform
    public const string handleLayer = "Handle";
    public const string platformLayer = "Platform";
    public const string groundLayer = "Ground";

    //tags
    public const string localPlayerTag = "LocalPlayer";
    public const string remotePlayerTag = "RemotePlayer";
    public const string NPCTag = "NPC";

    //gameobect所用字符串
    public const string gamePlayUITag = "GamePlayUI"; //游戏场景UI的tag
    public const string gameManagerTag = "Manager"; //游戏管理器

    //资源名
    public const string defaultCraftClassIcon = "wood"; //默认合成分类图标

    //表格文件名
    public const string itemTable = "item.csv"; //物品表名
    public const string craftClassTable = "craftClass.csv"; //合成分类表名字
    public const string armorTable = "armor.csv"; //护甲属性表
    public const string weaponTable = "weapon.csv"; //武器属性表 


    //物品信息格式化
    public const string atkFormat = "攻击: {0} - {1}\n";
    public const string atkInvervalFormat = "速率: {0:N}\n";
    public const string atkSpdFormat = "攻速: {0:N}\n";
    public const string crtlChanceFormat = "暴击率: {0}%\n";
    public const string crtlRateFormat = "暴击伤害: {0}%\n";
    public const string rcrFormat = "{0}%几率不消耗弹药\n";
    public const string hpFormat = "血量: {0}\n";
    public const string defFormat = "防御 : {0}\n";
    public const string spdFormat = "速度: {0}%\n";
    public const string jmpFormat = "跳跃: {0}%\n";
}
