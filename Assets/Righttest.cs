using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Righttest : MonoBehaviour
{

    [SerializeField] Rigidbody rb;
    [SerializeField] float power;
    [SerializeField] float velosity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward*(velosity-rb.velocity.x)*power,ForceMode.Acceleration);
        
    }
}
