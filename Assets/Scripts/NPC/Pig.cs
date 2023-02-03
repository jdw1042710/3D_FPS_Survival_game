using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class Pig : FrendlyAnimal
{
    private enum PigState
    {
        Idle = 0,
        Eat,
        Peak,
        Walk,
        MaxCount,
    }
    protected void Update()
    {
        if (!isAlive) return;
        Move();
        ElapseTime();
    }
    
    protected void ElapseTime()
    {
        if (isAction)
        {
            currentTime -= Time.deltaTime;
        }
        
        if (currentTime <= 0)
        {
            ResetAll();
            NextAction();
        }
    }
    
    
    protected override void ResetAll()
    {
        isWalking = false; isRunning = false; isAction = true;
        navMeshAgent.ResetPath();
        
        int id;
        id = Animator.StringToHash("Walk");
        animator.SetBool(id, isWalking);
        id = Animator.StringToHash("Run");
        animator.SetBool(id, isRunning);
        
        destination.Set(Random.Range(-movementRange, movementRange) , 0f, Random.Range(-movementRange, movementRange));
    }
    
    protected void NextAction()
    {
        PigState _nextState = (PigState)Random.Range(0, (int)PigState.MaxCount); // Idle, eat, peak, walk

        switch (_nextState)
        {
            case PigState.Idle:
                Idle();
                break;
            case PigState.Eat:
                Eat();
                break;
            case PigState.Peak:
                Peak();
                break;
            case PigState.Walk:
                Walk();
                break;
        }

        PlayIdleSound();
    }
    
    protected void Idle()
    {
        currentTime = waitTime;
    }

    protected void Eat()
    {
        int id = Animator.StringToHash("Eat");
        animator.SetTrigger(id);

        currentTime = waitTime;
    }

    protected void Peak()
    {
        int id = Animator.StringToHash("Peek");
        animator.SetTrigger(id);

        currentTime = waitTime;
    }
}
