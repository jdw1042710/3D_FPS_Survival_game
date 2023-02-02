using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor.PackageManager;
using UnityEngine;

public class HandController : MeleeWeaponController
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
