
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianScript : MonoBehaviour
{
    public float velocity = 1f, velocityRun = 2f, velocityJump = 250f, TimeOfChangeDirection = 3f, TimeOfChangeDirectionNow = 0f;
    public float RaycastDistance = 0.28f;
    public bool BOOLIsJumped = false, BOOLIsAttack = false, BOOLDirection = false, ChangeDirection = false, BOOLWhenChangeDirection = false, BOOLWhenATTACKChangeDirection=false;
    public baseCharacter barbarianKalitim;
    Vector3 ScaleOfStart;

    //AI
    public bool BOOLAiJump = false, BOOLAiAttack = false, BOOLAiLeft = false, BOOLAiRight = false, BOOLAiRun = false,BOOLAiAttackAble=false;
    public float DistanceOfAttack = 2f, DistanceOfAttackAraligi = 0.4f;
    GameObject GOPlayer;
  public  bool BOOLPlayereYaklas = false, BOOLPlayerDirection = false;
    //--

    public bool BOOLAiRightGETSET
    {
        get { return BOOLAiRight; }
        set
        {
            if (value == false)
            {
                gameObject.GetComponent<Animator>().SetBool("Walk", false);
                gameObject.GetComponent<Animator>().SetBool("Idle", true);
                gameObject.GetComponent<Animator>().SetBool("Run", false);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            BOOLAiRight = value;
        }
    }
    public bool BOOLAiLeftGETSET
    {
        get { return BOOLAiLeft; }
        set
        {
            if (value == false)
            {
                gameObject.GetComponent<Animator>().SetBool("Walk", false);
                gameObject.GetComponent<Animator>().SetBool("Idle", true);
                gameObject.GetComponent<Animator>().SetBool("Run", false);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            BOOLAiLeft = value;
        }
    }
    public bool BOOLAiRunGETSET
    {
        get { return BOOLAiRun; }
        set
        {
            if (value == false)
            {
                gameObject.GetComponent<Animator>().SetBool("Run", false);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);

            }
            BOOLAiRun = value;
        }
    }
    void Awake()
    {
        barbarianKalitim = new baseCharacter();

        barbarianKalitim.Damage = 20f;

        barbarianKalitim.Health = 60f;
        barbarianKalitim.CharacterName = "barbarian";
        BOOLAiRightGETSET = false;
        BOOLAiLeftGETSET = false;
        GOPlayer = GameController.Instance.Player.gameObject;
        ScaleOfStart = transform.localScale;
        gameObject.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().sortingLayerName = "PlayerLayer";
           
    }

    public void HitFunction()
    {
        gameObject.GetComponent<Animator>().SetBool("Attack", false);
        gameObject.GetComponent<Animator>().SetBool("Idle", true);
        gameObject.GetComponent<Animator>().SetBool("Jump", false);
        gameObject.GetComponent<Animator>().SetBool("Run", false);
        gameObject.GetComponent<Animator>().SetBool("Walk", false);
        gameObject.GetComponent<Animator>().SetTrigger("HitTrigger");
    }
    void FNBarbarianIsDead()
    {
        gameObject.GetComponent<Animator>().SetBool("Attack", false);
        gameObject.GetComponent<Animator>().SetBool("Idle", false);
        gameObject.GetComponent<Animator>().SetBool("Jump", false);
        gameObject.GetComponent<Animator>().SetBool("Run", false);
        gameObject.GetComponent<Animator>().SetBool("Walk", false);
        gameObject.GetComponent<Animator>().SetBool("Die", true);
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        Destroy(gameObject, 5f);
    }
    void FNBarbarianAttack()
    {
       
        gameObject.GetComponent<Animator>().SetBool("Attack", true);
        BOOLIsAttack = true;
        gameObject.GetComponent<Animator>().SetBool("Idle", true);
        gameObject.GetComponent<Animator>().SetBool("Jump", false);
        gameObject.GetComponent<Animator>().SetBool("Run", false);
        gameObject.GetComponent<Animator>().SetBool("Walk", false);
        
    }
    void FNBarbarianJump()
    {
        
        gameObject.GetComponent<Animator>().SetBool("Jump", true);
        gameObject.GetComponent<Animator>().SetBool("Idle", false);
        BOOLIsJumped = true;
        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0.0f);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, velocityJump * 10000));
    }
    void FNBarbarianAfterJump()
    {
        RaycastHit2D RaycastForJumpHit = Physics2D.Raycast(transform.position, Vector2.down, RaycastDistance, (1 << LayerMask.NameToLayer("Ground")));

        if (RaycastForJumpHit.collider != null)
        {
            BOOLIsJumped = false;
            BOOLAiJump = false;
            gameObject.GetComponent<Animator>().SetBool("Jump", false);
            gameObject.GetComponent<Animator>().SetBool("Idle", true);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);
        }
    }
    void Update()
    {
        if (BOOLAiLeft && BOOLAiRight)
        {
            BOOLAiLeftGETSET = false;
            BOOLAiRightGETSET = false;
        }
   
        if (ChangeDirection)
        {
            TimeOfChangeDirectionNow += Time.deltaTime;

            if (TimeOfChangeDirection <= TimeOfChangeDirectionNow || BOOLWhenChangeDirection != BOOLPlayerDirection)
            {

                ChangeDirection = false;
                TimeOfChangeDirectionNow = 0f;
                BOOLDirection = !BOOLDirection;
                ScaleOfStart = transform.localScale;
                ScaleOfStart.x *= -1;
                transform.localScale = ScaleOfStart;
               transform.GetChild(1).gameObject.active = false;
            }

        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            BOOLAiLeftGETSET = true;
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            BOOLAiRightGETSET = true;
        }
        if (Input.GetKeyUp(KeyCode.J))
        {
            BOOLAiRightGETSET = false;
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            BOOLAiLeftGETSET = false;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            BOOLAiRunGETSET = true;
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            BOOLAiRunGETSET = false;
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (BOOLAiAttackAble)
            BOOLAiAttack = true;

        }
        if (barbarianKalitim.isDead)
        {
            FNBarbarianIsDead();
        }

        if (BOOLIsAttack && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1)//Eğer Attack Animasonu Bitmişse
        {
            BOOLIsAttack = false;
            BOOLAiAttack = false;
            gameObject.GetComponent<Animator>().SetBool("Attack", false);
        }

        if (BOOLAiAttack && !barbarianKalitim.isDead && !BOOLIsAttack)//Attack
        {
            
            FNBarbarianAttack();

        }
        if (BOOLIsJumped && BOOLAiJump && !barbarianKalitim.isDead)//Zıplama Sonrası
        {
            FNBarbarianAfterJump();
        }
        if (BOOLAiJump && !BOOLIsJumped && !BOOLIsAttack && !barbarianKalitim.isDead)
        {
            FNBarbarianJump();

        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            BOOLAiJump = true;
        }


        #region Left-right

        if (BOOLAiRightGETSET && !BOOLIsAttack && !barbarianKalitim.isDead && !ChangeDirection)
        {

            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            if (BOOLAiRunGETSET)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * velocityRun, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("Run", true);
                gameObject.GetComponent<Animator>().SetBool("Walk", false);
            }
            else
            {
                gameObject.GetComponent<Animator>().SetBool("Walk", true);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * velocity, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (BOOLDirection)
            {
                ChangeDirection = true;
                gameObject.transform.GetChild(1).gameObject.active = true;
                gameObject.transform.GetChild(1).gameObject.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
                BOOLAiRightGETSET = false;
            }

        }
        else if (BOOLAiLeftGETSET && !BOOLIsAttack && !barbarianKalitim.isDead && !ChangeDirection)
        {

            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            if (BOOLAiRunGETSET)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * -velocityRun, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                gameObject.GetComponent<Animator>().SetBool("Run", true);
                gameObject.GetComponent<Animator>().SetBool("Walk", false);

            }

            else
            {
                gameObject.GetComponent<Animator>().SetBool("Walk", true);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(Time.deltaTime * -velocity, gameObject.GetComponent<Rigidbody2D>().velocity.y);
            }
            if (!BOOLDirection)
            {
                ChangeDirection = true;
                gameObject.transform.GetChild(1).gameObject.active = true;
                gameObject.transform.GetChild(1).gameObject.transform.localScale = new Vector3(transform.localScale.x, 1, 1);
                BOOLAiLeftGETSET = false;
            }

        }
        #endregion

    }
    void FixedUpdate()
    {
        //AI
        if (!barbarianKalitim.isDead)
        {
            if (GOPlayer.transform.position.x >= transform.position.x)//Player BArbarianın hangi tarafında kalıyor?
                BOOLPlayerDirection = true;
            else
                BOOLPlayerDirection = false;
        } 
        if (Mathf.Abs(Mathf.Abs(GOPlayer.transform.position.x) - Mathf.Abs(transform.position.x)) <= DistanceOfAttack 
            && (DistanceOfAttackAraligi < Mathf.Abs(Mathf.Abs(GOPlayer.transform.position.x) - Mathf.Abs(transform.position.x))||(0.4f > Mathf.Abs(Mathf.Abs(GOPlayer.transform.position.y) - Mathf.Abs(transform.position.y)))))//Player Atack Bölgesinde mi? || Çok da yaklaşma. SAldırı Payı bırak
        {
            if (!BOOLAiAttack) { 
            if (GOPlayer.transform.position.x >= transform.position.x)//Player BArbarianın hangi tarafında kalıyor?
                BOOLAiRightGETSET = true;
            else
                BOOLAiLeftGETSET = true;
            }
            

            if (!ChangeDirection)
                BOOLWhenChangeDirection = BOOLPlayerDirection;
        }

        else
        {
            BOOLPlayereYaklas = false;
            BOOLAiRightGETSET = false;
            BOOLAiLeftGETSET = false;
            //BOOLWhenChangeDirection = BOOLPlayerDirection;
        }
     
        //if (BOOLPlayereYaklas)
        //{
        //    if (GOPlayer.transform.position.x >= transform.position.x)//Player BArbarianın hangi tarafında kalıyor?
        //       BOOLAiRightGETSET = true;
        //    else
        //       BOOLAiLeftGETSET = true;

          
                
        //}
        if (DistanceOfAttackAraligi >= Mathf.Abs(Mathf.Abs(GOPlayer.transform.position.x) - Mathf.Abs(transform.position.x))
            && !ChangeDirection && !BOOLIsAttack 
            && 0.4f > Mathf.Abs(Mathf.Abs(GOPlayer.transform.position.y) - Mathf.Abs(transform.position.y)))//0.5f = Yüksekliği
        {
            if (BOOLWhenChangeDirection != BOOLPlayerDirection)
            {
                ChangeDirection = false;
                TimeOfChangeDirectionNow = 0f;
                BOOLDirection = !BOOLDirection;
                ScaleOfStart = transform.localScale;
                ScaleOfStart.x *= -1;
                transform.localScale = ScaleOfStart;
                gameObject.transform.GetChild(1).gameObject.active = false;
            }
            BOOLWhenChangeDirection = BOOLPlayerDirection;
            if (BOOLAiAttackAble)
             BOOLAiAttack = true;
           
        }

        //--
    }
    

}
