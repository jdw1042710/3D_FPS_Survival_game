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
    private Hand[] hands;

    // ���� ������ �ε��� ���ٿ� ��ųʸ�
    private Dictionary<string, int> gunIndexDict = new Dictionary<string, int>();
    private Dictionary<string, int> handIndexDict = new Dictionary<string, int>();

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController gunController;
    [SerializeField]
    private HandController handController;

    // ���� ���� Ÿ��
    [SerializeField]
    private WeaponType currentWeaponType;
    // ���� ���� (�ִϸ��̼�) ����
    public static Transform currentWeapon;
    public static Animator currentWeaponAnim;


    void Start()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            gunIndexDict.Add(guns[i].weaponName, i);
        }
        for (int i = 0; i < hands.Length; i++)
        {
            handIndexDict.Add(hands[i].weaponName, i);
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
                handController.ChangeHand(hands[handIndexDict[name]]);
                HandController.isActivated = true;
                break;
        }
    }

    private void DisableAllController()
    {
        GunController.isActivated = false;
        HandController.isActivated = false;
    }
}
