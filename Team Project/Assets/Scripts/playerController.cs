using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    
    [SerializeField] CharacterController characterController;

    [SerializeField] int HP;
    [SerializeField] float characterSpeed;
    [SerializeField] float jumpHeight;
    [SerializeField] int jumpAmount;
    [SerializeField] float gravityPull;


    [SerializeField] float fireRate;
    [SerializeField] int gunDamage;
    [SerializeField] int shootDistance;

    private bool playerOnGround;
    private bool isFiring;
    private int jumps;
    private Vector3 movement;
    private Vector3 velocity;

    void Start()
    {
        
    }

    
    void Update()
    {
        moving();

        if (Input.GetButton("Fire1") && !isFiring) 
        {
            StartCoroutine(shoot());
        }
    }

    void moving()
    {
        playerOnGround = characterController.isGrounded;

        if (playerOnGround && velocity.y < 0) 
        {
            jumpAmount = 0;
            velocity.y = 0;
        }

        movement = Input.GetAxis("Horizontal") * transform.right +
            Input.GetAxis("Vertical") * transform.forward;

        characterController.Move(movement * Time.deltaTime * characterSpeed);

        if (Input.GetButtonDown("Jump") && jumps < jumpAmount)
        {
            jumps++;
            velocity.y += jumpHeight;
        }
        velocity.y += gravityPull * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

    }

    IEnumerator shoot()
    {
        isFiring = true;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)),
            out hit, shootDistance))
        {
            IDamage canDamage = hit.collider.GetComponent<IDamage>();

            if (canDamage != null) 
            {
                canDamage.doDamage(gunDamage);
            }
        }
        yield return new WaitForSeconds(fireRate);
        isFiring = false;
    }

    public void takeDamage(int damage) 
    {
        HP -= damage;
    }
}
