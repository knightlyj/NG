using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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
                localPlayer.onDestroy += OnPlayerDestroy;
            }
        }
        return localPlayer;
    }

    private static void OnPlayerDestroy()
    {
        localPlayer.onDestroy -= OnPlayerDestroy;
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
    
    static public UIMouseItem GetUIMouseItem()
    {
        return GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().mouseItem;
    }

    //显示tips
    static UIItemTips tips = null;
    static public void ShowTips(Item item, UIItemTips.ShowPrice showPrice = UIItemTips.ShowPrice.None)
    {
        if (tips == null)
            tips = GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().itemTips;
        tips.ShowTips(item, showPrice);
    }

    //显示对话框
    static public void ShowDialog(string content, string[] options, UIDialogOpt.OnOptionChosen optionChosen)
    {
        GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().ShowDialog(content, options, optionChosen);
    }

    //关闭对话框
    static public void HideDialog()
    {
        GameObject.FindWithTag(TextResources.gamePlayUITag).GetComponent<GamePlayUI>().HideDialog();
    }

    //找到transform所属的窗口,并将窗口移动到前面
    static public void MoveWndToFront(Transform transform)
    {
        GameWindow wnd = null;
        Transform tr = transform;
        do
        {   //找到包含GameWindow组件的节点
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

    //找到level manager
    static public LevelManager GetLevelManager()
    {
        GameObject goManager = GameObject.FindGameObjectWithTag(TextResources.gameManagerTag);
        return goManager.GetComponent<LevelManager>();
    }

    //东西丢到玩家旁边
    static public void DropItemByPlayer(Item item)
    {

    }
    
    //方向换算成角度
    static public float Dir2Angle(Vector2 dir)
    {
        float angle = Mathf.Acos(dir.x / dir.magnitude) / Mathf.PI * 180;
        if (dir.y < 0)
            angle = -angle;
        return angle;
    }

    //找到所有玩家
    static public List<Player> FindAllPlayers()
    {
        List<Player> players = new List<Player>();

        GameObject local = GameObject.FindGameObjectWithTag(TextResources.localPlayerTag);
        players.Add(local.GetComponent<Player>());

        GameObject[] remote = GameObject.FindGameObjectsWithTag(TextResources.remotePlayerTag);
        foreach (GameObject go in remote)
        {
            players.Add(go.GetComponent<Player>());
        }
        return players;
    }

    //RT输出到文件
    public static void DumpRenderTexture(RenderTexture rt, string pngOutPath)
    {
        var oldRT = RenderTexture.active;

        var tex = new Texture2D(rt.width, rt.height);
        RenderTexture.active = rt;
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        tex.Apply();

        File.WriteAllBytes(pngOutPath, tex.EncodeToPNG());
        RenderTexture.active = oldRT;
    }
}



