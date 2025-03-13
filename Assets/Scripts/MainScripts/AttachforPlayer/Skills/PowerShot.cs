using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Powershot : SkillBase
// Start is called before the first frame update
{
    float speed = 2f;

    private void Awake()
    {
        skillName="PowerShot";
        skillExprain="������������i����B\n�З͂����т��\�����Ȃ�";

        DrowCost=10;
        DrowRate=0.5f;
        DamageReflectionRate = 3f;
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
