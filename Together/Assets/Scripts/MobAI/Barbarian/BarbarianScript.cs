using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianScript : MonoBehaviour
{
    public float velocity = 1f, velocityRun = 2f,velocityJump=200f;
    public float RaycastDistance=0.28f;
    bool BOOLIsJumped = false,BOOLIsAttack=false;
    public baseCharacter barbarianKalitim;
    void Awake()
    {
        barbarianKalitim = new baseCharacter();
        barbarianKalitim.Damage = 50f;
        barbarianKalitim.Health = 500f;
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
   
    void Update()
    {
        if (barbarianKalitim.isDead)
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
        if (BOOLIsAttack && GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >1)//Eğer Attack Animasonu Bitmişse
        {
            BOOLIsAttack = false;
            gameObject.GetComponent<Animator>().SetBool("Attack", false);
        }
        if (Input.GetKeyDown(KeyCode.T) && !barbarianKalitim.isDead)//Attack
        {
            gameObject.GetComponent<Animator>().SetBool("Attack", true);
            BOOLIsAttack = true;
            gameObject.GetComponent<Animator>().SetBool("Idle", true);
            gameObject.GetComponent<Animator>().SetBool("Jump", false);
            gameObject.GetComponent<Animator>().SetBool("Run", false);
            gameObject.GetComponent<Animator>().SetBool("Walk", false);
        }

        if (BOOLIsJumped && !barbarianKalitim.isDead)//Zıplama
        {
            RaycastHit2D RaycastForJumpHit = Physics2D.Raycast(transform.position, Vector2.down, RaycastDistance, (1 << LayerMask.NameToLayer("Ground")));
           
            if (RaycastForJumpHit.collider!=null)
            {
                BOOLIsJumped = false;
                gameObject.GetComponent<Animator>().SetBool("Jump", false);
                gameObject.GetComponent<Animator>().SetBool("Idle", true);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(gameObject.GetComponent<Rigidbody2D>().velocity.x, 0);  
            }
        }

        if (Input.GetKeyDown(KeyCode.Y) && !BOOLIsJumped && !BOOLIsAttack && !barbarianKalitim.isDead)
        {
            gameObject.GetComponent<Animator>().SetBool("Jump", true);
            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            BOOLIsJumped = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, velocityJump));
        }
        if (Input.GetKey(KeyCode.J) && !BOOLIsAttack && !barbarianKalitim.isDead)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            if (Input.GetKey(KeyCode.Tab))
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

        }
        if (Input.GetKey(KeyCode.G) && !BOOLIsAttack && !barbarianKalitim.isDead)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            if (Input.GetKey(KeyCode.Tab))
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

        }
   
      
        
        
        if (Input.GetKeyUp(KeyCode.J)||Input.GetKeyUp(KeyCode.G))
        {
            gameObject.GetComponent<Animator>().SetBool("Walk", false);
            gameObject.GetComponent<Animator>().SetBool("Idle", true);
            gameObject.GetComponent<Animator>().SetBool("Run", false);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gameObject.GetComponent<Rigidbody2D>().velocity.y);


        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            gameObject.GetComponent<Animator>().SetBool("Run", false);
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,gameObject.GetComponent<Rigidbody2D>().velocity.y);


        }
    }
  
  
}
