using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //�ǂɒǉ�����X�N���v�g
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent<Ballet>(out var bullet))//��ǂɂ���������i���������̂���Ȃ�j
        {
            bullet.StopBullet();
            Destroy(bullet.hitCollider);
        }
    }
}
