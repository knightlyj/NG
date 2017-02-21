using UnityEngine;
using System.Collections;
using System;

public enum PlayerControl_MainState
{
    OnGround,
    InAir,
}

public class Player : MonoBehaviour {
    //role properties
    public CharacterProperties properties = new CharacterProperties();
    public BuffModule buffModule = new BuffModule();

    public PlayerInput input;
    private Transform groundCheck1;
    private Transform groundCheck2;
    public bool isLocalPlayer = true;
    Animator animator;
    Transform trMainHand;
    Transform trOffHand;
    // Use this for initialization
    void Start () {
        groundCheck1 = transform.Find("GroundCheck1");
        groundCheck2 = transform.Find("GroundCheck2");
        animator = GetComponent<Animator>();
        trMainHand = transform.FindChild("Body").FindChild("MainHand");
        trOffHand = transform.FindChild("Body").FindChild("OffHand");

        SetUpBodySR();

    }

    bool grounded = true;
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            UpdateLocalInput();
        }

        //Debug.Log("grounded: " + grounded);
        this.buffModule.UpateBuff(Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (Time.timeScale <= 0.01f)
                Time.timeScale = 1;
            else
                Time.timeScale = 0;
        }
    }
    
    void FixedUpdate()
    {

    }
    
    

    void UpdateLocalInput()
    {
        if (isLocalPlayer)
        {   //update input from
            if (Input.GetKey(KeyCode.W))
                this.input.up = true;
            else
                this.input.up = false;

            if (Input.GetKey(KeyCode.A))
                this.input.left = true;
            else
                this.input.left = false;

            if (Input.GetKey(KeyCode.S))
                this.input.down = true;
            else
                this.input.down = false;

            if (Input.GetKey(KeyCode.D))
                this.input.right = true;
            else
                this.input.right = false;

            if (Input.GetKey(KeyCode.Space))
                this.input.jump = true;
            else
                this.input.jump = false;
        }
    }

    double jumpInterval = 500; //ms
    DateTime jumpTime = DateTime.Now;
    void CheckJump()
    {

    }

    bool isCrossPlatform = false;
    void CheckCrossPlatform()
    {
        
    }

    DateTime lastShootTime = DateTime.Now;
    void CheckShoot()
    {
        this.input.targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        DamageDate damage = new DamageDate();
        damage.physicalDamage = 10;
        damage.knockBack = 200;
        if (Input.GetMouseButton(0))
        {
            TimeSpan span = DateTime.Now - lastShootTime;
            if(span.TotalMilliseconds > 200)
            {
                ProjectileSpawner ps = GameObject.FindWithTag("ProjectileSpawner").GetComponent<ProjectileSpawner>();
                ps.Shoot(ProjectileSpawner.ProjectileType.Arrow, this.gameObject, transform.position, this.input.targetPos, 1, damage);
                lastShootTime = DateTime.Now;
            }
        }

        if (Input.GetMouseButton(1))
        {
            TimeSpan span = DateTime.Now - lastShootTime;
            if (span.TotalMilliseconds > 200)
            {
                ProjectileSpawner ps = GameObject.FindWithTag("ProjectileSpawner").GetComponent<ProjectileSpawner>();
                ps.Shoot(ProjectileSpawner.ProjectileType.Bullet, this.gameObject, transform.position, this.input.targetPos, 1, damage);
                lastShootTime = DateTime.Now;
            }
        }
    }

    void UpdateAnimation(Rigidbody2D rb)
    {
        /*if (grounded)
        {
            if (Mathf.Abs(rb.velocity.x) > 0.01f)
                animator.SetBool("Run", true);
            else
                animator.SetBool("Run", false);

            animator.SetFloat("VerticleSpeed", 0);
        }
        else
        {
            animator.SetBool("Run", false);
            animator.SetFloat("VerticleSpeed", rb.velocity.y);
        }*/

    }

    void UpdateArmDir() //武器瞄准方向
    {
        Vector2 direction = this.input.targetPos - (Vector2)this.transform.position;
        direction.Normalize();
        float angle = Mathf.Acos(direction.x) / Mathf.PI * 180;
        if (direction.y < 0)
            angle = -angle;

        //朝向在正面的180度范围内
        if (angle >= 90)
            angle = 180 - angle;
        else if (angle <= -90)
            angle = -180 - angle;

        //朝向左边时会导致角度反转,没详细了解原因
        if (!faceRight)
            angle = -angle;

        trMainHand.rotation = Quaternion.Euler(0, 0, angle);
        trOffHand.rotation = Quaternion.Euler(0, 0, angle);
    }

    bool faceRight = true;
    void UpdateFace()
    {
        Vector2 direction = this.input.targetPos - (Vector2)this.transform.position;
        bool change = false;
        if(direction.x >= 0 && !faceRight)
            change = true;
        else if(direction.x < 0 && faceRight)
            change = true;
        if (change)
        {
            faceRight = !faceRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }


    SpriteRenderer[] bodySRs;
    void SetUpBodySR()
    {
        bodySRs = new SpriteRenderer[7];
        bodySRs[0] = transform.FindChild("Body").GetComponent<SpriteRenderer>();
        bodySRs[1] = transform.FindChild("Body").FindChild("FrontLeg").GetComponent<SpriteRenderer>();
        bodySRs[2] = transform.FindChild("Body").FindChild("BackLeg").GetComponent<SpriteRenderer>();
        bodySRs[3] = transform.FindChild("Body").FindChild("Head").GetComponent<SpriteRenderer>();
        bodySRs[4] = transform.FindChild("Body").FindChild("MainHand").GetComponent<SpriteRenderer>();
        bodySRs[5] = transform.FindChild("Body").FindChild("OffHand").GetComponent<SpriteRenderer>();
        bodySRs[6] = transform.FindChild("Body").FindChild("MainHand").FindChild("Gun").GetComponent<SpriteRenderer>();
    }
    public void SetAlpha(float alpha)
    {
        foreach(SpriteRenderer sr in bodySRs)
        {
            Color newColor = sr.color;
            newColor.a = alpha;
            sr.color = newColor;
        }
    }
    
    public void HitByOther(DamageDate damage, Vector2 pos)
    {
        
        Vector2 dir = (Vector2)transform.position - pos;
        dir.Normalize();

        buffModule.AddBuff(this, BuffId.Invincible); 
        this.properties.DealDamage(damage);
    }
}
