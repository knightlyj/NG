using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIFrontSight : MonoBehaviour {
    Image imgFrontSight;

    void Awake()
    {
        imgFrontSight = GetComponent<Image>();
    }

    
    // Use this for initialization
    void Start () {
	
	}

    float turnAround = 0;
    
    // Update is called once per frame
    void Update () {

        //turn around
        turnAround += Time.deltaTime * 180;
        imgFrontSight.transform.eulerAngles = new Vector3(0, 0, turnAround);

        //zoom
        Zooming();
    }

    float curZoom = 1.0f;
    float toZoom = 1.0f;
    bool needZooming = false;
    public void SetZoom(float zoom)
    {
        this.toZoom = zoom;
        if (toZoom != curZoom)
            needZooming = true;
    }

    void Zooming()
    {
        if (needZooming)
        {
            float zoomSpd = 1.0f;
            float diff = toZoom - curZoom;
            float sign = Mathf.Sign(diff);
            float step = Mathf.Abs(diff) * Time.deltaTime * zoomSpd * sign;
            float diffAfter = curZoom + step - toZoom;

            //如果经过此步zoom后,有超过目标zoom,则设置为目标值
            if (Mathf.Sign(diffAfter) != sign)
            {
                curZoom = toZoom;
                needZooming = false;
            }
            else
            {   //还没到达目标值,步进即可
                curZoom += step;
            }
            transform.localScale = new Vector3(curZoom, curZoom, 1);
        }
    }
}
