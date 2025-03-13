using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandShot : SkillBase
{
    private void Awake()
    {
        skillName = "RandShot";
        skillExprain = "�����_���ɍL���Ɋg�U����5�{�̖�����";

        DrowCost = 5;
        DrowRate = 1.0f;
        DamageReflectionRate = 1.0f;
    }
    public override void Shoot()
    {
        //��{�I�Ƀ}�E�X�������ꂽ�Ƃ��ɍs��
        if (pc.CanShoot == true)
        {
            for (int i = 0; i < 5; i++)
            {
                pc.bulletid++;//����ID��ǉ�

                float randomOffsetX = UnityEngine.Random.Range(-15f, 15f);
                float randomOffsetY = UnityEngine.Random.Range(-15f, 15f);
                float randomOffsetZ = UnityEngine.Random.Range(-15f, 15f);
                Quaternion randomRotation = pc.firepos.rotation * Quaternion.Euler(randomOffsetX, randomOffsetY, randomOffsetZ);
                pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, randomRotation, pc.BaseDamage * DamageReflectionRate);
            }

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
