using UnityEngine;
using System.Collections;

public static class GameSetting
{
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
    public static double CPInterval = 500; //ms
    
}


public static class GameConstValu
{
    public const int MaterialOffset = 0;
    public const int ConsumableOffset = 10000;
    public const int WeaponOffset = 20000;
    public const int ArmorOffset = 30000;
}

