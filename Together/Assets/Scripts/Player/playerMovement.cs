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
    [SerializeField] bool isJump = false;
    [SerializeField] bool grounded = false;
    [SerializeField] bool isIdle = false;
    [SerializeField] bool isWalk = false;
    [SerializeField] bool isRun = false;
    [SerializeField] bool isPunch = false;
    [SerializeField] bool isSlide = false;
    [SerializeField] bool isRunPunch = false;
    [SerializeField] bool isDead = false;

    public baseCharacter player;

    stateTypes currentState = stateTypes.IDLE;

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
        gameOver = GameController.Instance.GameOver;

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
        AnimChecker();

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
        if (horizontal == 0 && !isRun && !isJump && !isPunch)
        {
            isIdle = true;
        }
        else isIdle = false;

        //walk
        if (horizontal != 0 && !isJump && !isRun && !isRunPunch && !isPunch && !isSlide)
        {
            isIdle = false;
            isWalk = true;
            maxSpeed = 1f;
        }
        else
        {
            isWalk = false;
        }

        //run
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isPunch)
        {
            if (horizontal != 0 && !isJump)
            {
                isRun = true;
                maxSpeed = maxRunSpeed;

            }

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;
            if (!isSlide)
                maxSpeed = 1f;
        }
        //run-punch
        if (Input.GetKeyDown(KeyCode.E) && isRun && !isSlide)
        {
            isRunPunch = true;
            runPunchTimer = 0.75f;
        }
        else if (runPunchTimer <= 0f)
        {
            isRunPunch = false;
        }


        //punch
        if (Input.GetKeyDown(KeyCode.E) && !isRunPunch && !isSlide)
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

        //jump
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            isJump = true;
            rbody.AddForce(new Vector2(0, jumpPower * 100));
            jumpCooldown = 0.2f;

        }


        //slide
        if (Input.GetKeyDown(KeyCode.Q) && isRun)
        {
            isSlide = true;
            rbody.AddForce(new Vector2(slidePower * 100, 0));
            slideTimer = 0.75f;
            maxSpeed = 3;
        }
        else if (slideTimer <= 0)
        {
            isSlide = false;
        }


    }

   void AnimChecker()
    {
        if (isIdle)
        {
            currentState = stateTypes.IDLE;
        }
        if (isWalk)
        {
            currentState = stateTypes.WALK;
        }
        if (isRun)
        {
            currentState = stateTypes.RUN;
        }
        if (isRunPunch)
        {
            currentState = stateTypes.RUNPUNCH;
        }
        if (isPunch)
        {
            currentState = stateTypes.PUNCH;
        }
        if (isJump)
        {
            currentState = stateTypes.JUMP;
        }
        if (isSlide)
        {
            currentState = stateTypes.SLİDE;
        }

    }

    enum stateTypes
    {
        IDLE,
        WALK,
        RUN,
        RUNPUNCH,
        PUNCH,
        JUMP,
        SLİDE
    }

    void AnimUpdate()
    {
        switch (currentState)
        {
            case stateTypes.IDLE:
                anim.Play("idle");
                break;
            case stateTypes.WALK:
                anim.Play("walk");
                break;
            case stateTypes.RUN:
                anim.Play("run");
                break;
            case stateTypes.RUNPUNCH:
                anim.Play("run-punch");
                break;
            case stateTypes.PUNCH:
                anim.Play("punch");
                break;
            case stateTypes.JUMP:
                anim.Play("jump");
                break;
            case stateTypes.SLİDE:
                anim.Play("slide");
                break;


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
