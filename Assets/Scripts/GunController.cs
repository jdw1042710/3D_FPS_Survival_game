using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //장착된 총
    [SerializeField]
    private Gun gun;

    //연사 속도 관련
    private float currentFireRate;

    //상태 변수
    private bool isReload = false;
    private bool isFineSightMode = false;

    //정조준 및 반동 관련 위치 변수
    [SerializeField]
    private Vector3 originPos;
    private Vector3 recoilBack;
    private Vector3 retroActionRecoilBack;

    //컴포넌트
    private AudioSource audioSource;
    [SerializeField]
    private Camera theCamera;

    //프리팹
    [SerializeField]
    private GameObject hitEffectPrefab;

    //충돌 정보
    private RaycastHit hitInfo;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originPos = transform.localPosition;
        recoilBack = new Vector3(originPos.x + gun.retroActionForce , originPos.y, originPos.z);
        retroActionRecoilBack = new Vector3(gun.fineSightOriginPos.x + gun.retroActionFineSightForce , gun.fineSightOriginPos.y, gun.fineSightOriginPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        FireRateCalc();
        TryFire();
        TryReload();
        TryFineSight();
    }
    //발사 관련
    private void FireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void TryFire()
    {
        if (Input.GetButton("Fire1") && currentFireRate <= 0 && !isReload)
        {
            Fire();
        }
    }
        //발사 전
    private void Fire()
    {
        if (gun.currentBulletNum > 0)
        {
            Shoot();
        }
        else
        {
            StartCoroutine(ReloadCouroutine());
        }
    }
        //발사
    private void Shoot()
    {
        gun.currentBulletNum--;
        currentFireRate = gun.fireRate;
        PlaySE(gun.fireSound);
        gun.muzzleFlash.Play();
        //총기반동
        StopCoroutine(RetroActionCoroutine());
        StartCoroutine(RetroActionCoroutine());
        Hit();
    }

    private void Hit()
    {
        if(Physics.Raycast(theCamera.transform.position, theCamera.transform.forward, out hitInfo, gun.range))
        {
            var clone = Instantiate(hitEffectPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            Destroy(clone, 2f);
        }
    }
    // 재장전 관련
    private void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReload && gun.currentBulletNum != gun.maxBulletNum) 
        {
            StartCoroutine(ReloadCouroutine());
        }
    }
    
    private IEnumerator ReloadCouroutine()
    {
        if(gun.carryBulletNum> 0)
        {
            CancelFineSight();

            isReload = true;
            gun.anim.SetTrigger("Reload");

            gun.carryBulletNum += gun.currentBulletNum;
            gun.currentBulletNum = 0;

            yield return new WaitForSeconds(gun.reloadTime);


            int _reload = Mathf.Clamp(gun.carryBulletNum, gun.carryBulletNum, gun.maxBulletNum);
            gun.carryBulletNum -= _reload;
            gun.currentBulletNum = _reload;
            isReload = false;
        }
        else
        {
            Debug.Log("Bullet is Empty");
        }
    }
    // 정조준 관련
    private void TryFineSight()
    {
        if (Input.GetButtonDown("Fire2") && !isReload)
        {
            FineSight();
        }
    }

    private void FineSight() 
    {
        isFineSightMode = !isFineSightMode;
        gun.anim.SetBool("FineSightMode", isFineSightMode);
        StopCoroutine(SightChangeCoroutine());
        StartCoroutine(SightChangeCoroutine());
    }

    IEnumerator SightChangeCoroutine()
    {
        Vector3 destPos = isFineSightMode ? gun.fineSightOriginPos : originPos;
        int count = 0;
        while(gun.transform.localPosition != destPos)
        {
            count++;
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, destPos, 0.2f);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        gun.transform.localPosition = destPos;
    }
    public void CancelFineSight()
    {
        if (isFineSightMode)
        {
            FineSight();
        }
    }

    // 반동 관련
    IEnumerator RetroActionCoroutine()
    {
        Vector3 _recoilBack = isFineSightMode ? retroActionRecoilBack: recoilBack;
        Vector3 _originPos = isFineSightMode ? gun.fineSightOriginPos : originPos;
        float force = isFineSightMode ? gun.retroActionFineSightForce : gun.retroActionForce;
        int count;
        gun.transform.localPosition = _originPos;
        //반동 시작
        count = 0;
        while(gun.transform.localPosition != _recoilBack)
        {
            count++;
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, _recoilBack, 0.4f);
            if (count > 15)
            {
                break;
            }
            yield return null;
        }
        gun.transform.localPosition = _recoilBack;

        //원위치
        count = 0;
        while (gun.transform.localPosition != _originPos)
        {
            count++;
            gun.transform.localPosition = Vector3.Lerp(gun.transform.localPosition, _originPos, 0.1f);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        gun.transform.localPosition = _originPos;
    }

    //사운드효과
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
