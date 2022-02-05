using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlayerInterface
{
    
    public bool IsNearTeamMembers();
    
    public void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item);

    public void ClickOnItem(Item item);

    public void RenderPlayerBody();

    public void NotRenderPlayerBody();
    
}