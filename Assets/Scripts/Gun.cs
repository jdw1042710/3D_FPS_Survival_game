using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public string typeName; // 이름
    public float range; // 사정거리
    public float accuracy; // 정확도
    public float fireRate; // 연사속도
    public float reloadTime; //재장전 속도

    public int damage; // 총의 데미지
    public int maxBulletNum; // 최대 장탄수
    public int currentBulletNum; // 현재 장탄수

    public int maxCarryBulletNum; // 소유 가능한 최대 총알 수
    public int carryBulletNum; // 현재 소유한 총알 수

    public float retroActionForce; // 반동 세기
    public float retroActionFineSightForce; // 정조준시 반동 세기

    public Vector3 fineSightOriginPos; // 정조준시 위치
    public Animator anim;
    public ParticleSystem muzzleFlash; // 총구 섬광 효과
    public AudioClip fireSound; // 총성 효과
}
