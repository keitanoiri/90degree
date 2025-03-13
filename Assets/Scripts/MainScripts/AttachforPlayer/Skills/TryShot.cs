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
        skillExprain = "横向きに拡散する三本の矢を放つ";

        DrowCost = 3;
        DrowRate = 1.0f;
        DamageReflectionRate = 1.0f;
    }
    public override void Shoot()
    {
        //基本的にマウスが離されたときに行う
        if (pc.CanShoot == true)
        {
            for (int i = 0; i < 3; i++)
            {
                pc.bulletid++;//球のIDを追加
                Quaternion rotationDelta = Quaternion.AngleAxis((i * 3f) - 3f, pc.gravityForce);
                pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, pc.firepos.rotation * rotationDelta, pc.BaseDamage * DamageReflectionRate);
            }

            pc.CanShoot = false;

            anicon.PlayShotAnimation();
        }
        anicon.ResetAimAnimations();
    }

    public override void Drow()
    {
        //基本的にマウスが押されたときに行う
        //矢を消費
        if (pc.NowArrowNum < DrowCost)
        {
            //足りない音で知らせる？
            return;
        }
        pc.NowArrowNum -= DrowCost;
        pc.characterUI.ChengeRemainArrowNumText(pc.NowArrowNum, pc.MaxArrowNum);

        //つがえるアニメーション
        anicon.PlayDrowAnimation(DrowRate);
    }
}
