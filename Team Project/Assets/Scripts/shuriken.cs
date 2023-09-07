using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shuriken : MonoBehaviour
{
    [SerializeField] Rigidbody rb; 

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;

    private GameObject shooter;

    public void setShooter(GameObject shooter)
    {
        this.shooter = shooter; 
    }
    void Start()
    {
        rb.velocity = transform.forward * speed; 
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null)
        {
            damageable.takeDamage(damage);
        }

        Destroy(gameObject); 
    }

}
