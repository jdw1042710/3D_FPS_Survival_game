using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponController : MonoBehaviour
{
    // ���� ������ Hand�� ���
    [SerializeField]
    protected MeleeWeapon meleeWeapon;

    //���� ����
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
        // ���� ���� Ȱ��ȭ
        isSwing= true;

        //���� ����
        StartCoroutine(HitCoroutine());

        yield return new WaitForSeconds(meleeWeapon.attackDelayB);
        // ���� ���� ��Ȱ��ȭ
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

    // ���� ��ü
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
