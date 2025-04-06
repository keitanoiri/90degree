using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimationController : MonoBehaviourPunCallbacks
{
    //���Animater��Flag�𐧌䂷��

    [SerializeField] SE_Player SE;
    [SerializeField] Transform spine;
    [SerializeField] Transform spine1;
    [SerializeField] Transform root;
    [SerializeField] Transform head;
    [SerializeField] Transform neck;
    [SerializeField] Transform character;
    [SerializeField] Transform firepos;
    [SerializeField] Transform arch2;
    [SerializeField] Transform righthand;
    [SerializeField] Transform lefthand;
    [SerializeField] Transform arrowRig;
    [SerializeField] GameObject arrowMesh;
    [SerializeField] Transform IKtarget;

    private Vector3 arch2defaultposicion;
    public Animator anim = null;
    private float deltamove;

    bool Recoiling = false;

    [SerializeField]�@PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        deltamove = 0;
        arch2defaultposicion = arch2.transform.localPosition;
    }

    

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine&&playerController.canMove)
        {
            if(Input.GetMouseButton(0)==true) SetMoveAnimationSpeed(0.5f);
            else SetMoveAnimationSpeed(1f);

            moveanimation();
        }

    }
    

    //IK�v�Z�̂��߂̃R�[���o�b�N
    private void OnAnimatorIK(int layerIndex)
    {

        if (anim.GetBool("aim") == true || anim.GetBool("drow") == true || Recoiling == true)//�|���\���Ă���Ƃ�
        {
            anim.SetLookAtWeight(1, 1, 1, 0, 0);
            anim.SetLookAtPosition(IKtarget.position);
        }
        else
        {
            anim.SetLookAtWeight(1,0.2f,0.8f,0,0);
            anim.SetLookAtPosition(IKtarget.position);
        }

        if (anim.GetBool("holdArrow") == true)
        {
            arrowMesh.SetActive(true);
            arrowRig.position = righthand.position + arrowRig.forward * 0.45f;
            arrowRig.rotation = righthand.rotation * Quaternion.Euler(-90, 0, 0);
        }
        else if (anim.GetBool("holdBow") == true)
        {
            arch2.transform.position = righthand.transform.position;
            Vector3 direction = firepos.position - righthand.position;
            arrowRig.rotation = Quaternion.LookRotation(direction);
            arrowRig.position = righthand.position + arrowRig.forward * 0.45f;
        }
        else
        {
            arrowMesh.SetActive(false);
            arch2.localPosition = arch2defaultposicion;
        }
    }

    //�ړ��A�j���[�V��������
    private void moveanimation()
    {
        float horizontalkey = Input.GetAxis("Horizontal");
        float verticalkey = Input.GetAxis("Vertical");
        //�x�[�X�̈ړ��ƃW�����v
        if(playerController.IsGround==false)
        {
            anim.SetBool("jump",true);
        }
        else
        {
            anim.SetBool("jump", false);
        }
        if (Mathf.Abs(verticalkey) - Mathf.Abs(horizontalkey) >= 0)//�O�̕��������傫���Ȃ�
        {
            if (verticalkey > 0)
            {
                anim.SetBool("runforward", true);
                anim.SetBool("runback", false);
                anim.SetBool("runleft", false);
                anim.SetBool("runright", false);
                anim.SetBool("idle", false);
            }
            else if (verticalkey < 0)
            {
                anim.SetBool("runforward", false);
                anim.SetBool("runback", true);
                anim.SetBool("runleft", false);
                anim.SetBool("runright", false);
                anim.SetBool("idle", false);
            }
            else if (deltamove == 0)//0�������Ȃ�
            {
                anim.SetBool("runforward", false);
                anim.SetBool("runback", false);
                anim.SetBool("runleft", false);
                anim.SetBool("runright", false);
                anim.SetBool("idle", true);
            }

        }
        else
        {
            if (horizontalkey > 0)
            {
                anim.SetBool("runforward", false);
                anim.SetBool("runback", false);
                anim.SetBool("runleft", false);
                anim.SetBool("runright", true);
                anim.SetBool("idle", false);
            }
            else if (horizontalkey < 0)
            {
                anim.SetBool("runforward", false);
                anim.SetBool("runback", false);
                anim.SetBool("runleft", true);
                anim.SetBool("runright", false);
                anim.SetBool("idle", false);
            }
        }
        deltamove = horizontalkey + verticalkey;
    }

    public void SetMoveAnimationSpeed(float speed)
    {
        anim.SetFloat("MoveSpeed",speed);
    }

    public void ResetAimAnimations()
    {
        anim.SetBool("aim", false);
        anim.SetBool("drow", false);
        anim.SetBool("holdBow", false);
        anim.SetBool("holdArrow", false);
    }

    public void PlayDrowAnimation(float firerate)
    {
        anim.SetFloat("DrowSpeed", firerate);
        anim.SetBool("drow", true);
        anim.SetBool("aim", false);//�X�V�^�C�~���O�̖��
    }

    public void PlayShotAnimation()
    {
        anim.SetBool("aim", false);
        anim.SetBool("drow", false);
    }

    public void FinishDrowAnimation()
    {
        if (photonView.IsMine == true)
        {
            anim.SetBool("aim", true);
            
            //�ˌ��\
            playerController.CanShoot = true;
        }
        Recoiling = true;
    }

    public void HoldBow()
    {
        if (photonView.IsMine)
        {
            anim.SetBool("holdBow", true);
            anim.SetBool("holdArrow", false);
            SE.SEdrow();
        }
    }

    public void HoldArrow()
    {
        if (photonView.IsMine)
        {
            anim.SetBool("holdArrow", true);
        }
    }

    public void FinishRecoil()
    {
        Recoiling = false;
    }

    public void WorkSEtiming()
    {
        SE.SEwork();
    }


}
