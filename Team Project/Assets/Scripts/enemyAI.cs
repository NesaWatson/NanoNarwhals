using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour
{
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform attackPos;

    [SerializeField] int HP;
    [SerializeField] int targetFaceSpeed;

    [SerializeField] float attackRate;
    [SerializeField] GameObject shuriken;

    Vector3 playerDir;
    Vector3 pushBack;
    bool playerInRange;
    bool isAttacking;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator attack()
    {
        isAttacking = true;
        Instantiate(shuriken, attackPos.position, transform.rotation); 
        yield return new WaitForSeconds(attackRate);
        isAttacking = false; 


    }
    IEnumerator flashDamage()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.01f);
        model.material.color = Color.white;
    }
    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir); 
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * targetFaceSpeed);
    }
}
