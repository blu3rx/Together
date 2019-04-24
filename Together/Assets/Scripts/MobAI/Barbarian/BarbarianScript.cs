using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarianScript : MonoBehaviour
{
    public float velocity = 1f, velocityRun = 2f,velocityJump=200f;
    public float RaycastDistance=0.28f;
    bool BOOLIsJumped = false;
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            gameObject.GetComponent<Animator>().SetBool("Attack", true);
        }
      
        if (BOOLIsJumped)
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
        
        if (Input.GetKeyDown(KeyCode.Y) && !BOOLIsJumped)
        {
            gameObject.GetComponent<Animator>().SetBool("Jump", true);
            gameObject.GetComponent<Animator>().SetBool("Idle", false);
            BOOLIsJumped = true;
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, velocityJump));
        }
        if (Input.GetKey(KeyCode.J))
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
        if (Input.GetKey(KeyCode.G))
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
