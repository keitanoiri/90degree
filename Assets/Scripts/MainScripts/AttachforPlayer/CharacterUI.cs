using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private Image ProgresImage;
    [SerializeField] private Animator animator;
    [SerializeField] private Image[] firemode;
    [SerializeField] private TextMeshProUGUI RemainArrows;
    // Update is called once per frame
    private void Start()
    {
        if (!photonView.IsMine)
        {
            enabled = false;
        }else
        {
            GameObject GMObje = GameObject.Find("GameManager");
            if (GMObje != null)
            {
                if (GMObje.TryGetComponent<GameManager>(out GameManager gm))
                {
                    firemode[0].sprite = gm.Skill0Sprite;
                    firemode[1].sprite = gm.Skill1Sprite;
                    firemode[2].sprite = gm.Skill2Sprite;
                }
            }
        }
    }
    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(1);
        if (stateInfo.IsName("Drow"))
        {
            // アニメーションの進行度を取得（0.0 ～ 1.0）
            float progress = stateInfo.normalizedTime % 1; // ループするアニメーションに対応
            ProgresImage.fillAmount = progress;
        }
        else if(stateInfo.IsName("hold"))
        {
            ProgresImage.fillAmount = 1;
            ProgresImage.color = Color.red;
        }
        else
        {
            ProgresImage.fillAmount = 0;
            ProgresImage.color = Color.white;
        }
    }

    public void ChengeRemainArrowNumText(int remainarrownum,int maxarrownum)
    {
        RemainArrows.text = remainarrownum.ToString()+"/"+maxarrownum.ToString();
    }

    public void firemodeUI(int firemodeint)
    {
        if(firemodeint<= firemode.Length)
        {
            Color c_c;
            switch(firemodeint)
            {
                
                case 0:
                    c_c = firemode[0].color;
                    c_c.a = 1;
                    firemode[0].color = c_c;
                    c_c = firemode[1].color;
                    c_c.a = 0.3f;
                    firemode[1].color = c_c;
                    c_c = firemode[2].color;
                    c_c.a = 0.3f;
                    firemode[2].color = c_c;
                    break;
                case 1:
                    c_c = firemode[0].color;
                    c_c.a = 0.3f;
                    firemode[0].color = c_c;
                    c_c = firemode[1].color;
                    c_c.a = 1;
                    firemode[1].color = c_c;
                    c_c = firemode[2].color;
                    c_c.a = 0.3f;
                    firemode[2].color = c_c;
                    break;  
                case 2:
                    c_c = firemode[0].color;
                    c_c.a = 0.3f;
                    firemode[0].color = c_c;
                    c_c = firemode[1].color;
                    c_c.a = 0.3f;
                    firemode[1].color = c_c;
                    c_c = firemode[2].color;
                    c_c.a = 1;
                    firemode[2].color = c_c;
                    break;

            }
        }
    }
}
