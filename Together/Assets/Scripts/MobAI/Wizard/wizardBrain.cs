using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wizardBrain : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rbody;

    private GameObject player;

    stateTypes currentState = stateTypes.WAIT;

    public baseCharacter wizard;

    public float waitDistance = 2.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        player = GameController.Instance.Player;

        wizard = new baseCharacter();
        wizard.CharacterName = "Cabbar-Wizard";

    }



    private void Update()
    {
        if (Mathf.Abs(player.transform.position.x - transform.position.x) > waitDistance)
        {
            currentState = stateTypes.WAIT;

        }else
        {
            currentState = stateTypes.ATTACK;
        }

        StateEngine();
    }
    


    void StateEngine()
    {
        switch (currentState)
        {
            case stateTypes.WAIT:
                rbody.velocity = new Vector2(0, rbody.velocity.y);
                anim.Play("idle");
                break;

            case stateTypes.ATTACK:
                anim.Play("walk");
                break;
        }
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
