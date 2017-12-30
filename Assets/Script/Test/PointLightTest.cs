using UnityEngine;
using System.Collections;

public class PointLightTest : MonoBehaviour {
    PointLightParam pointLight;
	// Use this for initialization
	void Start () {
        pointLight = new PointLightParam();
        pointLight.position = transform.position;
        pointLight.color = Color.yellow;
        pointLight.range = 0.5f;

        LightManager lm = Camera.main.GetComponent<LightManager>();
        lm.AddPointLight(pointLight);
    }
	
	// Update is called once per frame
	void Update () {
        pointLight.position = transform.position;

    }
}
