using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapHelper))]
public class MapHelperEditor : Editor {
    string mapName;

    public override void OnInspectorGUI()
    {
        string[] layerNames = { "Ground", "Platform", "Ladder" };
        GUILayout.BeginHorizontal();
        mapName = EditorGUILayout.TextField("地图名", mapName);
        if (GUILayout.Button("set map"))
        {
            GameObject map = GameObject.Find(mapName);
            if(map == null)
            {
                Debug.LogError("地图不存在");
                return;
            }
            map.tag = "Map";
            Helper.TravesalGameObj(map.transform, this.SetLayer);
            foreach (string s in layerNames)
            {   //遍历layer的子节点,mesh设置layer就行了,碰撞体还要额外设置一些其他属性
                Transform trLayer = map.transform.FindChild(s);
                for (int i = 0; i < trLayer.childCount; i++)
                {
                    Transform trChild = trLayer.GetChild(i);
                    trChild.gameObject.layer = LayerMask.NameToLayer(s);
                    if (trChild.name == "Collision")
                    {
                        if (s == "Platform")
                        {
                            Collider2D col = trChild.GetComponent<PolygonCollider2D>();
                            col.usedByEffector = true;

                            PlatformEffector2D effctor = trChild.gameObject.GetComponent<PlatformEffector2D>();
                            if (effctor == null)
                            {
                                effctor = trChild.gameObject.AddComponent<PlatformEffector2D>();
                                effctor.useColliderMask = false;
                            }
                        }
                        else if (s == "Ladder")
                        {
                            Collider2D col = trChild.GetComponent<PolygonCollider2D>();
                            col.isTrigger = true;
                        }
                    }
                }
            }
        }
        GUILayout.EndHorizontal();
        
    }

    void SetLayer(Transform t)
    {
        MeshRenderer mr = t.GetComponent<MeshRenderer>();
        if(mr != null)
        {
            mr.sortingLayerName = "Ground";
        }
    }

}
