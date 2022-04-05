using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryUIManager : MonoBehaviour
{
    [SerializeField] private CanvasRenderer helpUiPanel;

    public void CloseHelpUiPanel()
    {
        helpUiPanel.gameObject.SetActive(false);
    }
}
