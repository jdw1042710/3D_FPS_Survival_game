using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


public class Pig : MonoBehaviour
{
    [SerializeField] private string name;
    [SerializeField] private int hp;

    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    private float applySpeed;
    private Vector3 direction;

    private bool isAlive = true; // 살아있는지 여부
    private bool isWalking; // 걷는중인지 여부
    private bool isRunning; // 뛰는중인지 여부
    private bool isAction; // 행동중인지 여부
    
    [SerializeField] private float walkTime; // 걷는 모션 지속시간
    [SerializeField] private float runtime; // 뛰는 모션 지속시간
    [SerializeField] private float waitTime; // 기타 모션 지속시간
    
    private float currentTime = 0;

    private Animator animator;
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    [SerializeField] private AudioClip[] idlePigSound;
    [SerializeField] private AudioClip hurtPigSound;
    [SerializeField] private AudioClip deadPigSound;

    private enum PigState
    {
        Idle = 0,
        Eat,
        Peak,
        Walk,
        MaxCount,
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isAlive) return;
        Move();
        ElapseTime();
    }

    private void Move()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Slerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigidbody.MoveRotation(Quaternion.Euler(_rotation));
            rigidbody.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    private void ResetAll()
    {
        isWalking = false; isRunning = false; isAction = true;
        int id;
        id = Animator.StringToHash("Walk");
        animator.SetBool(id, isWalking);
        id = Animator.StringToHash("Run");
        animator.SetBool(id, isRunning);
        
        direction = new Vector3(0f, Random.Range(0f, 360f), 0f);
    }

    private void ElapseTime()
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

    private void NextAction()
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

    private void Idle()
    {
        currentTime = waitTime;
    }

    private void Eat()
    {
        int id = Animator.StringToHash("Eat");
        animator.SetTrigger(id);

        currentTime = waitTime;
    }

    private void Peak()
    {
        int id = Animator.StringToHash("Peek");
        animator.SetTrigger(id);

        currentTime = waitTime;
    }

    private void Walk()
    {
        isWalking = true;
        int id = Animator.StringToHash("Walk");
        animator.SetBool(id, isWalking);

        applySpeed = walkSpeed;
        
        currentTime = walkTime;
    }

    public void Run(Vector3 _targetPosition)
    {
        isRunning = true;
        int id = Animator.StringToHash("Run");
        animator.SetBool(id, isRunning);

        applySpeed = runSpeed;
        direction = Quaternion.LookRotation(transform.position - _targetPosition).eulerAngles;
        
        currentTime = runtime;
        
    }

    public void Damage(int _damage, Vector3 _targetPosition)
    {
        if(!isAlive) return;
        hp -= _damage;
        if (hp <= 0)
        {
            Dead();
            return;
        }
        PlaySE(hurtPigSound);
        int id = Animator.StringToHash("Hurt");
        animator.SetTrigger(id);

        ResetAll();
        Run(_targetPosition);
    }

    private void Dead()
    {
        isAlive = false;
        
        PlaySE(deadPigSound);
        ResetAll();
        int id = Animator.StringToHash("Dead");
        animator.SetTrigger(id);
    }

    private void PlayIdleSound()
    {
        int _index = Random.Range(0, idlePigSound.Length);
        PlaySE(idlePigSound[_index]);
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
