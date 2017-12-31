using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class GamePlayUI : MonoBehaviour
{
    public UIItemTips itemTips;
    public UIMouseItem mouseItem;
    public UIDynamicCursor dynCursor;

    [SerializeField]
    UICraftWnd craftWnd = null;
    [SerializeField]
    UIBagWnd bagWnd = null;
    [SerializeField]
    UIConsole console = null;
    [SerializeField]
    UIEquipWnd equipWnd = null;
    [SerializeField]
    UIStoreWnd storeWnd = null;
    [SerializeField]
    UIDialog dialog = null;

    void HideAllWnd()
    {
        craftWnd.HideWnd();
        bagWnd.HideWnd();
        console.HideWnd();
        equipWnd.HideWnd();
        storeWnd.HideWnd();
    }

    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {
        this.SetCursor(CursorState.Normal);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(GameSetting.openBagWnd)) //背包
        {
            bagWnd.SwitchShowAndHide();
            dialog.Hide();
        }
        if (Input.GetKeyDown(GameSetting.openCraftWnd)) //制造
        {
            craftWnd.SwitchShowAndHide();
            dialog.Hide();
        }
        if (Input.GetKeyDown(GameSetting.openEquipWnd)) //装备
        {
            equipWnd.SwitchShowAndHide();
            dialog.Hide();
        }


        if (Input.GetKeyDown(GameSetting.openConsole)) //控制台
        {
            if (GameSetting.enableConsole == true)
            {
                console.SwitchShowAndHide();
                dialog.Hide();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        { //close all windows
            HideAllWnd();
            dialog.Hide();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            ShowStore(null);
            dialog.Hide();
        }

        UpdateCursor(); //更新鼠标外观
    }
    
    ///*******************window state*************************
    public enum WndMode
    {
        HideAll,
        Store,
        Repo,
    }


    static bool firstShowStore = true;
    public void ShowStore(StoreInfo info)
    {
        if (firstShowStore)
        {   //第一次打开商店,界面摆在固定位置
            RectTransform rectStore = storeWnd.GetComponent<RectTransform>();
            RectTransform rectBag = bagWnd.GetComponent<RectTransform>();
            rectStore.anchoredPosition = new Vector2(250, 620);
            rectBag.anchoredPosition = new Vector2(250, 315);
            firstShowStore = false;
        }
        //显示窗口并设置商店数据
        storeWnd.ShowWnd();
        storeWnd.SetStoreInfo(info);
        bagWnd.ShowWnd();

        //隐藏对话框
        dialog.Hide();
    }

    public void ShowRepo()
    {

    }

    public void HideDialog()
    {
        dialog.Hide();
    }

    public void ShowDialog(string content, string[] options, UIDialogOpt.OnOptionChosen optionChosen)
    {
        HideAllWnd();
        dialog.ShowDialog(content, options, optionChosen);
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
                {   //鼠标没在UI上,检测鼠标覆盖的物体
                    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Collider2D[] hits = Physics2D.OverlapCircleAll(pos, 0.1f, 1 << LayerMask.NameToLayer(TextResources.CreatureLayer) | 1 << LayerMask.NameToLayer(TextResources.CCPLayer));
                    bool onNPC = false;
                    foreach(Collider2D hit in hits)
                    {
                        MonsterBase monster = hit.GetComponent<MonsterBase>();
                        if(monster != null && monster.faction >= 0)
                        {
                            onNPC = true;
                            if (Input.GetMouseButtonDown(1)) 
                            {  //有点击右键,则与NPC对话
                                monster.TalkWith();
                            }
                        }
                    }
                    if (onNPC)
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
}
