using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string typeName; // �̸�
    public float range; // �����Ÿ�
    public float accuracy; // ��Ȯ��
    public float fireRate; // ����ӵ�
    public float reloadTime; //������ �ӵ�

    public int damage; // ���� ������
    public int maxBulletNum; // �ִ� ��ź��
    public int currentBulletNum; // ���� ��ź��

    public int maxCarryBulletNum; // ���� ������ �ִ� �Ѿ� ��
    public int carryBulletNum; // ���� ������ �Ѿ� ��

    public float retroActionForce; // �ݵ� ����
    public float retroActionFineSightForce; // �����ؽ� �ݵ� ����

    public Vector3 fineSightOriginPos; // �����ؽ� ��ġ
    public Animator anim;
    public ParticleSystem muzzleFlash; // �ѱ� ���� ȿ��
    public AudioClip fireSound; // �Ѽ� ȿ��
}
