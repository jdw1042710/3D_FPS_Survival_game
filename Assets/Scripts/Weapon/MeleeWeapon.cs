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

    public string weaponName; // �Ǽ�, ��Ŭ...
    public MeleeWeaponType type;
    public float range; // ���� ����
    public int damage; // ���ݷ�
    public float workSpeed; // �۾� �ӵ�
    public float attackDelay; // ���� ������
    public float attackDelayA; // ���� ���� ~ ���� ���� Ȱ��ȭ
    public float attackDelayB; // ���� ���� ��Ȱ��ȭ ~ ���� ����

    public Animator anim;
}
