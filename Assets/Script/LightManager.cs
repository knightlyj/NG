using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO 点光源只受到ground遮挡
public class PointLightParam
{
    public Vector2 position;
    public Color color;
    public float range;
}

public class SpotLightParam
{
    public Vector2 position;
    public Color color;
    public float range;
    public float direction;
    public float angle; //外围从一半亮度开始衰减,中心是全亮度
    public float centerAngle; 
}

[ExecuteInEditMode]
public class LightManager : MonoBehaviour {
    public Texture white = null;

    Camera occlusionCamera = null;
    RenderTexture shadowMap = null;
    RenderTexture occlusionMap = null;
    RenderTexture lightMap = null;
    RenderTexture blurMap = null;
    Material shadowMapMat = null;
    Material lightMapMat = null;
    Material blurMat = null;
    // Use this for initialization
    void Start () {
        occlusionCamera = transform.FindChild("OccCamera").GetComponent<Camera>();
        occlusionMap = new RenderTexture(occlusionCamera.pixelWidth, occlusionCamera.pixelHeight, 0, RenderTextureFormat.ARGB32);
        occlusionMap.DiscardContents();
        occlusionCamera.targetTexture = occlusionMap;

        shadowMap = new RenderTexture(360, 1, 0, RenderTextureFormat.ARGB32);
        shadowMapMat = new Material(Shader.Find("Custom/ShadowMap"));

        lightMap = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0, RenderTextureFormat.ARGB32);
        lightMapMat = new Material(Shader.Find("Custom/LightMap"));

        blurMap = new RenderTexture(Camera.main.pixelWidth, Camera.main.pixelHeight, 0, RenderTextureFormat.ARGB32);
        blurMat = new Material(Shader.Find("Custom/Blur"));
        blurMat.SetVector("_TextureSize", new Vector4(1024, 768));

        SetAmbient(Color.white * 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
        if (Application.isPlaying)
        {
            //PointLightParam light = new PointLightParam();
            //light.position = transform.position;
            //light.position.x -= 2f;
            //light.range = 1.5f;
            //light.color = Color.cyan;
            //AddPointLight(light);

            //PointLightParam light2 = new PointLightParam();
            //light2.position = transform.position;
            //light2.position.x += 2f;
            //light2.range = 1.5f;
            //light2.color = Color.red;

            //AddPointLight(light2);
        }
	}

    public void SetAmbient(Color ambient)
    {
        Shader.SetGlobalColor("ambient", ambient);
    }

    List<SpotLightParam> spotLights = new List<SpotLightParam>();
    List<PointLightParam> pointLights = new List<PointLightParam>();

    /// <summary>
    /// 增加聚光灯
    /// </summary>
    /// <param name="light">光源参数</param>
    public void AddSpotLight(SpotLightParam light)
    {
        spotLights.Add(light);
    }

    /// <summary>
    /// 移除聚光灯
    /// </summary>
    /// <param name="light"></param>
    public void RemoveSpotLight(SpotLightParam light)
    {
        spotLights.Remove(light);
    }

    /// <summary>
    /// 增加点光源
    /// </summary>
    /// <param name="light">光源参数</param>
    public void AddPointLight(PointLightParam light)
    {
        pointLights.Add(light);
    }

    /// <summary>
    /// 移除点光源
    /// </summary>
    /// <param name="light"></param>
    public void RemovePointLight(PointLightParam light)
    {
        pointLights.Remove(light);
    }

    void OnPreRender()
    {
        if (Application.isPlaying)
            RenderLightMap();
        else
            Shader.SetGlobalTexture("lightMap", white);
    }

    void RenderLightMap()
    {
        ClearRenderTexture(lightMap);
        ClearRenderTexture(occlusionMap);
        occlusionCamera.RenderWithShader(Shader.Find("Custom/OccMap"), "OccType");  //使用替换shader渲染

        CameraControl cc = GetComponent<CameraControl>();
        //设置shader的全局参数
        Shader.SetGlobalVector("leftBottom", new Vector4(transform.position.x - cc.cameraSize.x, transform.position.y - cc.cameraSize.y, 0, 0));
        Shader.SetGlobalVector("rightTop", new Vector4(transform.position.x + cc.cameraSize.x, transform.position.y + cc.cameraSize.y, 0, 0));

        Shader.SetGlobalVector("cameraLB", new Vector4(transform.position.x - cc.cameraSize.x * 0.5f, transform.position.y - cc.cameraSize.y * 0.5f));
        Shader.SetGlobalVector("cameraSize", new Vector4(cc.cameraSize.x, cc.cameraSize.y, 0, 0));
        
        //渲染点光
        foreach (PointLightParam light in pointLights)
        {
            Shader.SetGlobalFloat("lightResolution", 100.0f);
            Shader.SetGlobalFloat("lightRange", light.range);
            Shader.SetGlobalVector("lightWorldPos", new Vector4(light.position.x, light.position.y, 0, 0)); //z为0表示点光

            ClearRenderTexture(shadowMap);
            Graphics.Blit(occlusionMap, shadowMap, shadowMapMat);//根据遮挡图渲染出阴影图

            Shader.SetGlobalColor("lightColor", light.color);
            Graphics.Blit(shadowMap, lightMap, lightMapMat); //根据阴影图,渲染叠加到光照图
        }
        
        //渲染聚光灯
        foreach (SpotLightParam light in spotLights)
        {
            Shader.SetGlobalFloat("lightResolution", 100.0f);
            Shader.SetGlobalFloat("lightRange", light.range);
            Shader.SetGlobalVector("lightWorldPos", new Vector4(light.position.x, light.position.y, 1, 0));  //z为1表示聚光灯
            Shader.SetGlobalFloat("lightDirection", light.direction);
            Shader.SetGlobalFloat("lightAngle", light.angle);
            Shader.SetGlobalFloat("lightCenterAngle", light.centerAngle);


            ClearRenderTexture(shadowMap);  
            Graphics.Blit(occlusionMap, shadowMap, shadowMapMat);  //根据遮挡图渲染出阴影图

            Shader.SetGlobalColor("lightColor", light.color);
            Graphics.Blit(shadowMap, lightMap, lightMapMat);   //根据阴影图,渲染叠加到光照图
        }
        

        Graphics.Blit(lightMap, blurMap, blurMat); //这里每个像素都会重绘,且不叠加,所以不用清理
        Shader.SetGlobalTexture("lightMap", blurMap);
    }

    void ClearRenderTexture(RenderTexture rtToClear)
    {
        RenderTexture rt = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = rtToClear;
        GL.Clear(true, true, Color.black);
        UnityEngine.RenderTexture.active = rt;
    }
}
