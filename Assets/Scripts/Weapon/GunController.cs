using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GunController : MonoBehaviour
{
    public static bool isActivated = false;
    //장착된 총
    [field : SerializeField]
    public Gun gun { get; private set; }

    //연사 속도 관련
    private float currentFireRate;

    // 자세에 따른 총의 정확도
    private float poseAccuracy;

    //상태 변수
    private bool isReload = false;
    public bool isFineSightMode = false;

    //정조준 및 반동 관련 위치 변수
    [SerializeField]
    private Vector3 originPos;
    private Vector3 recoilBack;
    private Vector3 retroActionRecoilBack;

    //컴포넌트
    private AudioSource audioSource;
    [SerializeField]
    private Camera camera;
    private PlayerController playerController;
    private CrossHair crossHair;

    //프리팹
    [SerializeField]
    private GameObject hitEffectPrefab;

    //충돌 정보
    private RaycastHit hitInfo;
    [SerializeField] private LayerMask layerMask;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        crossHair = FindObjectOfType<CrossHair>();
        originPos = transform.localPosition;
        recoilBack = new Vector3(originPos.x + gun.retroActionForce , originPos.y, originPos.z);
        retroActionRecoilBack = new Vector3(gun.fineSightOriginPos.x + gun.retroActionFineSightForce , gun.fineSightOriginPos.y, gun.fineSightOriginPos.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActivated)
        {
            FireRateCalc();
            PoseAccuracyCalc();
            TryFire();
            TryReload();
            TryFineSight();
            ApplyAnimation();
        }
    }
    //발사 관련
    private void FireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }
    private void PoseAccuracyCalc()
    {

        if(playerController.isRun || !playerController.isGround)
        {
            poseAccuracy = 30f;
        }
        else if (playerController.isWalk)
        {
            poseAccuracy = 0.08f;
        }
        else if (playerController.isCrouch)
        {
            poseAccuracy = 0.02f;
        }
        else if (isFineSightMode)
        {
            poseAccuracy = 0.0001f;
        }
        else // Idle상태
        {
            poseAccuracy = 0.04f;
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
        crossHair.FireAnimation();
        gun.muzzleFlash.Play();
        //총기반동
        StopCoroutine(RetroActionCoroutine());
        StartCoroutine(RetroActionCoroutine());
        Hit();
    }

    private void Hit()
    {
        Vector3 trajectory = camera.transform.forward + new Vector3(Random.Range(-poseAccuracy - gun.accuracy, poseAccuracy + gun.accuracy),
                                                                    Random.Range(-poseAccuracy - gun.accuracy, poseAccuracy + gun.accuracy),
                                                                    0);
        if (Physics.Raycast(camera.transform.position, trajectory, out hitInfo, gun.range, layerMask))
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

    public void CancelReload()
    {
        if (isReload)
        {
            StopCoroutine(ReloadCouroutine());
            isReload = false;
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

    private void ApplyAnimation()
    {
        crossHair.FineSightAnimation(isFineSightMode);
    }

    // 총 교체
    public void ChangeGun(Gun _gun)
    {
        if (WeaponManager.currentWeapon != null)
        {
            WeaponManager.currentWeapon.gameObject.SetActive(false);
        }
        gun = _gun;
        WeaponManager.currentWeapon = gun.transform;
        WeaponManager.currentWeaponAnim = gun.anim;

        gun.transform.localPosition = Vector3.zero;
        gun.gameObject.SetActive(true);
    }
}
