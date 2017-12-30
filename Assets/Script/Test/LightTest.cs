using UnityEngine;
using System.Collections;

public class LightTest : MonoBehaviour
{
    SpotLightParam spotLight = null;
    // Use this for initialization
    void Start()
    {
        spotLight = new SpotLightParam();
        spotLight.position = transform.position;
        spotLight.color = Color.white;
        spotLight.range = 3f;
        spotLight.direction = transform.eulerAngles.z * Mathf.PI / 180f;
        spotLight.angle = 70f * Mathf.PI / 180f;
        spotLight.centerAngle = 30f * Mathf.PI / 180f;

        LightManager lm = Camera.main.GetComponent<LightManager>();
        lm.AddSpotLight(spotLight);
    }
    public Player owner = null;
    float z = 0;
    // Update is called once per frame
    void Update()
    {


        //float rotSpeed = 50f;
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    z += rotSpeed * Time.deltaTime;
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    z -= rotSpeed * Time.deltaTime;
        //}
        //transform.eulerAngles = new Vector3(0, 0, z);

        spotLight.position = transform.position;
        spotLight.direction = transform.eulerAngles.z * Mathf.PI / 180f;
        if (!owner.faceRight)
            spotLight.direction = Mathf.PI - spotLight.direction;
    }
}
