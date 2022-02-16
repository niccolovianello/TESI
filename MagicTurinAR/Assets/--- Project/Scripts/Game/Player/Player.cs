using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public abstract class Player : NetworkBehaviour, PlayerInterface
{

    public abstract bool IsCloseToTeamMembers();

    
    public abstract void OpenDialogWindowToSeeArtifactsInAR(MagicItemSO item);


    public void ClickOnItem(Item item) { }



}