using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetExplorer : MonoBehaviour
{
    [SerializeField] private GameObject[] children;
    
    private MagicPlayer magicPlayer;
    public int distanceToEnableRender = 20;
    private Collider targetCollider;
    private bool rendered = false;
    private MissionsManager missionManager;

    private void Start()
    {
        magicPlayer = FindObjectOfType<MagicPlayer>();
        targetCollider = GetComponent<Collider>();
        missionManager = FindObjectOfType<MissionsManager>();
        NotRenderTargetExplorer();
    }

    private void Update()
    {
        if (Vector3.Distance(this.gameObject.transform.position, magicPlayer.gameObject.transform.position) < distanceToEnableRender && rendered == false)
        {
            rendered = true;
            RenderTargetExplorer();
        }
        else if (Vector3.Distance(this.gameObject.transform.position, magicPlayer.gameObject.transform.position) > distanceToEnableRender && rendered == true)
        {
            rendered = false;
            NotRenderTargetExplorer();
        }
    }
    private void OnMouseDown()
    {
        missionManager.OpenFinishMissionWindow();
    }

    private void RenderTargetExplorer()
    {
        foreach (var child in children)
        {
            child.SetActive(true);
        }
        targetCollider.enabled = true;
    }

    private void NotRenderTargetExplorer()
    {
        foreach (var child in children)
        {
            child.SetActive(false);
        }
        targetCollider.enabled = false;
    }
}
