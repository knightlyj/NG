using UnityEngine;
using System.Collections;

public class ShootTrail : MonoBehaviour {
    SpriteRenderer sr;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
    }

    float lifeTime = 0.1f;
    float maxLifeTime = 0.1f;
	// Update is called once per frame
	void Update () {
        if(lifeTime <= 0)
        {
            //gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        float alpha = lifeTime / maxLifeTime;
        sr.color = new Color(1, 1, 1, alpha);

        lifeTime -= Time.deltaTime;
    }

    public void ShowTtrail(Vector2 start, Vector2 end)
    {
        //子弹拖尾轨迹
        if(sr == null)
            sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(1, 1, 1, 1); //重置颜色
        Vector2 dist = end - start;
        float length = dist.magnitude;
        transform.localScale = new Vector3(length, 1, 1);
        transform.position = start;
        if (Mathf.Abs(length) <= 0.001f)
        {   //非常接近0,不要计算角度

        }
        else
        {
            float angle = Mathf.Acos(dist.x / length) / Mathf.PI * 180;
            if (dist.y < 0)
                angle = -angle;
            transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
        ShowShootSpark();
    }

    void ShowShootSpark()
    {
        ParticleSystem ps = transform.FindChild("ShootSpark").GetComponent<ParticleSystem>();
        ps.Stop();
        ps.Play();
    }
    
}
