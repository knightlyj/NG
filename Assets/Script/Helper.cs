﻿using UnityEngine;
using System.Collections;

public static class Helper {
    public delegate void OperationOnGameObj(Transform t);
    public static void TravesalGameObj(Transform t, OperationOnGameObj o)
    {
        o(t);
        int childCount = t.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                Transform c = t.GetChild(i);
                TravesalGameObj(c, o);
            }
        }
    }

    const float TilePixels = 16;
    const float PixelsPerUnit = 100;
    public const float TileSize = TilePixels / PixelsPerUnit;
    public static Vector2 WorldSpace2Tile(Vector2 pos)
    {
        Vector2 tile;
        tile.x = Mathf.Ceil(pos.x / TileSize);
        tile.y = Mathf.Ceil(pos.y / TileSize);
        return tile;
    }
    

    //找到本地玩家
    static LocalPlayer localPlayer = null;
    public static LocalPlayer FindLocalPlayer()
    {
        if (localPlayer != null)
            return localPlayer;

        GameObject goPlayer = GameObject.FindGameObjectWithTag("LocalPlayer");
        if (goPlayer != null)
        {
            localPlayer = goPlayer.GetComponent<LocalPlayer>();
            if (localPlayer != null)
            {
                localPlayer.EntityDestroyEvent += OnPlayerDestroy;
            }
        }
        return localPlayer;
    }

    private static void OnPlayerDestroy()
    {
        localPlayer.EntityDestroyEvent -= OnPlayerDestroy;
        localPlayer = null;
    }

    //十六进制颜色转Color
    static public Color HexToColor(int hex)
    {
        Color c = new Color();
        c.r = ((hex & 0xff0000) >> 16) / 255.0f;
        c.g = ((hex & 0x00ff00) >> 8) / 255.0f;
        c.b = (hex & 0xff) / 255.0f;
        c.a = 1.0f;
        return c;
    }

    static public bool FloatEqual(float a, float b)
    {
        return Mathf.Abs(a - b) < 0.0001f;
    }

    static public Vector2 UnityUIPos2WindowsPos(Vector2 pos)
    {
        Vector2 wndPos = new Vector2();
        wndPos.x = pos.x;
        wndPos.y = pos.y - Screen.height;
        return wndPos;
    }

    static UIItemTips GetUIItemTips()
    {
        return GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().itemTips;
    }

    static public UIMouseItem GetUIMouseItem()
    {
        return GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().mouseItem;
    }

    static UIItemTips tips = null;
    static public void ShowTips(Item item, UIItemTips.ShowPrice showPrice = UIItemTips.ShowPrice.None)
    {
        if (tips == null)
            tips = Helper.GetUIItemTips();
        tips.ShowTips(item, showPrice);
    }

    //找到gamewindow类,并将窗口移动到前面
    static public void MoveWndToFront(Transform transform)
    {
        GameWindow wnd = null;
        Transform tr = transform;
        do
        {
            wnd = tr.GetComponent<GameWindow>();
            if(wnd != null)
            {
                wnd.transform.SetAsLastSibling();
                break;
            }
            else
            {
                tr = tr.parent;
            }
        }
        while (tr != null);
    }

    //找到manager
    static public GameManager GetManager()
    {
        GameObject goManager = GameObject.FindGameObjectWithTag("Manager");
        return goManager.GetComponent<GameManager>();
    }

    //东西丢到玩家旁边
    static public void DropItemByPlayer(Item item)
    {

    }
    
}



