using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rbody;

    public float moveSpeed = 5f;
    public float maxSpeed = 5f;
    public float runSpeed = 10;
    public float maxRunSpeed = 3;
    public float jumpPower = 100f;
    private float jumpCooldown = 0f;
    private float horizontal = 0f;
    private float punchCooldown = 0f;
    private float runPunchCooldown = 0f;

    bool facingRight = true;
    [SerializeField] bool headHit = false;
    [SerializeField] bool jumped = false;
    [SerializeField] bool grounded = false;
    [SerializeField] bool isIdle = false;
    [SerializeField] bool isWalk = false;
    [SerializeField] bool isRun = false;
    [SerializeField] bool isCrounch = false;
    [SerializeField] bool isCrouchWalk = false;
    [SerializeField] bool isPunch = false;
    [SerializeField] bool isSlide = false;
    [SerializeField] bool isRunPunch = false;

     public baseCharacter player;

    void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();

        player = new baseCharacter();

        player.Health = 100f;
        player.Damage = 200f;
        player.CharacterName = "Player";
    }

    void Start()
    {
       
        // GameController.Instance._eagle.gameObject.GetComponent<EagleScript>().eagle.Hit(200);


    }

    void Update()
    {

        if (jumpCooldown > 0)
            jumpCooldown -= Time.deltaTime;
        if (punchCooldown > 0)
            punchCooldown -= Time.deltaTime;
        if (runPunchCooldown > 0)
            runPunchCooldown -= Time.deltaTime;


        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.31f, (1 << 9));
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.down * 0.31f), Color.red, 1f);
        RaycastHit2D hit2 = Physics2D.Raycast(transform.position, Vector2.up, 0.31f, (1 << 9));
        Debug.DrawLine(transform.position, (Vector2)transform.position + (Vector2.up * 0.31f), Color.yellow, 1f);

        if (hit.collider != null && jumpCooldown <= 0)
        {
            grounded = true;
            jumped = false;
        }
        else grounded = false;

        if (hit2.collider != null)
        {
            headHit = true;
        }
        else headHit = false;



        InputUpdate();
        CheckAnim();
        UpdateAnims();

    
    }

    void FixedUpdate()
    {

        FixedInputUpdate();
        Facing();

        


    }

    void InputUpdate()
    {


        if (Input.GetKeyDown(KeyCode.Space) && grounded && jumpCooldown <= 0) //zıplama
        {
            jumped = true;
            jumpCooldown = 0.3f;
            rbody.AddForce(new Vector2(0, jumpPower * 100));

        }


        if (Input.GetKeyDown(KeyCode.LeftShift))//koşma
        {
            isRun = true;


        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isRun = false;

        }

        if (Input.GetKeyDown(KeyCode.E)) //yumruk
        {
            isPunch = true;
            if (!grounded)
            {
                punchCooldown = 0.5f;
            }
            else
            {
                punchCooldown = 0.75f;
            }
           

        }
        else if (punchCooldown <= 0)
        {
            isPunch = false;
        }


        if (Input.GetButtonDown("Crouch") && !jumped)//eğilme
        {
            isCrounch = true;
            isWalk = false;

            if (horizontal != 0)
            {
                isCrouchWalk = true;
                isCrounch = false;
                maxSpeed = 0.5f;
            }

        }
        else if (Input.GetButtonUp("Crouch"))
        {
            isCrounch = false;
            isCrouchWalk = false;

        }
        if (isCrounch)
        {
            if (horizontal != 0 )
            {
                isCrouchWalk = true;
                maxSpeed = 0.5f;
                isCrounch = false;
            }
            if (horizontal == 0)
            {
                isCrouchWalk = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && !jumped && isRun)//kayma
        {
            
            isSlide = true;
        } else if (Input.GetKeyUp(KeyCode.Q))
        {
             isSlide = false;
            
           
        }

        if (Input.GetKeyDown(KeyCode.E) && isRun)//koşarken yumruk
        {
            isRunPunch = true;
            runPunchCooldown = 0.75f;
        }
        else if (runPunchCooldown <= 0)
        {
            isRunPunch = false;
        }

    }

    void FixedInputUpdate()
    {

        horizontal = Input.GetAxis("Horizontal");
        rbody.velocity = new Vector2(horizontal * moveSpeed,rbody.velocity.y);




        if (isRun)
        {
            maxSpeed = maxRunSpeed;
            rbody.AddForce(new Vector2(horizontal * runSpeed, 0));
        }
        if (isSlide)
        {
            maxSpeed = 2;
            rbody.AddForce(new Vector2(horizontal * runSpeed, 0));
        }
        if (isPunch)
        {
            maxSpeed = 0;
            
        }

        if (Mathf.Abs(rbody.velocity.x) > maxSpeed)
        {
            Vector2 clamped = new Vector2(Mathf.Clamp(rbody.velocity.x, -maxSpeed, maxSpeed), rbody.velocity.y);
            rbody.velocity = clamped;
        }
    }


    void CheckAnim()
    {
        if (horizontal == 0 && !jumped)// idle a geçiş
        {
            isIdle = true;
            isWalk = false;
            
        }
        if (horizontal != 0 && !jumped && !isRun && !isCrouchWalk)// yürümeye geçiş
        {
            maxSpeed = 1;
            isWalk = true;
            isIdle = false;
        }
        if (isCrounch)//eğilmeye geçiş
        {
            isIdle = false;
            isRun = false;
            isWalk = false;
        }
        if (isRun)//koşmaya geçiş
        {
            isIdle = false;
            isWalk = false;
            isCrounch = false;
            isCrouchWalk = false;
        }
        if (jumped)//zıplama
        {
            isIdle = false;
            isWalk = false;
            isRun = false;
            isCrounch = false;
            isCrouchWalk = false;
            isSlide = false;
        }
        if (isCrouchWalk)//eğilerek yürüme
        {
            isIdle = false;
        }
        if (isPunch)//yumruk
        {
            isIdle = false;
            isCrouchWalk = false;
            isCrounch = false;
            isRun = false;
            isWalk = false;
            jumped = false;
        }
        if (isSlide)//kayma
        {
            isIdle = false;
            isCrouchWalk = false;
            isCrounch = false;
            isRun = false;
            isWalk = false;
        }
        if (isRunPunch)//koşarak yumruk
        {
            isIdle = false;
            isCrouchWalk = false;
            isCrounch = false;
            isRun = false;
            isWalk = false;
            isPunch = false;
        }


  



    }

    void UpdateAnims()
    {

        if (isIdle)//idle anim
        {
            anim.Play("idle");
        }
        if (isWalk)//walk anim
        {
            anim.Play("walk");
        }
        if (jumped)
        {

            anim.Play("jump");
        }
        if (isRun)//koşmaya geçiş
        {

            anim.Play("run");
        }
        if (isCrounch)//eğilme
        {
            anim.Play("crouch");
        }
        if (isCrouchWalk)//eğilerek yürüme
        {
            anim.Play("crouch-walk");
        }
        if (isPunch)//yumruk
        {
            anim.Play("punch");
        }
        if (isSlide)//kayma
        {
            anim.Play("slide");
        }
        if (isRunPunch)
        {
            anim.Play("run-punch");
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
