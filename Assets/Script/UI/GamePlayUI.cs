using UnityEngine;
using System.Collections;

public class GamePlayUI : MonoBehaviour {
    public UIItemTips itemTips;
    public UIMouseItem mouseItem;

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
    }
}
