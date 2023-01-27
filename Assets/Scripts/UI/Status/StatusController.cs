using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusController : MonoBehaviour
{
    private Status status;
    
    private float spRecoveryDelayDeltaTime; // 마지막 스테미나 사용 ~ 현재까지 시간
    private float hungryDecreaseDeltaTime; // 마지막 배고픔 감소 ~ 현재까지 시간
    private float thirstyDecreaseDeltaTime; // 마지막 목마름 감소 ~ 현재까지 시간
    
    void Awake()
    {
        status = GetComponent<Status>();
        status.hp = status.MAX_HP;
        status.dp = status.MAX_DP;
        status.sp = status.MAX_SP;
        status.hungry = status.MAX_HUNGRY;
        status.thirsty = status.MAX_THIRSTY;
        status.satisfy = status.MAX_SATISFY;
    }

    private void FixedUpdate()
    {
        Hungry();
        Thirsty();
        RecoverSP();
    }

    private void Hungry()
    {
        if (status.hungry > 0)
        {
            if (hungryDecreaseDeltaTime > status.hungryDecreaseTime)
            {
                DecreaseHungry(1);
                hungryDecreaseDeltaTime = 0;
            }
            else
            {
                hungryDecreaseDeltaTime += Time.deltaTime;
            }
        }
    }
    
    private void Thirsty()
    {
        if (status.thirsty > 0)
        {
            if (thirstyDecreaseDeltaTime > status.thirstyDecreaseTime)
            {
                DecreaseThirsty(1);
                thirstyDecreaseDeltaTime = 0;
            }
            else
            {
                thirstyDecreaseDeltaTime += Time.deltaTime;
            }
        }
    }

    private void RecoverSP()
    {
        if (status.sp == status.MAX_SP) return;
        
        if (status.spRecoveryDelayTime > spRecoveryDelayDeltaTime)
            spRecoveryDelayDeltaTime += Time.deltaTime;
        else
            spRecoveryDelayDeltaTime = status.spRecoveryDelayTime;

        if (spRecoveryDelayDeltaTime.Equals(status.spRecoveryDelayTime))
        {
            status.sp += status.spRecoveryAmount;
            status.sp = Mathf.Min(status.sp, status.MAX_SP);
        }
            
    }

    public void IncreaseHP(int _amount)
    {
        status.hp += _amount;
        status.hp = Mathf.Min(status.hp, status.MAX_HP);
    }
    
    public void DecreaseHP(int _amount)
    {
        if (status.dp > 0)
        {
            DecreaseDP(_amount);
            return;
        }
        status.hp -= _amount;
        status.hp = Mathf.Max(status.hp, 0);
    }
    
    public void IncreaseDP(int _amount)
    {
        status.dp += _amount;
        status.dp = Mathf.Min(status.dp, status.MAX_DP);
    }
    
    public void DecreaseDP(int _amount)
    {
        status.dp -= _amount;
        status.dp = Mathf.Max(status.dp, 0);
    }
    
    public void IncreaseHungry(int _amount)
    {
        status.hungry += _amount;
        status.hungry = Mathf.Min(status.hungry, status.MAX_HUNGRY);
    }
    
    public void DecreaseHungry(int _amount)
    {
        status.hungry -= _amount;
        status.hungry = Mathf.Max(status.hungry, 0);
    }
    
    public void IncreaseThirsty(int _amount)
    {
        status.thirsty += _amount;
        status.thirsty = Mathf.Min(status.thirsty, status.MAX_THIRSTY);
    }
    
    public void DecreaseThirsty(int _amount)
    {
        status.thirsty -= _amount;
        status.thirsty = Mathf.Max(status.thirsty, 0);
    }
    
    public bool TryDecreaseStamina(int _amount)
    {
        spRecoveryDelayDeltaTime = 0;

        if (status.sp > _amount)
        {
            status.sp -= _amount;
            return true;
        }
        else
        {
            return false;
        }
    }
}
