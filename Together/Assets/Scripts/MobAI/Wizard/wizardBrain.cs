using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wizardBrain : MonoBehaviour
{
    stateTypes currentState = stateTypes.WAIT;

    private Animator anim;
    private Rigidbody2D rbody;
    private SpriteRenderer rendr;


   
    private Transform player;
    public baseCharacter wizard;
    public GameObject projectile;


    private float walkTimer = -0.5f;
    public float waitDistance = 2.5f;
    public float attackDistance = 1f;
    public float walkTime = 0.5f;
    public float walkSpeed = 0.5f;
    public float stopDistance = 0.3f;
    private float timeBtwShots;
    public float startTimeBtwShots;

    private bool isFire = false;

    private void Awake()
    {
     
        wizard = new baseCharacter();
        wizard.CharacterName = "Cabbar-Wizard";

    }
    private void Start()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rendr = GetComponent<SpriteRenderer>();
        player = GameController.Instance.Player.transform;


        timeBtwShots = startTimeBtwShots;
    }



    private void Update()
    {

        if (walkTimer >= 0)
            walkTimer -= Time.deltaTime;

        StateSelector();

    }

    void StateSelector()
    {

        if (Mathf.Abs(player.transform.position.x - transform.position.x) > waitDistance)
        {

            currentState = stateTypes.WAIT;
            anim.Play("idle");

        }
        if (Mathf.Abs(player.transform.position.x - transform.position.x) < attackDistance)
        {
            currentState = stateTypes.ATTACK;
        }


        switch (currentState)
        {
            case stateTypes.WAIT:
                rbody.velocity = new Vector2(0, rbody.velocity.y);
                break;
            case stateTypes.ATTACK:
                Attack();
                break;

        }
    }


    void Attack()
    {
        // Debug.Log("Vur gavura!!");

        if (rbody.velocity.magnitude > 0 && !isFire)
            anim.Play("walk");

        if (Vector2.Distance(transform.position, player.position) > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, walkSpeed * Time.deltaTime);
        }
        else
            anim.Play("idle");


        //TODO: ateş etme burada toplar için objectpool yapılıcak
        if (timeBtwShots <= 0)
        {
            anim.Play("attack");
            Instantiate(projectile,transform.GetChild(0).position,Quaternion.identity);
            timeBtwShots = startTimeBtwShots;
        }
        else
        {
            timeBtwShots -= Time.deltaTime;
        }





        if (player.transform.position.x - transform.position.x < 0)
            rendr.flipX = true;
        else
            rendr.flipX = false;

    }
    



    enum stateTypes
    {
        WAIT,
        ATTACK
    }

    public float Health
    {
        get
        {
            return wizard.Health;
        }
        set
        {
            wizard.Health = value;
        }
    }

    public float Damage
    {
        get
        {
            return wizard.Damage;
        }
        set
        {
            wizard.Damage = value;
        }
    }


}
