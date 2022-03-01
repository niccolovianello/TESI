using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class Demon : Enemy
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Text life;
    // [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private Transform AR_Player;
    [SerializeField] private float dangerDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius;
    [SerializeField] private float viewAngle;

    [SerializeField] private ParticleSystem soul;

    [SerializeField] private LayerMask layers;

    private bool hit;

    private Vector3 direction;
    
    private float currentHealth;

    private Animator animator;

    private Renderer[] renderers;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material.shader = Shader.Find("Shader Graphs/Skeleton_Alpha");
        }
        
    }


    private void Update()
    {
        if (life != null)
        {
            if (currentHealth <= 0) life.text = "";
            else life.text = currentHealth.ToString();
        }

        if (currentHealth > 0 && !hit)
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
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DeathClip());
        }
        else StartCoroutine(HitClip());
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
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);

        soul.transform.position = transform.position;
        soul.gameObject.SetActive(true);

        float alpha = 0.95f;

        do
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material.SetFloat("_Alpha", alpha);
            }
            
            alpha -= 0.025f;
            yield return new WaitForSeconds(.025f);
            
        } while (alpha > 0f);
        
        Destroy(gameObject);
        MissionsManager MM = FindObjectOfType<MissionsManager>();
        GameManager gm = FindObjectOfType<GameManager>();
        
        NetworkPlayer networkPlayer = null;
        if (gm.GetIsMission())
            MM.OpenFinishMissionWindow();
        else
        {
            foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
            {
                if(nt.isLocalPlayer)
                    networkPlayer = nt;

                nt.RenderPlayerBody();
            }
            
            
            foreach (Spell spell in FindObjectsOfType<Spell>())
            {
                spell.Destroy();
            }
            
            SceneManager.UnloadSceneAsync("AR_EnemyFight");
            gm.PlayerCameraObject.SetActive(true);
            gm.EnableMainGame();
        }

    }

    private IEnumerator HitClip()
    {
        bool lessThan = Random.Range(0, 10) < 5;
        
        if(lessThan) animator.SetTrigger("Damage");
        else animator.SetTrigger("Knockback");
        
        hit = true;
        yield return new WaitForSeconds(1f);
        hit = false;
    }

    private IEnumerator AttackClip()
    {
        animator.SetBool("Attack", true);
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, layers);

        foreach (Collider collider in colliders)
        {
            collider.GetComponent<HealthManager>().DecreaseHealth(Random.Range(7f, 13f));
        }
        
        yield return new WaitForSeconds(3f);
    }

}
