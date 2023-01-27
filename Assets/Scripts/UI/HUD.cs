using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    // Weapon Manager���� HUD Ȱ��ȭ/��Ȱ��ȭ
    [SerializeField]
    private GameObject bulletUI;

    // ������Ʈ
    [SerializeField]
    private GunController gunController;
    private Gun gun;

    [SerializeField]
    private TextMeshProUGUI maxBulletText;
    [SerializeField]
    private TextMeshProUGUI currentBulletText;
    [SerializeField]
    private TextMeshProUGUI carryBulletText;

    private void Update()
    {
        CheckBullet();
    }

    private void CheckBullet()
    {
        gun = gunController.gun;
        maxBulletText.text = gun.maxBulletNum.ToString();
        currentBulletText.text = gun.currentBulletNum.ToString();
        carryBulletText.text = gun.carryBulletNum.ToString();
    }
}
