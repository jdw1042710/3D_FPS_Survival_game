using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    // 현재 장착된 Hand형 장비
    [SerializeField]
    protected MeleeWeapon meleeWeapon;

    //상태 변수
    protected bool isAttack = false;
    protected bool isSwing = false;

    protected RaycastHit hitInfo;

    protected void TryAttack()
    {
        if (Input.GetButton("Fire1"))
        {
            if (!isAttack)
            {
                StartCoroutine(AttackCoroutine());
            }
        }
    }

    protected IEnumerator AttackCoroutine()
    {
        isAttack = true;
        meleeWeapon.anim.SetTrigger("Attack");
        yield return new WaitForSeconds(meleeWeapon.attackDelayA);
        // 공격 판정 활성화
        isSwing= true;

        //공격 판정
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(meleeWeapon.attackDelayB);
        // 공격 판정 비활성화
        isSwing = false;

        yield return new WaitForSeconds(meleeWeapon.attackDelay - meleeWeapon.attackDelayA - meleeWeapon.attackDelayB);
        isAttack = false;
    }

    protected IEnumerator HitCoroutine()
    {
        while (isSwing)
        {
            if (CheckHit())
            {
                isSwing = false;
                HitAction();
            }
            yield return null;
        }
    }

    protected bool CheckHit()
    {
        return Physics.Raycast(transform.position, transform.forward, out hitInfo, meleeWeapon.range);
    }

    protected virtual void HitAction()
    {
        Debug.Log(hitInfo.transform.name);
    }

    // 무기 교체
    public virtual void ChangeMeleeWeapon(MeleeWeapon _meleeWeapon)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        meleeWeapon = _meleeWeapon;
        WeaponManager.currentWeapon = meleeWeapon.transform;
        WeaponManager.currentWeaponAnim = meleeWeapon.anim;

        meleeWeapon.transform.localPosition = Vector3.zero;
        meleeWeapon.gameObject.SetActive(true);
    }
}
