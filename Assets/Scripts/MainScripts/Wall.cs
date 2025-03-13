using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.TryGetComponent<Ballet>(out var bullet))//‚â‚ª•Ç‚É‚ ‚½‚Á‚½‚ç
        {
            bullet.StopBullet();
            Destroy(bullet.hitCollider);
        }
    }
}
