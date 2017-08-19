using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIDynamicCursor : MonoBehaviour
{
    public enum CursorState
    {
        Hidden,
        FrontSight,
        Loading,
    }
    
    UIFrontSight frontSight = null;
    void Awake()
    {
        frontSight = transform.FindChild("FrontSight").GetComponent<UIFrontSight>();
    }

    public float zoom = 1.0f;
    // Use this for initialization
    void Start()
    {
        SetCursorState(CursorState.Hidden);
    }


    // Update is called once per frame
    void Update()
    {
        
        this.transform.position = Input.mousePosition;

        //if (Input.GetMouseButtonDown(0)) {
        //    zoom += 0.5f;
        //}
        //else
        //{
        //    zoom -= 3f * Time.deltaTime;
        //    if (zoom < 1.0f)
        //        zoom = 1.0f;
        //}
        //frontSight.SetZoom(zoom);
    }

    //rate用来设置准星缩放和loading进度
    public void SetCursorState(CursorState state, float rate = 0)
    {   //内部没有记录状态,每次调用都更新,开销不大,省事
        switch (state)
        {
            case CursorState.Hidden:
                frontSight.gameObject.SetActive(false);
                break;
            case CursorState.FrontSight:
                frontSight.gameObject.SetActive(true);
                frontSight.SetZoom(rate + 1.0f);
                break;
            case CursorState.Loading:
                frontSight.gameObject.SetActive(false);
                break;
        }
        
    }
}
