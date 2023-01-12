using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class WeaponManager : MonoBehaviour
{
    enum WeaponType
    {
        Gun,
        Hand,
        Axe
    }

    // ���� �ߺ� ��ü ���� ���� (semaphore)
    public static bool isChangeWeapon = false;

    //���� ��ü ������ (���� �ִ� �ð�, ������ �ð�)
    [SerializeField]
    private float changeWeaponOutDelayTime;
    [SerializeField]
    private float changeWeaponInDelayTime;

    //��� ���� ������
    [SerializeField]
    private Gun[] guns;
    [SerializeField]
    private MeleeWeapon[] meleeWeapons;

    // ���� ������ �ε��� ���ٿ� ��ųʸ�
    private Dictionary<string, int> gunIndexDict = new Dictionary<string, int>();
    private Dictionary<string, int> handIndexDict = new Dictionary<string, int>();
    private Dictionary<string, int> axeIndexDict = new Dictionary<string, int>();

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;
    [SerializeField]
    private AxeController axeController;

    // ���� ���� Ÿ��
    [SerializeField]
    private WeaponType currentWeaponType;
    // ���� ���� (�ִϸ��̼�) ����
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    void Awake()
    {
        gunController = GetComponent<GunController>();
        handController= GetComponent<HandController>();
        axeController = GetComponent<AxeController>();

        guns = GetComponentsInChildren<Gun>(true);
        meleeWeapons = GetComponentsInChildren<MeleeWeapon>(true);

        for (int i = 0; i < guns.Length; i++)
        {
            gunIndexDict.Add(guns[i].weaponName, i);
        }
        for (int i = 0; i < meleeWeapons.Length; i++)
        {
            switch (meleeWeapons[i].type)
            {
                case MeleeWeaponType.Hand:
                    handIndexDict.Add(meleeWeapons[i].weaponName, i);
                    break;
                case MeleeWeaponType.Axe:
                    axeIndexDict.Add(meleeWeapons[i].weaponName, i);
                    break;
            }
        }

        currentWeapon = gunController.gun.transform;
        currentWeaponAnim = gunController.gun.anim;
        GunController.isActivated = true;
    }

    void Update()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //���� ��ü (��)
                StartCoroutine(ChangeWeaponCoroutine(WeaponType.Gun, "SubMachineGun1"));
            }else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                //���� ��ü (��)
                StartCoroutine(ChangeWeaponCoroutine(WeaponType.Hand, "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                //���� ��ü (��)
                StartCoroutine(ChangeWeaponCoroutine(WeaponType.Axe, "Axe"));
            }
        }
    }

    private IEnumerator ChangeWeaponCoroutine(WeaponType _type, string name)
    {
        isChangeWeapon = true;
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponOutDelayTime);

        CancelAllWeaponAction();
        ChangeWeapon(_type, name);

        yield return new WaitForSeconds(changeWeaponInDelayTime);

        currentWeaponType = _type;
        isChangeWeapon = false;
    }
    private void CancelAllWeaponAction()
    {
        switch (currentWeaponType)
        {
            case WeaponType.Gun:
                gunController.CancelFineSight();
                gunController.CancelReload();
                break;
            case WeaponType.Hand:
            case WeaponType.Axe:
                break;
        }
    }

    private void ChangeWeapon(WeaponType _type, string name)
    {
        DisableAllController();
        switch (_type)
        {
            case WeaponType.Gun:
                gunController.ChangeGun(guns[gunIndexDict[name]]);
                GunController.isActivated = true;
                break;
            case WeaponType.Hand:
                handController.ChangeMeleeWeapon(meleeWeapons[handIndexDict[name]]);
                HandController.isActivated = true;
                break;
            case WeaponType.Axe:
                axeController.ChangeMeleeWeapon(meleeWeapons[axeIndexDict[name]]);
                AxeController.isActivated = true;
                break;
        }
    }

    private void DisableAllController()
    {
        GunController.isActivated = false;
        HandController.isActivated = false;
        AxeController.isActivated = false;
    }
}
