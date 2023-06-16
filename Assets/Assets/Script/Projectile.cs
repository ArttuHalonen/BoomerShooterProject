using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime;
    Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 100f, ForceMode.Impulse);
    }
    private void OnTriggerEnter (Collider other)
    {
        Destroy(this.gameObject);
    }
}
