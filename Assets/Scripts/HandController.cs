using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public static bool isActivated = false;
    // ���� ������ Hand�� ���
    [SerializeField]
    private Hand hand;

    //���� ����
    private bool isAttack = false;
    private bool isSwing = false;

    private RaycastHit hitInfo;


    // Update is called once per frame
    void Update()
    {
        if (isActivated) 
        {
            TryAttack();
        }
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
        // ���� ���� Ȱ��ȭ
        isSwing= true;

        //���� ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(hand.attackDelayB);
        // ���� ���� ��Ȱ��ȭ
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

    // ���� ��ü
    public void ChangeHand(Hand _hand)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        hand = _hand;
        WeaponManager.currentWeapon = hand.transform;
        WeaponManager.currentWeaponAnim = hand.anim;

        hand.transform.localPosition = Vector3.zero;
        hand.gameObject.SetActive(true);
    }
}
