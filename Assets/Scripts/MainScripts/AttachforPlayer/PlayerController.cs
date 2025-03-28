using Cysharp.Threading.Tasks;
using Photon.Pun;
using System;
using UnityEngine;
using System.Collections.Generic;

public class PlayerController: MonoBehaviourPunCallbacks
{
    //注意カメラの方向制御はアニメーションコントローラーで行っている。
    Quaternion nowRotation;

    public float movePower = 300f; // 移動速度
    public float moveSpeed = 10f; // 移動速度
    public GameObject PlayerCamera;
    public GameObject Ballet;
    public int PlayerID { get; private set; }

    [SerializeField]public float gravity = -9.81f;//重力加速度
    [SerializeField]public float duration = 0.2f;//回転時間
    [SerializeField]public float ViewAngleY;
    public float elapsedTime = 0f;
    private Quaternion targetToRotation;
    public Vector3 gravityForce;

    public int bulletid = 0;

    public float JumpForce = 5f;

    private Rigidbody rb;

    public float MaxSpeed=20;
    public　bool IsGround=false;
    public bool CanShoot=false;

    //接地判定
    public Transform groundCheck; // 地面チェックのためのTransform
    public float groundCheckRadius = 0.2f; // 地面チェック用の球の半径
    public LayerMask groundLayer; // 地面を示すLayer

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
        //攻撃種類のリスト
        typeof(ArrowShot),typeof(TryShot),typeof(RandShot),typeof(Fastshot),typeof(Powershot)
    };

    private void Awake()
    {
        PlayerID = PhotonNetwork.LocalPlayer.ActorNumber;

        targetToRotation = transform.rotation;
        gravityForce = transform.up * gravity;


        rb = GetComponent<Rigidbody>();
        //rb.useGravity = false; // Rigidbody の gravity を無効にする
        //カメラの設定（Ismineを含む）
        ActivateCamera();

        //生成時にHPを設定
        PhotonNetwork.LocalPlayer.SetHP(MaxHP);

        //生成時にシールドを獲得
        ActivateShield();

        characterUI = GetComponentInChildren<CharacterUI>();
        MaxArrowNum = 15;
        NowArrowNum = MaxArrowNum;


        if (photonView.IsMine)
        {
            //Skillコンポーネントの追加
            int skillindex1 = PlayerPrefs.GetInt("Skill0", 0);
            Skill1 = (SkillBase)gameObject.AddComponent(SkillType[skillindex1]);
            int skillindex2 = PlayerPrefs.GetInt("Skill1", 0);
            Skill2 = (SkillBase)gameObject.AddComponent(SkillType[skillindex2]);
            int skillindex3 = PlayerPrefs.GetInt("Skill2", 0);
            Skill3 = (SkillBase)gameObject.AddComponent(SkillType[skillindex3]);

            //生成時に感度を設定
            mouseSensitivityX = PlayerPrefs.GetFloat("Vartical", 50);
            mouseSensitivityY = PlayerPrefs.GetFloat("Horizontal", 50);

            //UI初期化
            characterUI.firemodeUI(0);
            firemode = 0;
            characterUI.ChengeRemainArrowNumText(NowArrowNum, MaxArrowNum);
        }
    }

    override public void OnEnable()
    {
        //アクティブになるたびに感度を再設定
        mouseSensitivityX = PlayerPrefs.GetFloat("Vartical", 50);
        mouseSensitivityY = PlayerPrefs.GetFloat("Horizontal", 50);
    }


    void Update()
    {
        if (photonView.IsMine&&canMove)//自身の場合のみ実行&動ける時
        {
            bool oldIsGround = IsGround;  
            IsGround = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);//接地判定
            if(oldIsGround==false&&IsGround==true) {//着地時に運動をリセット
                rb.velocity = Vector3.zero;           // 直線速度をゼロに設定
                rb.angularVelocity = Vector3.zero;    // 回転速度をゼロに設定
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
        //移動用(WASD）
        // 入力の取得
        float moveHorizontal = Input.GetAxis("Horizontal"); // A, Dキー
        float moveVertical = Input.GetAxis("Vertical"); // W, Sキー
                                                        // 移動方向の計算
        Vector3 movement = Character.right * moveHorizontal + Character.forward * moveVertical;
        movement.Normalize();

        //坂道対応
        //前方にレイを飛ばす
        

        RaycastHit hit;

        if(IsGround==true && Physics.Raycast(transform.position + (movement * 0.1f)+(Character.transform.up*0.2f), movement*1f, out hit,0.5f))
        {
            //傾斜に沿った方向をmovementに設定
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
            // 力を相対的に加える
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
                    // ヒットした面の法線ベクトルを取得
                    Vector3 normal = hit.normal;
                    gravityForce = normal * gravity;
                    Quaternion fromToRotation = Quaternion.FromToRotation(Character.transform.up, normal);
                    elapsedTime = 0f;

                    targetToRotation = fromToRotation * Character.transform.rotation;

                    nowRotation = Character.transform.rotation;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))//90度回転
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
            //カメラと音声を有効化
            PlayerCamera.SetActive(true);
            //UIの有効化
            PlayerUICanvas.SetActive(true);
        }
        else
        {
            //自身のものでなければ無効化
            PlayerCamera.SetActive(false);

            PlayerUICanvas.SetActive(false);
        }
    }

    private void CameraIKControl()
    {
        // マウス入力の取得と体の向きの変更
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

    private void ParticleCheck()//updateで実行
    {
        // パーティクルシステムが再生終了しており、すべてのパーティクルが消滅した場合
        if (P_Shield.activeSelf==true&&!shieldParticle.IsAlive())
        {
            Debug.Log("パーティクルシステムの寿命が終了しました");
            P_Shield.SetActive(false);
        }
    }

    public async void HitEffect()
    {
        SE.SEhit();
        HitEffectUI.SetActive(true);

        var token = this.GetCancellationTokenOnDestroy(); // GameObject が破壊されたらキャンセル
        try
        {
            await UniTask.Delay(1000, cancellationToken: token);
            HitEffectUI.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("オブジェクトが破壊されたためタスクをキャンセルしました");
        }
    }

    public async void KillEffect()
    {
        SE.SEkill();
        KillEffectUI.SetActive(true);

        var token = this.GetCancellationTokenOnDestroy(); // GameObject が破壊されたらキャンセル
        try
        {
            await UniTask.Delay(1000, cancellationToken: token);
            KillEffectUI.SetActive(false);
        }
        catch (OperationCanceledException)
        {
            Debug.Log("オブジェクトが破壊されたためタスクをキャンセルしました");
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
