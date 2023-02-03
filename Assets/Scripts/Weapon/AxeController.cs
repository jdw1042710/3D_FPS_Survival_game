using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MeleeWeaponController
{
    public static bool isActivated = false;
    void Update()
    {
        if (isActivated)
        {
            TryAttack();
        }
    }

    protected override void HitAction() 
    {
        Rock _rock = hitInfo.transform.GetComponent<Rock>();
        if (_rock)
        {
            _rock.Minning();
        }
        else if (hitInfo.transform.CompareTag("NPC"))
        {
            // 이후 상속을 통해 해결
            hitInfo.transform.GetComponent<Animal>().Damage(meleeWeapon.damage, transform.position);
            SoundManager.instance.PlaySE("NPC_Hit");
        }
        else
        {
            base.HitAction();
        }
    }
}
