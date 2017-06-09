using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

[CustomEditor(typeof(GamePanel))]
class GamePanelEditor : Editor
{
    GamePanel wnd;
    void OnEnable()
    {
        wnd = target as GamePanel;
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("set up"))
            SetUp();
    }

    Vector2 closeOffset = new Vector2(-5, -5);
    Vector2 closeSize = new Vector2(19, 19);
    void SetUp()
    {
        //close button
        RectTransform rectClose = wnd.transform.FindChild("Close") as RectTransform;
        rectClose.anchorMin = Vector2.one;
        rectClose.anchorMax = Vector2.one;
        rectClose.offsetMin = closeOffset - closeSize;
        rectClose.offsetMax = closeOffset;
    }
}