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


public static class GlobalVariable
{
    public static bool mouseOnUI;  //鼠标停留在UI上
    public static bool inputFieldActive; //在输入文字状态
}
