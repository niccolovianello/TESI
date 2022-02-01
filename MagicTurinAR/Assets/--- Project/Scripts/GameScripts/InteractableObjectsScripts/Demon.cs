using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : Enemy
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Text life;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private Transform AR_Player;
    [SerializeField] private float dangerDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private float viewAngle;

    [SerializeField] private ParticleSystem soul;

    [SerializeField] private LayerMask layers;

    [SerializeField] private Material material;

    private Vector3 direction;
    
    private float currentHealth;

    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    
    private void Update()
    {
        if (life != null)
        {
            if (currentHealth <= 0) life.text = "";
            else life.text = currentHealth.ToString();
        }

        if (currentHealth > 0)
        {
            if (isAware())
            {
                Vector3 targetDirection = AR_Player.transform.position - transform.position;
                targetDirection.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.1f);
                transform.Translate(0, 0, runSpeed);
                animator.SetBool("Aware", true);

                if ((transform.position - AR_Player.transform.position).magnitude < attackDistance)
                {
                    Attack();
                }

                else animator.SetBool("Attack", false);
            }
            
            else
            {
                /*
                int dirx;
                int diry;
                int dirz;
                direction = new Vector3(dirx, diry, dirz);
                */

                animator.SetBool("Aware", false);
            }
        }
        
    }

    private void Attack()
    {
        StartCoroutine(AttackClip());
    }

    public void Damage(float damage)
    {
        StartCoroutine(HitClip());

        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            StartCoroutine(DeathClip());
        }
    }

    private bool isAware()
    {
        Vector3 targetDirection = transform.position - AR_Player.transform.position;
        bool viewsARPlayer = Mathf.Abs(Vector3.Angle(Vector3.forward, targetDirection)) < viewAngle && Mathf.Abs((transform.position - AR_Player.transform.position).magnitude) < dangerDistance;
        
        return viewsARPlayer;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


    // ANIMATOR COROUTINES
    private IEnumerator DeathClip()
    {
        animator.SetBool("Death", true);
        yield return new WaitForSeconds(5f);

        
        //fixare
        for (float alpha = 1; alpha > 0; alpha--)
        {
            material.SetFloat("Alpha", alpha);
            yield return new WaitForSeconds(.05f);
        }

        soul.transform.position = transform.position + new Vector3 (0,1f,0);
        soul.gameObject.SetActive(true);

        Destroy(gameObject);
    }

    private IEnumerator HitClip()
    {
        animator.SetBool("Hit", true);
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Hit", false);
    }

    private IEnumerator AttackClip()
    {
        animator.SetBool("Attack", true);
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, layers);

        foreach (Collider collider in colliders)
        {
            Debug.Log(collider);
            collider.GetComponent<HealthManager>().DecreaseHealth();
        }
        yield return new WaitForSeconds(3f);
    }

}
