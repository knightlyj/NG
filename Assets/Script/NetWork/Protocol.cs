using UnityEngine;
using System.Collections;
using System.Runtime.Serialization;
using System;

[Serializable]
public class GameMsg
{
    public enum MsgType
    {
        None,
        Test,
        //client
        JoinGameReq,
        QuitGameReq,
        PlayerActionInfo,

        //server
        JoinGameRsp,
    }

    public MsgType type = MsgType.None;
    public object content = null;
}

[Serializable]
public class TestMsg1
{
    public int a = 10;
}

[Serializable]
public class TestMsg2
{
    public byte x = 11;
    public byte y = 22;
}

[Serializable]
public class JoinGameReq
{
    public JoinGameReq(string name)
    {
        this.name = name;
    }
    public string name = null;
}

[Serializable]
public class JoinGameRsp
{
    //return 
    //0-success 
    //1-already in game 
    //-1 - error
    public int success;
}

[Serializable]
public class PlayerStateInfo
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;

    public bool leftMouse;
    public bool rightMouse;

    public Vector2 position;
    public Vector2 direction;
    public Vector2 speed;
}
