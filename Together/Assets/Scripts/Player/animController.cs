using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animController : MonoBehaviour
{
    private Animator anim;

    stateTypes currentState = stateTypes.IDLE;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }


    private void Update()
    {
        AnimUpdate();
        
        
    }
    enum stateTypes
    {
        IDLE,
        WALK,
        RUN,
        RUNPUNCH,
        PUNCH,
        JUMP,
        SLIDE,
        DEAD
    }

    public void AnimChanger (bool isJump,bool isIdle, bool isWalk,bool isRun,bool isPunch,bool isSlide,bool isRunPunch,bool isDead)
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
            currentState = stateTypes.SLIDE;
        }
        if (isDead)
        {
            currentState = stateTypes.DEAD;

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.3f)
            {
                GameController.Instance.GameOver = true;
            }
        }
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
            case stateTypes.SLIDE:
                anim.Play("slide");
                break;
            case stateTypes.DEAD:
                anim.Play("dead");
                break;


        }

    }


}
