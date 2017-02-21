using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public struct PlayerInput
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool jump;
    public bool leftMouse;
    public bool rightMouse;

    public Vector2 targetPos;
}

public class FrameInfo
{
    public Int64 count = -1;
    public PlayerInput[] inputs = new PlayerInput[8];
}

public class FrameControl : MonoBehaviour {
    List<FrameInfo> frameInfoList = new List<FrameInfo>();
    // Use this for initialization
    void Start () {
	
	}

    // Update is called once per frame
    float totalTime = 0;
	void Update () {
        CollectFrameInfo(0.016f);


    }

    void FixedUpdate()
    {

    }

    void PlayOneFrame(FrameInfo info)
    {
        this.frameInfoList.Add(info);
    }

    //同步添加帧数据
    Int64 frameCount = -1;
    void AddFrame(FrameInfo info)
    {
        info.count = frameCount++;
        frameInfoList.Add(info);
    }

    //收集帧信息
    FrameInfo curFrame;
    float curFrameTime = 0;
    const float FrameInterval = 1.0f / 20.0f;
    void CollectFrameInfo(float deltaTime)
    {
        if (curFrame == null)
            curFrame = new FrameInfo();

        //收集本机操作


        //判断当前帧结束
        bool complete = false;
        curFrameTime += deltaTime;
        if(curFrameTime >= FrameInterval)
        {
            curFrameTime = 0;
            complete = true;
        }

        if (complete)
        {
            this.AddFrame(curFrame);
            curFrame = null;
        }
            
    }

    //*****************帧暂停和恢复*************************
    //只有animator受影响,物理逻辑由帧同步逻辑控制
    void PauseFrame()
    {
        Time.timeScale = 0;
    }

    void ResumeFrame()
    {
        Time.timeScale = 1;
    }
}
