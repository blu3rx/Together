using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rbody;

    

    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    public float maxRunSpeed = 3;
    public float jumpPower = 2f;
    public float slidePower = 3f;

    private float slideTimer = 0f;
    private float punchTimer = 0f;
    private float runPunchTimer=0f;
    private float jumpCooldown = 0f;
    private float horizontal;



    bool facingRight = true;
    bool gameOver = false;
    [SerializeField] bool headHit = false;
    [SerializeField] bool isJump = false;
    [SerializeField] bool grounded = false;
    [SerializeField] bool isIdle = false;
    [SerializeField] bool isWalk = false;
    [SerializeField] bool isRun = false;
    [SerializeField] bool isCrounch = false;
    [SerializeField] bool isCrouchWalk = false;
    [SerializeField] bool isPunch = false;
    [SerializeField] bool isSlide = false;
    [SerializeField] bool isRunPunch = false;
    [SerializeField] bool isDead = false;

    public baseCharacter player;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();

        player = new baseCharacter();

        player.Health = 100f;
        player.Damage = 200f;
        player.CharacterName = "Player";
        gameOver = false;
    }


    void Update()
    {
        isDead = player.isDead;
        if (jumpCooldown > 0)
            jumpCooldown -= Time.deltaTime;
        if (runPunchTimer > 0)
            runPunchTimer -= Time.deltaTime;
        if (punchTimer > 0)
            punchTimer -= Time.deltaTime;
        if (slideTimer > 0)
            slideTimer -= Time.deltaTime;




        DrawRay();
        MoveChecker();
        AnimUpdate();
        if(!isSlide)
         Facing();
    }


    void FixedUpdate()
    {
        FixedInputUpdate();
    }


    void DrawRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.31f, (1 << 9));
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.down * 0.31f), Color.red, 1f);
        //RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.up, 0.31f, (1 << 9));
        //Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.up * 0.31f), Color.yellow, 1f);

        if (hit.collider != null && jumpCooldown <= 0)
        {
            grounded = true;
            isJump = false;
        }
        else grounded = false;
    }



    void FixedInputUpdate()
    {
        if(!isSlide)
            horizontal = Input.GetAxisRaw("Horizontal");
        rbody.velocity = new Vector2(horizontal * moveSpeed, rbody.velocity.y);

        if (Mathf.Abs(rbody.velocity.x) > maxSpeed)
        {
            Vector2 clamped = new Vector2(Mathf.Clamp(rbody.velocity.x, -maxSpeed, maxSpeed), rbody.velocity.y);
            rbody.velocity = clamped;
        }
    }


    void MoveChecker()
    {
        //idle 
        if (horizontal == 0 && !isRun &&!isJump&&!isPunch)
        {
            isIdle = true;
        }
        else isIdle = false;

        //yürüme 
        if (horizontal != 0&&!isJump&&!isRun&&!isRunPunch&&!isPunch&&!isSlide)
        {
            isIdle = false;
            isWalk = true;
            maxSpeed = 1f;
        }
        else
        {
            isWalk = false;
        }

        //koşma
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isPunch)
        {
            if (horizontal != 0&&!isJump)
            {
                isRun = true;
                maxSpeed = maxRunSpeed;
          
            }
            
        }else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            if(!isSlide)
                maxSpeed = 1f;
        }
        //run-punch
        if (Input.GetKeyDown(KeyCode.E)&&isRun&&!isSlide)
        {
            isRunPunch = true;
            runPunchTimer = 0.75f;
        }
        else if (runPunchTimer <= 0f)
        {
            isRunPunch = false;
        }


        //punch
        if (Input.GetKeyDown(KeyCode.E)&&!isRunPunch&&!isSlide)
        {
            isPunch = true;
            maxSpeed = 0f;
            if (!grounded)
            {
                punchTimer = 0.5f;
            }
            else
            {
                punchTimer = 0.75f;
            }


        }
        else if (punchTimer <= 0)
        {
            isPunch = false;
        }

        //zıplama
        if (Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            isJump = true;
            rbody.AddForce(new Vector2(0, jumpPower * 100));
            jumpCooldown = 0.2f;
            
        }


        //slide
        if (Input.GetKeyDown(KeyCode.Q)&&isRun)
        {
            isSlide = true;
            rbody.AddForce(new Vector2(slidePower* 100, 0));
            slideTimer = 0.75f;
            maxSpeed = 3;
        }else if (slideTimer <= 0)
        {
            isSlide = false;
        }

    }

    void AnimUpdate()
    {
        if (isIdle)
        {
            anim.Play("idle");
            isRun = false;
        }

        if (isWalk&&!isRun)
        {
            anim.Play("walk");
        }

        if (isRun)
        {
            isWalk = false;
            isIdle = false;
            anim.Play("run");
        }

        if (isRunPunch)
        {
            isRun = false;
            isIdle = false;
            isWalk = false;
            isJump = false;
            anim.Play("run-punch");
        }

        if (isPunch)
        {
            isRun = false;
            isIdle = false;
            isWalk = false;
            isJump = false;
            anim.Play("punch");
        }

        if (isJump)
        {
            isRun = false;
            isIdle = false;
            isWalk = false;
            anim.Play("jump");
        }

        if (isSlide)
        {
            isRun = false;
            isIdle = false;
            isWalk = false;
            isJump = false;
            anim.Play("slide");
        }



    }











    void Facing()
    {
        if (horizontal > 0 && !facingRight)
            Flip();
        else if (horizontal < 0 && facingRight)
            Flip();
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


}
