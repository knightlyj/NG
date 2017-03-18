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
            {
                //Transform layer = map.transform.FindChild(s);
                //Transform collision = layer.FindChild("Collision");
                //collision.gameObject.layer = LayerMask.NameToLayer(s);
                Transform collision = map.transform.FindChild(s).FindChild("Collision");
                collision.gameObject.layer = LayerMask.NameToLayer(s);
                //Collider2D col = collision.GetComponent<PolygonCollider2D>();
                //col.isTrigger = true;
                if (s == "Platform")
                {
                    Collider2D col = collision.GetComponent<PolygonCollider2D>();
                    col.usedByEffector = true;

                    PlatformEffector2D effctor = collision.gameObject.GetComponent<PlatformEffector2D>();
                    if (effctor == null)
                    {
                        effctor = collision.gameObject.AddComponent<PlatformEffector2D>();
                        effctor.useColliderMask = false;
                    }
                }
                else if (s == "Ladder")
                {
                    Collider2D col = collision.GetComponent<PolygonCollider2D>();
                    col.isTrigger = true;
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
