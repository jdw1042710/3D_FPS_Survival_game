using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    // 크로스 헤어 비활성화를 위한 부모객체
    [SerializeField] 
    private GameObject crossHairHUD;
    [SerializeField]
    private GunController gunController;

    public void WalkAnimation(bool _flag)
    {
        anim.SetBool("Walk", _flag);
    }
    public void RunAnimation(bool _flag)
    {
        anim.SetBool("Run", _flag);
    }
    public void CrouchAnimation(bool _flag)
    {
        anim.SetBool("Crouch", _flag);
    }
    public void FineSightAnimation(bool _flag)
    {
        anim.SetBool("FineSight", _flag);
    }

    public void FireAnimation()
    {
        anim.SetTrigger("Fire");
    }
}
