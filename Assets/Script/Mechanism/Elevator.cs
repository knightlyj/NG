using UnityEngine;
using System.Collections;

/// <summary>
/// 触发一次走一趟的电梯,可以设置多个目标点,走直线
/// </summary>
public class Elevator : Mechanism
{
    [SerializeField]
    Vector2[] checkPoints = null;
    int curPoint = 0;
    int destPoint = 0;

    SpriteRenderer sr = null;
    void Awake()
    {
        accStep = acceleration * Time.fixedDeltaTime; //如果fixed间隔设置改了,这里会修正
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {
        
    }

    void Update()
    {
        if (checkPoints == null)
        {
            Debug.LogError(transform.name + ": elevator's checkPoints is null");
        }
        else if (curPoint != destPoint)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Vector2 dir = checkPoints[destPoint] - (Vector2)transform.position;
            float distance = dir.magnitude;
            dir.Normalize();
            if (Mathf.Abs(distance) <= GameSetting.touchThreshold)
            {   //arrived
                transform.position = checkPoints[destPoint];
                curPoint = destPoint;
                rb.velocity = Vector2.zero;  //速度要清零
            }
            else
            {   //go on
                float v = Vector2.Dot(rb.velocity, dir);
                float deltaTime = Time.deltaTime;
                if (v < 0)
                {   //当前速度反了,朝目标方向加速
                    v += acceleration * deltaTime;
                }
                else if (v * v / acceleration >= distance)
                {   //靠近了,需要减速
                    v -= acceleration * deltaTime;
                }
                else if (v < maxSpeed)
                {   //还有一定距离,没达到最大速度,则加速
                    v += acceleration * deltaTime;
                }
                rb.velocity = dir * v; //设置速度
            }
        }
    }

    float maxSpeed = 2.5f;
    const float acceleration = 3.0f;
    float accStep = acceleration * 0.02f;  //算作50帧
    void FixedUpdate()
    {
        
    }
    
    public override bool canTrigger { get { return curPoint == destPoint; } }
    public override void Trigger()
    {
        if (!canTrigger)
            return;

        if (checkPoints == null)
        {
            Debug.LogError(transform.name + ": elevator's checkPoints is null");
        }
        else
        {
            destPoint = curPoint + 1;
            if (destPoint >= checkPoints.Length)
                destPoint = 0;
        }
    }
}