using UnityEngine;
using System.Collections;

public struct MyTransform
{
    public FpVector2 position; //position in world space
    public int rotation; //rotation on z-axis
}

public class MyGameObject
{
    public MyTransform transform;
}    
