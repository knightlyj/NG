using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GamePlayUI : MonoBehaviour {
    public UIItemTips itemTips;
    public UIMouseItem mouseItem;
    public UIDynamicCursor dynCursor;

    [SerializeField]
    GameWindow craftWnd = null;
    [SerializeField]
    GameWindow bagWnd = null;
    [SerializeField]
    GameWindow console = null;
    [SerializeField]
    GameWindow equipWnd = null;
    [SerializeField]
    GameWindow storeWnd = null;

    void Awake()
    {
    }

    // Use this for initialization
    void Start () {
        this.SetCursor(CursorState.Normal);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(GameSetting.openBagWnd)) //背包
        {
            bagWnd.SwitchShowAndHide();
        }
        if (Input.GetKeyDown(GameSetting.openCraftWnd)) //制造
        {
            craftWnd.SwitchShowAndHide();
        }
        if (Input.GetKeyDown(GameSetting.openEquipWnd)) //装备
        {
            equipWnd.SwitchShowAndHide();
        }


        if (Input.GetKeyDown(GameSetting.openConsole)) //控制台
        {
            console.SwitchShowAndHide();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        { //close all windows
            bagWnd.HideWnd();
            craftWnd.HideWnd();
            equipWnd.HideWnd();

            console.HideWnd();
        }

        UpdateCursor(); //更新鼠标外观
    }


    private void UpdateCursor()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {  //在UI上
            GlobalVariable.mouseOnUI = true;
        }
        else
        { //没有在UI上
            GlobalVariable.mouseOnUI = false;
        }

        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            if (localPlayer.aiming)
            {
                this.SetCursor(CursorState.Shoot);
            }
            else
            {
                if (!GlobalVariable.mouseOnUI)
                {
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (Physics2D.OverlapCircle(pos, 0.1f, LayerMask.NameToLayer(TextResources.NPCLayer)) != null)
                    {
                        this.SetCursor(CursorState.Talk);
                    }
                    else
                    {
                        //Debug.Log("nothing")
                        this.SetCursor(CursorState.Normal);
                    }
                }
            }
        }
    }

    //******************cursor etc**********************
    public enum CursorState
    {
        Normal,
        Talk,
        Shoot,
    }
    [SerializeField]
    Texture2D normalCursor = null;
    [SerializeField]
    Texture2D talkCursor = null;

    private void SetCursor(CursorState state, float frontSight = 0)
    {
        switch (state)
        {
            case CursorState.Normal:
                Cursor.SetCursor(normalCursor, Vector2.zero, CursorMode.Auto);
                dynCursor.SetCursorState(UIDynamicCursor.CursorState.Hidden);
                Cursor.visible = true;
                break;
            case CursorState.Talk:
                Cursor.SetCursor(talkCursor, Vector2.zero, CursorMode.Auto);
                dynCursor.SetCursorState(UIDynamicCursor.CursorState.Hidden);
                Cursor.visible = true;
                break;
            case CursorState.Shoot:
                dynCursor.SetCursorState(UIDynamicCursor.CursorState.FrontSight);
                Cursor.visible = false;
                break;
        }
    }

}
