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
        else
        {
            base.HitAction();
        }
    }
}
