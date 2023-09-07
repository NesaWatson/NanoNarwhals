using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuriken : MonoBehaviour
{
    [SerializeField] Rigidbody rb; 

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime; 
    void Start()
    {
        rb.velocity = transform.forward * speed; 
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject); 
    }

}
