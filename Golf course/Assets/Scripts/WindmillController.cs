using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillController : MonoBehaviour
{
    public float spinSpeed;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = spinSpeed;
    }


    void Update()
    {
        rb.AddTorque(Vector3.forward * 1000, ForceMode.Impulse);
    }
}
