using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MouseCheck : MonoBehaviour {
    GraphicRaycaster raycaster;
	// Use this for initialization
	void Start () {
        raycaster = GetComponent<GraphicRaycaster>();

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
