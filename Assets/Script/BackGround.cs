using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class BackGround : MonoBehaviour {
    Transform near = null;
    Transform far = null;
    Transform farthest = null;

    void Awake()
    {
        near = transform.FindChild("Near");
        far = transform.FindChild("Far");
        farthest = transform.FindChild("Farthest");
    }

    // Use this for initialization
    void Start () {
        CameraControl cf = GameObject.FindWithTag("MainCamera").GetComponent<CameraControl>();
        cf.CameraResizeEvent += this.OnCameraResize;

        if (near == null || far == null || farthest == null)
        {
            Debug.LogError("BackGround.Start >> background null");
        }
        farBasePos = far.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (cameraFollow != null)
        {
            //最远场景,直接跟随镜头移动
            farthest.position = (Vector2)cameraFollow.transform.position;

            //远景,按比例移动
            if (basePosX > 0 && slideRangeX > 0)
            {
                Vector2 cameraUv = cameraFollow.GetUvInMap();
                float rateX = cameraUv.x * slideRangeX + basePosX;
                float rateY = cameraUv.y * slideRangeY + basePosY;
                //SpriteRenderer sr = far.GetComponent<SpriteRenderer>();
                //Rect textureRect = sr.sprite.textureRect;
                float offsetX = farSize.x * (0.5f - rateX);
                float offsetY = farSize.y * (0.5f - rateY);
                far.position = new Vector2(cameraFollow.transform.position.x + offsetX, farBasePos.y + offsetY * 0.5f);

                //Debug.Log("uv " + cameraUv);
            }
            //近景直接与地图同比例,可在tiledMap中编辑
        }
    }

    CameraControl cameraFollow = null;
    void OnCameraResize(object sender)
    {
        CameraControl cf = sender as CameraControl;
        cameraFollow = cf;
        SetBgSize();
    }

    float basePosX, basePosY; //必须都大于0
    float slideRangeX, slideRangeY;
    Vector2 farSize;
    Vector2 farBasePos;
    void SetBgSize()
    {
        //Vector2 cameraSize = cameraFollow.CameraSize;

        //远景
        SpriteRenderer sr = far.GetComponent<SpriteRenderer>();
        Rect textureRect = sr.sprite.textureRect;
        this.farSize = new Vector2(textureRect.width / sr.sprite.pixelsPerUnit * far.localScale.x, 
            textureRect.height / sr.sprite.pixelsPerUnit * far.localScale.y);
        float rateX = cameraFollow.CameraSize.x / farSize.x;
        float rateY = cameraFollow.CameraSize.y / farSize.y;
        if (rateX > 1 || rateY > 1)
            Debug.LogError("BackGround.SetBgSize >> size rate less than 1");

        basePosX = rateX / 2;
        slideRangeX = 1 - rateX;

        basePosY = rateY / 2;
        slideRangeY = 1 - rateY;
    }
}
