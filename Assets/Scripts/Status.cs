using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Status : MonoBehaviour
{
    //체력
    [ReadOnly(true)] 
    public int MAX_HP; // 최대 체력
    [HideInInspector]
    public int hp; // 현재 체력
    
    //스테미나
    [ReadOnly(true)]
    public int MAX_SP; // 최대 스테미나
    [HideInInspector]
    public int sp; // 현재 스테미나
    [ReadOnly(true)]
    public int spRecoveryAmount; // 스테미나 회복 양
    [ReadOnly(true)]
    public int spRecoveryDelayTime; // 스테미나 회복 딜레이

    //방어력
    [ReadOnly(true)]
    public int MAX_DP; // 최대 방어력
    [HideInInspector]
    public int dp; // 현재 방어력

    //배고픔
    [ReadOnly(true)]
    public int MAX_HUNGRY; // 최대 배고픔
    [HideInInspector]
    public int hungry; // 현재 베고픔
    [ReadOnly(true)]
    public int hungryDecreaseTime; // 배고픔 감소 시간간격

    //목마름
    [ReadOnly(true)]
    public int MAX_THIRSTY; // 최대 목마름
    [HideInInspector]
    public int thirsty; // 현재 목마름
    [ReadOnly(true)]
    public int thirstyDecreaseTime; // 목마름 감소 시간간격

    //만족도
    [ReadOnly(true)]
    public int MAX_SATISFY; // 최대 만족도
    [HideInInspector]
    public int satisfy; // 현재 만족도
    
}
