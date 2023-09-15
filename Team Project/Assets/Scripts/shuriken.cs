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
        rb.velocity = (gameManager.instance.player.transform.position - transform.forward).normalized * speed;
        Destroy(gameObject, destroyTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.isTrigger)
        {
            return;
        }
        Debug.Log("Shuriken collided with: " + other.name);

        IDamage damageable = other.GetComponent<IDamage>();

        if (damageable != null && other.gameObject != shooter)
        {
            Debug.Log("Shuriken damaged: " + other.name);
            damageable.takeDamage(damage);
        }

        Destroy(gameObject); 
    }

}
