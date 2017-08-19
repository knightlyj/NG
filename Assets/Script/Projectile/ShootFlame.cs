using UnityEngine;
using System.Collections;

public class ShootFlame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //初始缩放和位置有一点点随机
        lifeTime = maxLifeTime;
        float random = Random.Range(0.5f, 1);
        transform.localScale = new Vector3(random, random, 1);

        float sign = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
        const float offset = 0.03f;
        transform.localPosition += new Vector3(random * offset * sign, 0, 0);
    }

    [SerializeField]
    float lifeTime;
    const float maxLifeTime = 0.05f;
	// Update is called once per frame
	void Update () {
        //life time
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
        
    }

    public void Init(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        float angle = Mathf.Acos(direction.x) / Mathf.PI * 180;
        if (direction.y < 0)
            angle = -angle;
        angle -= 90;
        transform.eulerAngles = new Vector3(0, 0, angle);
    }
}
