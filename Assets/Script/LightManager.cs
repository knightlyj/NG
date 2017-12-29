using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour {
    public Texture white = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!Application.isPlaying)
        {
            Debug.Log("playing");
            Shader.SetGlobalTexture("lightMap", white);
        }
	}
}
