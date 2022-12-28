using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    // 현재 장착된 Hand형 장비
    [SerializeField]
    private Hand hand;

    //상태 변수
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        TryAttack();
    }

    private void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    IEnumerator AttackCoroutine()
    {
        isAttack = true;
        hand.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(hand.attackDelayA);
        // 공격 판정 활성화
        isSwing= true;

        //공격 판정
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(hand.attackDelayB);
        // 공격 판정 비활성화
        isSwing = false;

        yield return new WaitForSeconds(hand.attackDelay - hand.attackDelayA - hand.attackDelayB);
        isAttack = false;
    }

    IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckHit())
            {
                Debug.Log(hitInfo.transform.name);
                isSwing = false;
            }
            yield return null;
        }
    }

    private bool CheckHit()
    {
        return Physics.Raycast(transform.position, transform.forward, out hitInfo, hand.range);
    }
}
