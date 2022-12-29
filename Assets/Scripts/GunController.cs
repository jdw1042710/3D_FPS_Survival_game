using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class GunController : MonoBehaviour
{
    //������ ��
    [SerializeField]
    private Gun gun;

    //���� �ӵ� ����
    private float currentFireRate;

    //���� ����
    private bool isReload = false;
    private bool isFineSightMode = false;

    //������ �� �ݵ� ���� ��ġ ����
    [SerializeField]
    private Vector3 originPos;
    private Vector3 recoilBack;
    private Vector3 retroActionRecoilBack;

    //������Ʈ
    private AudioSource audioSource;
    [SerializeField]
    private Camera theCamera;

    //������
    [SerializeField]
    private GameObject hitEffectPrefab;

    //�浹 ����
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
    //�߻� ����
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
        //�߻� ��
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
        //�߻�
    private void Shoot()
    {
        gun.currentBulletNum--;
        currentFireRate = gun.fireRate;
        PlaySE(gun.fireSound);
        gun.muzzleFlash.Play();
        //�ѱ�ݵ�
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
    // ������ ����
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
    // ������ ����
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

    // �ݵ� ����
    IEnumerator RetroActionCoroutine()
    {
        Vector3 _recoilBack = isFineSightMode ? retroActionRecoilBack: recoilBack;
        Vector3 _originPos = isFineSightMode ? gun.fineSightOriginPos : originPos;
        float force = isFineSightMode ? gun.retroActionFineSightForce : gun.retroActionForce;
        int count;
        gun.transform.localPosition = _originPos;
        //�ݵ� ����
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

        //����ġ
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

    //����ȿ��
    private void PlaySE(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

}
