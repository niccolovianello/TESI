using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : Item
{
    private CapsuleCollider collider;

    private bool rendered = false;
    
    [SerializeField] private GameObject body;
    
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
    }

    internal override void Update()
    {
        base.Update();
        if (!IsClickable() && rendered)
        {
            DoNotRenderItem();
            rendered = false;
        }

        if (IsClickable() && !rendered)
        {
            RenderItem();
            rendered = true;
        }
    }
    
    public override void OnMouseDown()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        SceneManager.LoadScene("AR_EnemyFight", LoadSceneMode.Additive);
        gameManager.PlayerCameraObject.SetActive(false);
        gameManager.DisableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }
        Destroy(gameObject);
    }

    public override void DoNotRenderItem()
    {
        body.SetActive(false);
        collider.enabled = false;
    }

    public override void RenderItem()
    {
        body.SetActive(true);
        collider.enabled = true;
    }
}
