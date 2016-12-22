using UnityEngine;
using System.Collections;
using System;

public struct PlayerInput
{
    public bool up;
    public bool down;
    public bool left;
    public bool right;
    public bool jump;
}

public enum BoundSide
{
    Top,
    Bottom,
    Left,
    Right,
}

public class Player : MonoBehaviour {
    //role properties
    float maxHorSpeed = 1.0f;
    float movForce = 10;
    float movForceInAir = 1;
    float jumpSpeed = 4;
    public CharacterProperties Properties;

    public PlayerInput input;
    private Transform groundCheck1;
    private Transform groundCheck2;
    public bool isLocalPlayer = true;
	// Use this for initialization
	void Start () {
        groundCheck1 = transform.Find("GroundCheck1");
        groundCheck2 = transform.Find("GroundCheck2");
    }

    bool grounded = true;
	// Update is called once per frame
	void Update () {
        if (isLocalPlayer)
        {
            UpdateInput();
            CheckShoot();
        }
        grounded = Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Ground"))
            || Physics2D.Linecast(transform.position, groundCheck1.position, 1 << LayerMask.NameToLayer("Platform"))
            || Physics2D.Linecast(transform.position, groundCheck2.position, 1 << LayerMask.NameToLayer("Platform"));
        
        //Debug.Log("grounded: " + grounded);
    }
    
    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //**********************movement*****************************
        //input
        if (grounded) { //在地面时,不按左右则立即停下来,或者由摩擦力停下,并且按住左右会有较大的力,摩擦力由物理引擎实现
            if (!this.input.left && !this.input.right && rb.velocity.y < 0) //有向上的速度,也不会触发立即停止或者摩擦力
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {
                if (this.input.left && rb.velocity.x > -maxHorSpeed)
                        rb.AddForce(new Vector2(-movForce, 0));
                if (this.input.right && rb.velocity.x < maxHorSpeed)
                        GetComponent<Rigidbody2D>().AddForce(new Vector2(movForce, 0));
            }
        }
        else
        {   //在空中时,无摩擦力,按住左右会有较小的力
            if (this.input.left && rb.velocity.x > -maxHorSpeed)
                rb.AddForce(new Vector2(-movForceInAir, 0));
            if (this.input.right && rb.velocity.x < maxHorSpeed)
                GetComponent<Rigidbody2D>().AddForce(new Vector2(movForceInAir, 0));
        }
        //max speed
        if (rb.velocity.x < -maxHorSpeed)
            rb.velocity = new Vector2(-maxHorSpeed, rb.velocity.y);
        else if (rb.velocity.x > maxHorSpeed)
            rb.velocity = new Vector2(maxHorSpeed, rb.velocity.y);

        
        CheckJump(); //jump
        CheckCrossPlatform(); //按住下时,要穿过platform
    }
    
    

    void UpdateInput()
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
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (grounded)
        {
            if (this.input.jump)
            {
                TimeSpan span = DateTime.Now - jumpTime;
                if (span.TotalMilliseconds > jumpInterval)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    jumpTime = DateTime.Now;
                }
            }
        }
    }

    bool isCrossPlatform = false;
    double crossPlatformInterval = 500; //ms
    DateTime crossPlatformTime = DateTime.Now;
    void CheckCrossPlatform()
    {
        //由于物理引擎在向下未完全穿过时会弹开collider,故按下后在一个较短时间内保持穿过的layer
        if (isCrossPlatform)
        {
            //Debug.Log("crossing");
            TimeSpan span = DateTime.Now - crossPlatformTime;
            if (span.TotalMilliseconds > crossPlatformInterval)
            {
                gameObject.layer = LayerMask.NameToLayer("Player");
                isCrossPlatform = false;
            }
        }
        else
        {
            if (input.down && grounded)
            {
                gameObject.layer = LayerMask.NameToLayer("PlayerCrossPlatform");
                crossPlatformTime = DateTime.Now;
                isCrossPlatform = true;
            }
        }
    }

    DateTime lastShootTime = DateTime.Now;
    void CheckShoot()
    {
        if (Input.GetMouseButton(0))
        {
            TimeSpan span = DateTime.Now - lastShootTime;
            if(span.TotalMilliseconds > 200)
            {
                ProjectileSpawner ps = GameObject.FindWithTag("ProjectileSpawner").GetComponent<ProjectileSpawner>();
                Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ps.Shoot(ProjectileSpawner.ProjectileType.Arrow, this.gameObject, transform.position, targetPos, 1);
                lastShootTime = DateTime.Now;
            }
        }

        if (Input.GetMouseButton(1))
        {
            TimeSpan span = DateTime.Now - lastShootTime;
            if (span.TotalMilliseconds > 200)
            {
                ProjectileSpawner ps = GameObject.FindWithTag("ProjectileSpawner").GetComponent<ProjectileSpawner>();
                Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                ps.Shoot(ProjectileSpawner.ProjectileType.Bullet, this.gameObject, transform.position, targetPos, 1);
                lastShootTime = DateTime.Now;
            }
        }
    }
}
