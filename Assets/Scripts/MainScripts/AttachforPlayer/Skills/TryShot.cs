using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TryShot : SkillBase
{
    private void Awake()
    {
        skillName = "TryShot";
        skillExprain = "�������Ɋg�U����O�{�̖�����";

        DrowCost = 3;
        DrowRate = 1.0f;
        DamageReflectionRate = 1.0f;
    }
    public override void Shoot()
    {
        //��{�I�Ƀ}�E�X�������ꂽ�Ƃ��ɍs��
        if (pc.CanShoot == true)
        {
            for (int i = 0; i < 3; i++)
            {
                pc.bulletid++;//����ID��ǉ�
                //Quaternion rotationDelta = Quaternion.AngleAxis((i * 3f) - 3f, pc.firepos.forward);
                //pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, pc.firepos.rotation * rotationDelta, pc.BaseDamage * DamageReflectionRate);

                // ���݂̃��[�J����]�p���擾
                Vector3 currentLocalEuler = pc.firepos.localEulerAngles;
                // Y���Ɋp�x�����Z�i��F15�x�j
                currentLocalEuler.y += ((i * 3f)-3f);
                // Quaternion�Ƃ��ĕԂ�
                Quaternion modifiedRotation = Quaternion.Euler(currentLocalEuler);
                //���[���h�ϊ�
                Quaternion worldRotation = pc.firepos.parent.rotation * modifiedRotation;
                pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, worldRotation, pc.BaseDamage * DamageReflectionRate);
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
        pc.NowArrowNum -= DrowCost;
        pc.characterUI.ChengeRemainArrowNumText(pc.NowArrowNum, pc.MaxArrowNum);

        //������A�j���[�V����
        anicon.PlayDrowAnimation(DrowRate);
    }
}
