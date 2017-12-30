using UnityEngine;
using System.Collections;
using Tiled2Unity;
using System;
using System.IO;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class CameraControl : MonoBehaviour
{
    public delegate void CameraResizeCB(object sender);
    public event CameraResizeCB CameraResizeEvent;

    Vector2 _cameraSize;
    public Vector2 cameraSize { get { return this._cameraSize; } }
    Vector2 mapSize;
    
    void Awake()
    {
        CalcCameraSize();
        GetMapSize();
    }

    // Use this for initialization
    void Start()
    {
        

        
    }

    // Update is called once per frame
    void Update()
    {
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            FollowPlayer(localPlayer);
        }
        
    }

    
    void CalcCameraSize()
    {
        float orthographicSize = Camera.main.orthographicSize;
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        float cameraHeight = orthographicSize * 2;
        float cameraWidth = cameraHeight * aspectRatio;
        _cameraSize = new Vector2(cameraWidth, cameraHeight);

        if (CameraResizeEvent != null)
            CameraResizeEvent(this);

        
    }

    void GetMapSize()
    {
        GameObject mapGo = GameObject.FindWithTag("Map");
        if (mapGo != null)
        {
            TiledMap map = mapGo.GetComponent<TiledMap>();
            mapSize = new Vector2(map.MapWidthInPixels * map.ExportScale, map.MapHeightInPixels * map.ExportScale);
        }
    }

    //float followSpeed = 15.0f;
    void FollowPlayer(Player player)
    {
        //Vector2 dist = player.position - transform.position;
        //float length = dist.magnitude;
        //float followLength = followSpeed * Time.deltaTime;
        //if (length <= followLength)
        //{
        //    transform.position = new Vector3(player.position.x, player.position.y, transform.position.z);
        //}
        //else
        //{
        //    dist.Normalize();
        //    dist *= followLength;
        //    transform.position += (Vector3)dist;
        //}
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.transform.position.z);
    }


    public Vector2 GetUvInMap()
    {
        Vector2 uv = new Vector2();
        uv.x = transform.position.x / mapSize.x;
        uv.y = transform.position.y / mapSize.y;
        return uv;
    }

    
    
}


