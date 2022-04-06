using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class Demon : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    
    [SerializeField] private Text life;
    
    [SerializeField] private float runSpeed;
    
    [SerializeField] private float dangerDistance;
    
    [SerializeField] private float attackDistance;
    
    [SerializeField] private Transform attackPoint;
    
    [SerializeField] private float attackRadius;
    
    [SerializeField] private float viewAngle;
    
    [SerializeField] private ParticleSystem soul;

    [SerializeField] private LayerMask layers;
    
    private CapsuleCollider _collider;
    
    public HealthManager _arPlayerHealthManager;

    private bool _hit;
    
    private float _currentHealth;

    private Animator _animator;

    private Renderer[] _renderers;
    
    // private bool justHit = false;


    private void Awake()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collider.isTrigger = true;
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
    }

    private void Start()
    {
        _renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in _renderers)
        {
            renderer.material.shader = Shader.Find("Shader Graphs/Skeleton_Alpha");
        }

        _arPlayerHealthManager = FindObjectOfType<HealthManager>();

    }


    private void Update()
    {
        if (life != null)
        {
            if (_currentHealth <= 0) life.text = "";
            else life.text = _currentHealth.ToString();
        }

        if (_currentHealth > 0 && !_hit)
        {
            if (IsAware())  //|| justHit
            {
                Vector3 targetDirection = _arPlayerHealthManager.transform.position - transform.position;
                targetDirection.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(targetDirection), 0.1f);
                transform.Translate(0, 0, runSpeed);
                _animator.SetBool("Aware", true);

                if ((transform.position - _arPlayerHealthManager.transform.position).magnitude < attackDistance)
                {
                    Attack();
                }

                else _animator.SetBool("Attack", false);
            }

            else
            {
                _animator.SetBool("Aware", false);
            }
        }
        
    }

    private void Attack()
    {
        StartCoroutine(AttackClip());
    }

    public void Damage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DeathClip());
        }
        else StartCoroutine(HitClip());
    }

    private bool IsAware()
    {
        var playerPosition = _arPlayerHealthManager.transform.position;
        var thisPosition = transform.position;
        
        Vector3 targetDirection = thisPosition - playerPosition;
        
        bool distanceARPlayer = Mathf.Abs((thisPosition - playerPosition).magnitude) < dangerDistance;
        bool angleARplayer = Mathf.Abs(Vector3.Angle(Vector3.forward, targetDirection)) < viewAngle;
        
        // Debug.Log(distanceARPlayer + " " + angleARplayer);

        return distanceARPlayer && angleARplayer ;
    }

    private void OnDrawGizmos()
    {
        if (attackPoint == null) return;
        
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }


    // ANIMATOR COROUTINES
    private IEnumerator DeathClip()
    {
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(3f);
        
        Instantiate(soul, transform);

        float alpha = 0.95f;

        do
        {
            foreach (Renderer renderer in _renderers)
            {
                renderer.material.SetFloat("_Alpha", alpha);
            }
            
            alpha -= 0.025f;
            yield return new WaitForSeconds(.025f);
            
        } while (alpha > 0f);

        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
        MissionsManager mm = FindObjectOfType<MissionsManager>();
        GameManager gm = FindObjectOfType<GameManager>();
        
        NetworkPlayer networkPlayer = null;
        if (gm.GetIsMission())
            mm.OpenFinishMissionWindow();
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

    //private IEnumerator AfterHitClip()
    //{
    //    justHit = true;

    //    yield return new WaitForSeconds(3f);

    //    justHit = false;
    //}


    private IEnumerator HitClip()
    {
        //bool lessThan = Random.Range(0, 10) < 5;

        //if(lessThan) animator.SetTrigger("Damage");
        //else animator.SetTrigger("Knockback");

        _animator.SetTrigger("Damage");

        _hit = true;
        yield return new WaitForSeconds(1f);

        // StartCoroutine(AfterHitClip());

        _hit = false;
    }

    private IEnumerator AttackClip()
    {
        _animator.SetBool("Attack", true);

        yield return new WaitForSeconds(1f);
        Collider[] colliders = Physics.OverlapSphere(attackPoint.position, attackRadius, layers);

        foreach (Collider collider in colliders)
        {
            // collider.GetComponent<HealthManager>().DecreaseHealth(Random.Range(7f, 13f));
            collider.GetComponent<HealthManager>().DecreaseHealth(1f);
        }
        
        yield return new WaitForSeconds(1f);
        _animator.SetBool("Attack", false);
    }

}
