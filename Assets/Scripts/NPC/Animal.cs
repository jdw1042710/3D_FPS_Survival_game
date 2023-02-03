using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    [SerializeField] protected string name;
    [SerializeField] protected int hp;

    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    protected float applySpeed;
    protected Vector3 direction;

    protected bool isAlive = true; // 살아있는지 여부
    protected bool isWalking; // 걷는중인지 여부
    protected bool isRunning; // 뛰는중인지 여부
    protected bool isAction; // 행동중인지 여부
    
    [SerializeField] protected float walkTime; // 걷는 모션 지속시간
    [SerializeField] protected float runtime; // 뛰는 모션 지속시간
    [SerializeField] protected float waitTime; // 기타 모션 지속시간
    
    protected float currentTime = 0;

    protected Animator animator;
    protected Rigidbody rigidbody;
    protected AudioSource audioSource;

    [SerializeField] protected AudioClip[] idleSound;
    [SerializeField] protected AudioClip hurtSound;
    [SerializeField] protected AudioClip deadSound;
    

    protected void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    protected abstract void ResetAll();

    protected void Move()
    {
        if (isWalking || isRunning)
        {
            Vector3 _rotation = Vector3.Slerp(transform.eulerAngles, new Vector3(0f, direction.y, 0f), 0.01f);
            rigidbody.MoveRotation(Quaternion.Euler(_rotation));
            rigidbody.MovePosition(transform.position + transform.forward * applySpeed * Time.deltaTime);
        }
    }

    protected void Walk()
    {
        isWalking = true;
        int id = Animator.StringToHash("Walk");
        animator.SetBool(id, isWalking);

        applySpeed = walkSpeed;
        
        currentTime = walkTime;
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
        PlaySE(hurtSound);
        int id = Animator.StringToHash("Hurt");
        animator.SetTrigger(id);
    }

    protected void Dead()
    {
        isAlive = false;
        
        PlaySE(deadSound);
        ResetAll();
        int id = Animator.StringToHash("Dead");
        animator.SetTrigger(id);
    }

    protected void PlayIdleSound()
    {
        int _index = Random.Range(0, idleSound.Length);
        PlaySE(idleSound[_index]);
    }

    protected void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
