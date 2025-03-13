using Photon.Pun.Demo.Asteroids;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class ArrowShot : SkillBase
// Start is called before the first frame update
{
     

    private void Awake()
    {
        skillName="ArrowShot";
        skillExprain="矢を放つ";

        DrowCost=1;
        DrowRate=1.0f;
        DamageReflectionRate = 1.0f;
    }
    public override void Shoot()
    {
        //基本的にマウスが離されたときに行う
        if(pc.CanShoot == true)
        {
            pc.bulletid++;//球のIDを追加
            pc.photonView.RPC(nameof(pc.Fire), RpcTarget.All, pc.bulletid, pc.gravityForce, pc.firepos.position, pc.firepos.rotation,pc.BaseDamage*DamageReflectionRate);
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
