using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static bool isActivated = true;
    // �̵��ӵ� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;
    private float applySpeed;

    // 
    [SerializeField]
    private float jumpForce;

    // ���� ����
    public bool isWalk { get; private set; } = false;
    public bool isRun { get; private set; } = false;
    public bool isCrouch { get; private set; } = false;
    public bool isGround { get; private set; } = true;
    private Vector3 exPos; // ���� ������ ��ġ

    // Crouch ���� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    // ī�޶� �ΰ���
    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = .0f;

    // ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;
    private StatusController statusController;
    [SerializeField]
    private CrossHair crossHair;


    // Start is called before the first frame update
    void Awake()
    {
        myRigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        statusController = GetComponent<StatusController>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActivated) return;
        CheckGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();
        CharacterRotation();
        ApplyAnimation();
    }

    private void FixedUpdate()
    {
        MoveCheck();
    }
    // ���̱�
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0;
        while(_posY != applyCrouchPosY)
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.1f);
            theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, _posY, theCamera.transform.localPosition.z);
            if(count > 15)
            {
                break;
            }
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z);
    }

    private void CancelCrouch()
    {
        if (isCrouch)
        {
            Crouch();
        }
    }
    // ����
    private void CheckGround() 
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump() 
    {
        //������ ��ũ���� ����
        CancelCrouch();
        myRigid.velocity = transform.up * jumpForce;
    }
    // �޸���
    private void TryRun() 
    {
        if (Input.GetKey(KeyCode.LeftShift) && statusController.TryDecreaseStamina(10))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        WeaponManager.instance.CancelAllWeaponAction();
        CancelCrouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }
    //�̵�
    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");
        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;
        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);

    }

    private void MoveCheck() {
        isWalk = !isRun && !isCrouch && isGround && (Vector3.Distance(exPos ,transform.position) > 0.01f);
        exPos = transform.position;
    }
    // ȸ��
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }
    private void CharacterRotation() {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f, _yRotation, 0f) * lookSensitivity;
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY));

    }

    //�ִϸ��̼� ����
    private void ApplyAnimation()
    {
        crossHair.WalkAnimation(isWalk);
        WeaponManager.currentWeaponAnim.SetBool("Walk", isWalk);
        crossHair.RunAnimation(isRun || !isGround);
        WeaponManager.currentWeaponAnim.SetBool("Run", isRun);
        crossHair.CrouchAnimation(isCrouch);
        WeaponManager.currentWeaponAnim.SetBool("Crouch", isCrouch);
    }
}
