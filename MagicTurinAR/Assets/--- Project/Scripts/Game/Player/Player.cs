using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Player : NetworkBehaviour, PlayerInterface
{
    
    public bool IsNearTeamMembers()
    {
        return false;
    }
    
    public abstract void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item);


    public void ClickOnItem(Item item) { }

    public abstract void RenderPlayerBody();


    public abstract void NotRenderPlayerBody();

}