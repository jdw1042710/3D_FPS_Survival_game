using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MeleeWeaponType
{
    Hand,
    Axe,
    Pickaxe,
}

public class MeleeWeapon : MonoBehaviour
{

    public string weaponName; // 맨손, 너클...
    public MeleeWeaponType type;
    public float range; // 공격 범위
    public int damage; // 공격력
    public float workSpeed; // 작업 속도
    public float attackDelay; // 공격 딜레이
    public float attackDelayA; // 공격 시작 ~ 공격 판정 활성화
    public float attackDelayB; // 공격 판정 비활성화 ~ 공격 종료

    public Animator anim;
}
