using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
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
    bool canSeePlayer;
    bool isAttacking;
    void Start()
    {
        gameManager.instance.updateGameGoal(1);
    }
    void Update()
    {
        if(canSeePlayer)
        {
            playerDir = gameManager.instance.player.transform.position - transform.position;

            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                faceTarget(); 

                if(!isAttacking )
                {
                    {
                        StartCoroutine(attack()); 
                    }
                }
            }
            agent.SetDestination(gameManager.instance.player.transform.position); 
        }
    }
    bool CanSeePlayer()
    {
        Ray ray = new Ray(transform.position, playerDir);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                return true; 
            }
        }
        return false; 
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
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }
    public void physics(Vector3 dir)
    {
        agent.velocity += dir / 3;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSeePlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canSeePlayer = false;
        }
    }
}
