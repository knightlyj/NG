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
    public Vector2 CameraSize { get { return this._cameraSize; } }
    Vector2 mapSize;


    Camera occlusionCamera = null;
    RenderTexture shadowMap = null;
    RenderTexture occlusionMap = null;
    RenderTexture lightMap = null;
    Material shadowMapMat = null;
    Material lightMapMat = null;

    // Use this for initialization
    void Start()
    {
        CalcCameraSize();
        GetMapSize();

        occlusionCamera = transform.FindChild("OccCamera").GetComponent<Camera>();
        occlusionMap = new RenderTexture(occlusionCamera.pixelWidth, occlusionCamera.pixelHeight, 0, RenderTextureFormat.ARGB32);
        occlusionMap.DiscardContents();
        occlusionCamera.targetTexture = occlusionMap;

        shadowMap = new RenderTexture(360, 1, 0, RenderTextureFormat.ARGB32);
        shadowMapMat = new Material(Shader.Find("Custom/ShadowMap"));

        lightMap = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0, RenderTextureFormat.ARGB32);
        lightMapMat = new Material(Shader.Find("Custom/LightMap"));
    }

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

    // Update is called once per frame
    void Update()
    {
        LocalPlayer localPlayer = Helper.FindLocalPlayer();
        if (localPlayer != null)
        {
            FollowPlayer(localPlayer);
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            

            RenderLightMap();
            DumpRenderTexture(occlusionMap, "D:\\occ.png");
            DumpRenderTexture(lightMap, "d:\\a.png");
        }


        PointLightParam light = new PointLightParam();
        light.position = transform.position;
        light.position.x -= 0.5f;
        light.range = 1.5f;
        light.color = Color.white;
        AddPointLight(light);

        PointLightParam light2 = new PointLightParam();
        light2.position = transform.position;
        light2.position.x += 0.5f;
        light2.range = 1.5f;
        light2.color = Color.red;

        AddPointLight(light2);
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

    
    List<PointLightParam> pointLights = new List<PointLightParam>();

    public void AddPointLight(PointLightParam light)
    {
        pointLights.Add(light);
    }

    void OnPreRender()
    {
        RenderLightMap();
    }

    void RenderLightMap()
    {
        ClearRenderTexture(lightMap);
        ClearRenderTexture(occlusionMap);
        occlusionCamera.RenderWithShader(Shader.Find("Custom/OccMap"), "OccType");  //需要替换shader

        Shader.SetGlobalVector("leftBottom", new Vector4(transform.position.x - _cameraSize.x, transform.position.y - _cameraSize.y, 0, 0));
        Shader.SetGlobalVector("rightTop", new Vector4(transform.position.x + _cameraSize.x, transform.position.y + _cameraSize.y, 0, 0));

        Shader.SetGlobalVector("cameraLB", new Vector4(transform.position.x - _cameraSize.x * 0.5f, transform.position.y - _cameraSize.y * 0.5f));
        Shader.SetGlobalVector("cameraSize", new Vector4(_cameraSize.x, _cameraSize.y, 0, 0));

        foreach (PointLightParam light in pointLights)
        {
            Shader.SetGlobalFloat("lightResolution", 100.0f);
            Shader.SetGlobalFloat("lightRange", light.range);
            Shader.SetGlobalVector("lightWorldPos", new Vector4(light.position.x, light.position.y, 0, 0));

            ClearRenderTexture(shadowMap);
            Graphics.Blit(occlusionMap, shadowMap, shadowMapMat);

            Shader.SetGlobalColor("lightColor", light.color);
            Graphics.Blit(shadowMap, lightMap, lightMapMat);
        }
        pointLights.Clear();

        Shader.SetGlobalTexture("lightMap", lightMap);
        
    }

    void ClearRenderTexture(RenderTexture rtToClear)
    {
        RenderTexture rt = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = rtToClear;
        GL.Clear(true, true, Color.clear);
        UnityEngine.RenderTexture.active = rt;
    }
}

public class PointLightParam
{
    public Vector2 position;
    public Color color;
    public float intensity;
    public float range;
}
