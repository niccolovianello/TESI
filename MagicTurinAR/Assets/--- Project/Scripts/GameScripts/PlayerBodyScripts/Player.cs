using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Player : MonoBehaviour, PlayerInterface
{
    
    public bool IsNearTeamMembers()
    {
        return false;
    }

    

    public abstract void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item);


    public void ClickOnItem(Item item) { }
    
}