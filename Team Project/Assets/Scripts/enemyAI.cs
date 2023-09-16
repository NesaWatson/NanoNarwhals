using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator animate;
    [SerializeField] Transform attackPos;
    [SerializeField] Transform headPos;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)][SerializeField] int HP;
    [Range(1, 10)][SerializeField] int targetFaceSpeed;
    [Range(45, 180)][SerializeField] int viewAngle;
    [Range(5, 25)][SerializeField] int wanderDist;
    [Range(5, 25)][SerializeField] int wanderTime;
    [SerializeField] float animSpeed; 

    [Header("----- Weapon Stats -----")]
    [SerializeField] float attackRate;
    [SerializeField] int attackAngle;
    [SerializeField] GameObject[] shurikens;

    Vector3 playerDir;
    Vector3 pushBack;
    bool playerInRange;
    bool isAttacking;
    float stoppingDistOrig;
    float angleToPlayer;
    bool wanderDestination;
    Vector3 startingPos;
    enemySpawner spawner;
    Transform playerTransform;
    bool isAlerted;
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        gameManager.instance.updateGameGoal(1);

        playerTransform = gameManager.instance.player.transform;
        spawner = FindObjectOfType<enemySpawner>();

        enemyManager.instance.registerAlertedEnemy(this);
    }
    void Update()
    {
        float agentVel = agent.velocity.normalized.magnitude;

        animate.SetFloat("Speed", Mathf.Lerp(animate.GetFloat("Speed"), agentVel, Time.deltaTime + animSpeed));
      
        if(playerInRange && canViewPlayer())
        {
            setAlerted(playerDir);
            
            if (!isAttacking && angleToPlayer <= attackAngle)
            {

                StartCoroutine(attack());
            }
        }
        else
        {
            StopCoroutine(attack());
            isAttacking = false;
            StartCoroutine(wander());
        }
        //else if(playerInRange && canViewPlayer())
        //{
        //    setAlerted(playerDir);
        //}
    }
    public void setAlerted(Vector3 playerPos)
    {
        if (!isAlerted)
        {
            isAlerted = true;
            agent.SetDestination(playerPos);
        }
    }
    IEnumerator wander()
    {
        if (agent.remainingDistance < 0.05f && !wanderDestination)
        {
            wanderDestination = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(wanderTime);

            Vector3 randomPos = Random.insideUnitSphere * wanderDist;
            randomPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randomPos, out hit, wanderDist, 1);
            agent.SetDestination(hit.position);

            wanderDestination = false;
        }
    }
    bool canViewPlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
#if(UNITY_EDITOR)
        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, playerDir);
#endif
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                enemyManager.instance.alertAllEnemies(gameManager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    faceTarget();

                    if (!isAttacking && angleToPlayer <= attackAngle)
                    {
                        StartCoroutine(attack());
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    } 
    IEnumerator attack()
    {
        while (playerInRange)
        {
            isAttacking = true;

            int randomIndex = Random.Range(0, shurikens.Length);
            GameObject seletedShuriken = shurikens[randomIndex];

            Instantiate(seletedShuriken, attackPos.position, transform.rotation)
            .GetComponent<shuriken>()
                .SetShooter(gameObject);
            //shuriken shurikenScript = newShuriken.GetComponent<shuriken>();
            
            //if(shurikenScript != null )
            //{
            //    shurikenScript.SetShooter(gameObject);
            //}
            yield return new WaitForSeconds(attackRate);
        }
        isAttacking = false; 
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        StartCoroutine(flashDamage());
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (HP <= 0)
        {
            gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
        if(spawner != null)
        {
            spawner.EnemyDestroyed();
        }
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
    void onDestory()
    {
        if (enemyManager.instance != null)
        {
            enemyManager.instance.unregisteredAlertedEnemies(this);
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
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
