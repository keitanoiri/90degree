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
        // パーティクルシステムが再生終了しており、すべてのパーティクルが消滅した場合
        if (!shieldParticle.IsAlive())
        {
            Debug.Log("パーティクルシステムの寿命が終了しました");
            P_Shield.SetActive(false);
        }
    }
}
