using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    // Weapon Manager에서 HUD 활성화/비활성화
    [SerializeField]
    private GameObject bulletUI;

    // 컴포넌트
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
