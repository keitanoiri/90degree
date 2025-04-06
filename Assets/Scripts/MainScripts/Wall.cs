using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //壁に追加するスクリプト
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent<Ballet>(out var bullet))//矢が壁にあたったら（当たったのが矢なら）
        {
            bullet.StopBullet();
            Destroy(bullet.hitCollider);
        }
    }
}
