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
}
