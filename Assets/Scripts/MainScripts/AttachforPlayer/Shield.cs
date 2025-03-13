using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] GameObject P_Shield;
    [SerializeField] ParticleSystem shieldParticle;
    public void ActivateShield()
    {
        //5byoukann
        if (P_Shield != null)
        {
            P_Shield.SetActive(true);   
        }
    }

    private void Update()
    {
        // �p�[�e�B�N���V�X�e�����Đ��I�����Ă���A���ׂẴp�[�e�B�N�������ł����ꍇ
        if (!shieldParticle.IsAlive())
        {
            Debug.Log("�p�[�e�B�N���V�X�e���̎������I�����܂���");
            P_Shield.SetActive(false);
        }
    }
}
