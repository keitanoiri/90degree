using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController: MonoBehaviourPunCallbacks
{
    //���ӃJ�����̕�������̓A�j���[�V�����R���g���[���[�ōs���Ă���B
    Quaternion nowRotation;

    public float movePower = 300f; // �ړ����x
    public float moveSpeed = 10f; // �ړ����x
    public GameObject PlayerCamera;
    public GameObject Ballet;
    public int PlayerID { get; private set; }

    [SerializeField]public float gravity = -9.81f;//�d�͉����x
    [SerializeField]public float duration = 0.2f;//��]����
    [SerializeField]public float ViewAngleY;
    public float elapsedTime = 0f;
    private Quaternion targetToRotation;
    public Vector3 gravityForce;

    public int bulletid = 0;

    public float JumpForce = 5f;

    private Rigidbody rb;

    public float MaxSpeed=20;
    public�@bool IsGround=false;
    public bool CanShoot=false;

    //�ڒn����
    public Transform groundCheck; // �n�ʃ`�F�b�N�̂��߂�Transform
    public float groundCheckRadius = 0.2f; // �n�ʃ`�F�b�N�p�̋��̔��a
    public LayerMask groundLayer; // �n�ʂ�����Layer

    [SerializeField] public int MaxHP;
    [SerializeField] public Transform firepos;

    [SerializeField] private GameObject Killcam;
    [SerializeField] public SE_Player SE;
    [SerializeField] private Transform Character;
    [SerializeField] Transform IKRoot;
    [SerializeField] Transform IKtarget;
    [SerializeField] Transform HeadPos;
    [SerializeField] GameObject PlayerUICanvas;
    [SerializeField] GameObject HitEffectUI;
    [SerializeField] GameObject KillEffectUI;

    private float mouseSensitivityX;
    private float mouseSensitivityY;


    [SerializeField] public GameObject P_Shield;
    [SerializeField] ParticleSystem shieldParticle;

    public CharacterUI characterUI;
    private int firemode = 1;
    private int castedfiremode;

    public int MaxArrowNum = 15;
    public int NowArrowNum = 15;

    public bool canMove = true;

    [SerializeField] public int BaseDamage = 50;
    [SerializeField] private SkillBase Skill1;
    [SerializeField] private SkillBase Skill2;
    [SerializeField] private SkillBase Skill3;
    
    private List<Type> SkillType = new List<Type>
    {
        //�U����ނ̃��X�g
        typeof(ArrowShot),typeof(TryShot),typeof(RandShot),typeof(Fastshot),typeof(Powershot)
    };

    private void Awake()
    {
        PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;

        targetToRotation = transform.rotation;
        gravityForce = transform.up * gravity;


        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false; // Rigidbody �� gravity �𖳌��ɂ���
        //�J�����̐ݒ�iIsmine���܂ށj
        ActivateCamera();

        //��������HP��ݒ�
        PhotonNetwork.LocalPlayer.SetHP(MaxHP);

        //�������ɃV�[���h���l��
        ActivateShield();

        characterUI = GetComponentInChildren<CharacterUI>();
        MaxArrowNum = 15;
        NowArrowNum = MaxArrowNum;


        if (photonView.IsMine)
        {
            //Skill�R���|�[�l���g�̒ǉ�
            int skillindex1 = PlayerPrefs.GetInt("Skill0", 0);
            Skill1 = (SkillBase)gameObject.AddComponent(SkillType[skillindex1]);
            int skillindex2 = PlayerPrefs.GetInt("Skill1", 0);
            Skill2 = (SkillBase)gameObject.AddComponent(SkillType[skillindex2]);
            int skillindex3 = PlayerPrefs.GetInt("Skill2", 0);
            Skill3 = (SkillBase)gameObject.AddComponent(SkillType[skillindex3]);

            //�������Ɋ��x��ݒ�
            mouseSensitivityX = PlayerPrefs.GetFloat("Vartical", 50);
            mouseSensitivityY = PlayerPrefs.GetFloat("Horizontal", 50);

            //UI������
            characterUI.firemodeUI(0);
            firemode = 0;
            characterUI.ChengeRemainArrowNumText(NowArrowNum, MaxArrowNum);
        }
    }

    override public void OnEnable()
    {
        //�A�N�e�B�u�ɂȂ邽�тɊ��x���Đݒ�
        mouseSensitivityX = PlayerPrefs.GetFloat("Vartical", 50);
        mouseSensitivityY = PlayerPrefs.GetFloat("Horizontal", 50);
    }


    void Update()
    {
        if (photonView.IsMine&&canMove)//���g�̏ꍇ�̂ݎ��s&�����鎞
        {
            bool oldIsGround = IsGround;  
            IsGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);//�ڒn����
            if(oldIsGround==false&&IsGround==true) {//���n���ɉ^�������Z�b�g
                rb.velocity = Vector3.zero;           // �������x���[���ɐݒ�
                rb.angularVelocity = Vector3.zero;    // ��]���x���[���ɐݒ�
            }

            WeaponSistems();
            ChengeGravity();
            Jamp();
            CameraIKControl();

            ParticleCheck();

        }
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Gravity();
            if (canMove)Move();
        }
    }
    private void Jamp()
    {
        if(IsGround==true&&Input.GetKeyDown(KeyCode.Space)==true)
        {
            Vector3 JumpAngleForce = Character.up*JumpForce;
            rb.AddForce(JumpAngleForce, ForceMode.Impulse);
        }
    }

    private void Move()
    {
        //�ړ��p(WASD�j
        // ���͂̎擾
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, D�L�[
        float moveVertical = Input.GetAxis("Vertical"); // W, S�L�[
                                                        // �ړ������̌v�Z
        Vector3 movement = Character.right * moveHorizontal + Character.forward * moveVertical;
        movement.Normalize();

        //�⓹�Ή�
        //�O���Ƀ��C���΂�
        

        RaycastHit hit;

        if(IsGround==true && Physics.Raycast(transform.position + (movement * 0.1f)+(Character.transform.up*0.2f), movement*1f, out hit,0.5f))
        {
            //�X�΂ɉ�����������movement�ɐݒ�
            Vector3 hitNormal = hit.normal;
            movement = Vector3.ProjectOnPlane(movement, hitNormal);
        }


        
        if (IsGround == true&&!Input.GetMouseButton(0))
        {
            Vector3 projectedVelocity = Vector3.Project(rb.velocity, Character.transform.up);
            rb.velocity = movement * moveSpeed+projectedVelocity;
        }
        else if (IsGround==false)
        {
            Debug.Log("IsGroundfalse");
            // �͂𑊑ΓI�ɉ�����
            rb.AddForce(movement * movePower);
        }
        else{
            Vector3 projectedVelocity = Vector3.Project(rb.velocity, Character.transform.up);
            rb.velocity = movement * moveSpeed/2 + projectedVelocity;
        }
    }

    private void ChengeGravity()
    {
        elapsedTime += Time.deltaTime;

        if (Input.GetMouseButtonDown(1))
        {
            SE.SEgravity();

            if (Input.GetKey(KeyCode.LeftShift) )
            {
                gravityForce = PlayerCamera.transform.forward * gravity*-1;
                elapsedTime = 0f;
                Quaternion fromToRotation = Quaternion.FromToRotation(Character.transform.up, PlayerCamera.transform.forward*-1);
                targetToRotation = fromToRotation * Character.transform.rotation;
                nowRotation = Character.transform.rotation;
            }
            else
            {
                Camera cam = PlayerCamera.GetComponent<Camera>();
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // �q�b�g�����ʂ̖@���x�N�g�����擾
                    Vector3 normal = hit.normal;
                    gravityForce = normal * gravity;
                    Quaternion fromToRotation = Quaternion.FromToRotation(Character.transform.up, normal);
                    elapsedTime = 0f;

                    targetToRotation = fromToRotation * Character.transform.rotation;

                    nowRotation = Character.transform.rotation;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))//90�x��]
        {
            SE.SEgravity();
            Quaternion fromToRotation = Quaternion.AngleAxis(-90, Character.transform.forward);
            elapsedTime = 0f;
            targetToRotation = fromToRotation * Character.transform.rotation;

            gravityForce = targetToRotation * Vector3.up * gravity;

            nowRotation = Character.transform.rotation;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SE.SEgravity();
            Quaternion fromToRotation = Quaternion.AngleAxis(90, Character.transform.forward);
            elapsedTime = 0f;
            targetToRotation = fromToRotation * Character.transform.rotation;

            gravityForce = targetToRotation * Vector3.up * gravity;

            nowRotation = Character.transform.rotation;
        }
        if (elapsedTime < duration)
        {
            Character.transform.rotation = Quaternion.Slerp(nowRotation,targetToRotation, elapsedTime / duration);
        }
    }
    private void Gravity()
    {
        rb.AddForce(gravityForce, ForceMode.Acceleration);
    }
    private void ActivateCamera()
    {
        if (photonView.IsMine)
        {
            //�J�����Ɖ�����L����
            PlayerCamera.SetActive(true);
            //UI�̗L����
            PlayerUICanvas.SetActive(true);
        }
        else
        {
            //���g�̂��̂łȂ���Ζ�����
            PlayerCamera.SetActive(false);

            PlayerUICanvas.SetActive(false);
        }
    }

    private void CameraIKControl()
    {
        // �}�E�X���͂̎擾�Ƒ̂̌����̕ύX
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime*2;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime*2;

        IKRoot.Rotate(Character.right, -mouseY, Space.World);
        IKRoot.Rotate(Character.up, mouseX, Space.World);
        //IKRoot.position = PlayerCamera.transform.position;

        Quaternion Q = Quaternion.Inverse(Character.rotation) * IKRoot.rotation;

        if (Q.eulerAngles.x < 180 && Q.eulerAngles.x > ViewAngleY)
        {

            IKRoot.Rotate(Character.right, -(Q.eulerAngles.x - ViewAngleY), Space.World);
        }
        else if (Q.eulerAngles.x >= 180 && Q.eulerAngles.x < 360 - ViewAngleY)
        {
            IKRoot.Rotate(Character.right, -(Q.eulerAngles.x - (360 - ViewAngleY)), Space.World);
        }

        Character.Rotate(0, Q.eulerAngles.y, 0);

        PlayerCamera.transform.LookAt(IKtarget, Character.transform.up);

        firepos.rotation = PlayerCamera.transform.rotation;
    }

    public void ActivateShield()
    {
        //5byoukann
        if (P_Shield != null)
        {
            P_Shield.SetActive(true);
        }
    }

    public void GetSupplyItem()
    {
        SE.SEItem();
        NowArrowNum = MaxArrowNum;
        //characterUI = GetComponentInChildren<CharacterUI>();
        characterUI.ChengeRemainArrowNumText(NowArrowNum,   MaxArrowNum);
    }

    private void ParticleCheck()//update�Ŏ��s
    {
        // �p�[�e�B�N���V�X�e�����Đ��I�����Ă���A���ׂẴp�[�e�B�N�������ł����ꍇ
        if (P_Shield.activeSelf==true&&!shieldParticle.IsAlive())
        {
            Debug.Log("�p�[�e�B�N���V�X�e���̎������I�����܂���");
            P_Shield.SetActive(false);
        }
    }

    public async void HitEffect()
    {
        SE.SEhit();
        HitEffectUI.SetActive(true);

        var token = this.GetCancellationTokenOnDestroy(); // GameObject ���j�󂳂ꂽ��L�����Z��
        try
        {
            await UniTask.Delay(1000, cancellationToken: token);
            HitEffectUI.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("�I�u�W�F�N�g���j�󂳂ꂽ���߃^�X�N���L�����Z�����܂���");
        }
    }

    public async void KillEffect()
    {
        SE.SEkill();
        KillEffectUI.SetActive(true);

        var token = this.GetCancellationTokenOnDestroy(); // GameObject ���j�󂳂ꂽ��L�����Z��
        try
        {
            await UniTask.Delay(1000, cancellationToken: token);
            KillEffectUI.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("�I�u�W�F�N�g���j�󂳂ꂽ���߃^�X�N���L�����Z�����܂���");
        }
    }


    private void WeaponSistems()
    {
        //Serectfire
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            firemode = 0;
            characterUI.firemodeUI(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            firemode = 1;
            characterUI.firemodeUI(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            firemode = 2;
            characterUI.firemodeUI(2);
        }

        if (Input.GetMouseButtonDown(0))
        {
            switch (firemode)
            {
                case 0:
                    Skill1.Drow();
                    castedfiremode = 0;
                    break;
                case 1:
                    Skill2.Drow();
                    castedfiremode = 1;
                    break;
                case 2:
                    Skill3.Drow();
                    castedfiremode = 2;
                    break;
            }
        }
        else if (Input.GetMouseButtonUp(0)) {
            switch (castedfiremode)
            {
                case 0:
                    Skill1.Shoot();
                    break;
                case 1:
                    Skill2.Shoot();
                    break;
                case 2:
                    Skill3.Shoot();
                    break;
            }
        }
    }

    [PunRPC]
    public void Fire(int id, Vector3 gravity, Vector3 pos, Quaternion rotate,float Damage)
    {
        SE.SEshot();
        GameObject cleatedBallet = Instantiate(Ballet);
        Ballet ballet = cleatedBallet.GetComponent<Ballet>();
        if (ballet != null) { ballet.Init(id, photonView.OwnerActorNr, gravity, pos, rotate, Damage); }
    }

    [PunRPC]
    public void Fire(int id, Vector3 gravity, Vector3 pos, Quaternion rotate, float Damage,float SPEED)
    {
        SE.SEshot();
        GameObject cleatedBallet = Instantiate(Ballet);
        Ballet ballet = cleatedBallet.GetComponent<Ballet>();
        if (ballet != null) { ballet.Init(id, photonView.OwnerActorNr, gravity, pos, rotate, Damage,SPEED); }
    }
}
