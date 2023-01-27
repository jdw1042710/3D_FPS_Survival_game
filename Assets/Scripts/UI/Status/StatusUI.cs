using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    enum StatusType
    {
        HP = 0,
        DP,
        SP,
        HUNGRY,
        THIRSTY,
        SATISFY,
        MAX_COUNT,
    }

    [SerializeField]
    private Status status;
    
    [SerializeField] 
    private Image[] statusImages;
    
    private void Update()
    {
        statusImages[(int)StatusType.HP].fillAmount = (float)status.hp / status.MAX_HP;
        statusImages[(int)StatusType.DP].fillAmount = (float)status.dp / status.MAX_DP;
        statusImages[(int)StatusType.SP].fillAmount = (float)status.sp / status.MAX_SP;
        statusImages[(int)StatusType.HUNGRY].fillAmount = (float)status.hungry / status.MAX_HUNGRY;
        statusImages[(int)StatusType.THIRSTY].fillAmount = (float)status.thirsty / status.MAX_THIRSTY;
        statusImages[(int)StatusType.SATISFY].fillAmount = (float)status.satisfy / status.MAX_SATISFY;

    }
}
