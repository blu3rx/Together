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
    private float runPunchTimer = 0f;
    private float jumpCooldown = 0f;
    private float horizontal;



    bool facingRight = true;
    bool gameOver = false;

    [SerializeField] bool grounded = false;
    [SerializeField] bool isJump = false;
    [SerializeField] bool isIdle = false;
    [SerializeField] bool isWalk = false;
    [SerializeField] bool isRun = false;
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
        player.CharacterName = "Player";


        gameOver = false;
    }


    void Update()
    {

        isDead = player.isDead;
        GameController.Instance.GameOver = gameOver;

        if (jumpCooldown > 0)
            jumpCooldown -= Time.deltaTime;
        if (runPunchTimer > 0)
            runPunchTimer -= Time.deltaTime;
        if (punchTimer > 0)
            punchTimer -= Time.deltaTime;
        if (slideTimer > 0)
            slideTimer -= Time.deltaTime;


        GetComponent<animController>().AnimChanger(
        isJump, isIdle, isWalk, isRun, isPunch, isSlide, isRunPunch, isDead
        );

        DrawRay();
        MoveChecker();



        if (!isSlide)
            Facing();

       // Debug.Log(player.Health);
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

        if (!isSlide)
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
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.Joystick1Button0))
        {
            isPunch = false;
            if (horizontal != 0 && !isJump)
            {
                isRun = true;
                maxSpeed = maxRunSpeed;

            }

        }
        else
        {
            isRun = false;
            if (!isSlide)
                maxSpeed = 1f;
        }

        //run-punch
        if (Input.GetKeyDown(KeyCode.E) && isRun && !isSlide || Input.GetKeyDown(KeyCode.Joystick1Button2) && isRun && !isSlide)
        {
            isRunPunch = true;
            runPunchTimer = 0.75f;
            if (!grounded)
            {
                runPunchTimer = 0.75f;
                isJump = false;
            }
        }
        else if (runPunchTimer <= 0f)
        {
            isRunPunch = false;
        }


        //punch
        if ((Input.GetKeyDown(KeyCode.E) && !isRunPunch && !isSlide )||( Input.GetKeyDown(KeyCode.Joystick1Button2) && !isRun && !isSlide))
        {
            isPunch = true;
            maxSpeed = 0f;

            if (!grounded)
            {
                punchTimer = 0.3f;
                isJump = false;
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
        if (Input.GetKeyDown(KeyCode.UpArrow) && grounded ||
            Input.GetKeyDown(KeyCode.W) && grounded ||
            Input.GetKeyDown(KeyCode.Space) && grounded ||
            Input.GetKeyDown(KeyCode.Joystick1Button1) && grounded)
        {
            isJump = true;
            rbody.AddForce(new Vector2(0, jumpPower * 100));
            jumpCooldown = 0.2f;
            if (isSlide)
                isSlide = false;
        }


        //slide
        if (!isSlide)
        {
            if ((Input.GetKeyDown(KeyCode.Q) && isRun) || (Input.GetAxis("Slide") > 0 && isRun))
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
        }else if(slideTimer <= 0)
        {
                isSlide = false;
            
        }
    
        if (isDead)
        {
            isJump = false;
            isPunch = false;
            isRun = false;
            isIdle = false;
            isWalk = false;
            isRunPunch = false;
            isSlide = false;

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


    public float Health
    {
        get
        {
            return player.Health;
        }
        set
        {
            player.Health = value;
        }
    }

    public float Damage
    {
        get
        {
            return player.Damage;
        }
        set
        {
            player.Damage = value;
        }
    }

}
