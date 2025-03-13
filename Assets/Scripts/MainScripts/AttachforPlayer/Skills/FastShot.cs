using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Fastshot : SkillBase
// Start is called before the first frame update
{
    float speed = 0.5f;

    private void Awake()
    {
        skillName="FastShot";
        skillExprain="�f���������Ė����B\n�З͂Ƒ����͍T����";

        DrowCost=1;
        DrowRate=2.0f;
        DamageReflectionRate = 0.7f;
    }
    public override void Shoot()
    {
        //��{�I�Ƀ}�E�X�������ꂽ�Ƃ��ɍs��
        if(pc.CanShoot == true)
        {
            pc.bulletid++;//����ID��ǉ�
            pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, pc.firepos.rotation,pc.BaseDamage*DamageReflectionRate,speed);
            pc.CanShoot = false;

            anicon.PlayShotAnimation();
        }
        anicon.ResetAimAnimations();
        
    }

    public override void Drow()
    {
        //��{�I�Ƀ}�E�X�������ꂽ�Ƃ��ɍs��
        //�������
        if (pc.NowArrowNum < DrowCost)
        {
            //����Ȃ����Œm�点��H
            return;
        }
        pc.NowArrowNum-=DrowCost;
        pc.characterUI.ChengeRemainArrowNumText(pc.NowArrowNum, pc.MaxArrowNum);

        //������A�j���[�V����
        anicon.PlayDrowAnimation(DrowRate);
    }




}
