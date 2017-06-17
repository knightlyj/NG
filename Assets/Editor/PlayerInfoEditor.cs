using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.IO;

//[CustomEditor(typeof(PlayerInfo))]
class PlayerInfoEditor : Editor
{
    PlayerInfo info;
    void OnEnable()
    {
        info = target as PlayerInfo;
    }

    public override void OnInspectorGUI()
    {


    }
    
}
