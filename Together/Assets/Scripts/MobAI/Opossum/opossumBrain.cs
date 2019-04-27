using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opossumBrain : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rbody;
    private SpriteRenderer rendr;

    stateTypes currentState = stateTypes.WAIT;

    private GameObject player;
    public baseCharacter opossum;

    private float walkTimer = -0.5f;
    public float walkTime = 0.5f;
    public float waitDistance=1f;
    public float walkSpeed = 0.5f;

    [SerializeField] private bool walkLeft = false;


    private void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rendr = GetComponent<SpriteRenderer>();
        player = GameController.Instance.Player;

        opossum = new baseCharacter();
        opossum.Damage = 500;
        opossum.Health = 100;
        opossum.CharacterName = "Opposum";
        
    }

    private void Update()
    {
        if (walkTimer >= 0)
            walkTimer -= Time.deltaTime;
       //  Debug.Log(walkLeft);

    

        if (Mathf.Abs( player.transform.position.x - transform.position.x)> waitDistance)
        {
            currentState = stateTypes.WAIT;
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            anim.Play("idle");
        }
        else
        {
            currentState = stateTypes.WALK;
        }

      //  Debug.Log(currentState);

        if (currentState == stateTypes.WALK)
        {
            Walk();
            if (walkTimer <= 0)
            {
                walkTimer = walkTime;
                if (walkLeft)
                    walkLeft = false;
                else if (!walkLeft)
                    walkLeft = true;
            }
                
        }
    }

    void Walk()
    {
        anim.Play("walk");

        if (walkLeft) {
            rbody.velocity = new Vector2(-walkSpeed, rbody.velocity.y);
            rendr.flipX = false;
        }
        else{
            rbody.velocity = new Vector2(walkSpeed, rbody.velocity.y);
            rendr.flipX = true;
        }
           

    }

    enum stateTypes
    {
        WAIT,
        WALK
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag=="Player")
            col.gameObject.GetComponent<playerMovement>().player.Hit(opossum.Damage);
    }


}
