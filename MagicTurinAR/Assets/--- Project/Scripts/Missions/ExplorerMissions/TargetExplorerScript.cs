using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetExplorerScript : MonoBehaviour
{
    private MagicPlayer magicPlayer;
    public int distanceToEnableRender = 20;
    private MeshRenderer meshRenderer;
    private Collider targetCollider;
    private bool rendered = false;
    private MissionsManager missionManager;

    private void Start()
    {
        magicPlayer = FindObjectOfType<MagicPlayer>();
        meshRenderer = GetComponent<MeshRenderer>();
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
        meshRenderer.enabled = true;
        targetCollider.enabled = true;
    }

    private void NotRenderTargetExplorer()
    {
        meshRenderer.enabled = false;
        targetCollider.enabled = false;
    }
}
