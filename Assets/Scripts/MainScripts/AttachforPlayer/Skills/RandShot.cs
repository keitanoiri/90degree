using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandShot : SkillBase
{
    private void Awake()
    {
        skillName = "RandShot";
        skillExprain = "ランダムに広くに拡散する5本の矢を放つ";

        DrowCost = 5;
        DrowRate = 1.0f;
        DamageReflectionRate = 1.0f;
    }
    public override void Shoot()
    {
        //基本的にマウスが離されたときに行う
        if (pc.CanShoot == true)
        {
            for (int i = 0; i < 5; i++)
            {
                pc.bulletid++;//球のIDを追加

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
        //基本的にマウスが押されたときに行う
        //矢を消費
        if (pc.NowArrowNum < DrowCost)
        {
            //足りない音で知らせる？
            return;
        }
        pc.NowArrowNum-=DrowCost;
        pc.characterUI.ChengeRemainArrowNumText(pc.NowArrowNum, pc.MaxArrowNum);

        //つがえるアニメーション
        anicon.PlayDrowAnimation(DrowRate);
    }
}
