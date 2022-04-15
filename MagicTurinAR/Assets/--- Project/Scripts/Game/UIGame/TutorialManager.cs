using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

       [SerializeField] private GameObject levelGui;
       
       [SerializeField] private List<GameObject> tutorialPanels;

       private int _activePanel;

       private void Start()
       {
              foreach (var panel in tutorialPanels)
              {
                     panel.SetActive(false);
              }
              
              _activePanel = 0;
              tutorialPanels[_activePanel].SetActive(true);
       }

       public void SwitchTutorialPanel()
       {
              _activePanel++;
              
              Debug.Log(_activePanel);

              
              tutorialPanels[_activePanel - 1].SetActive(false);
              
              if (!(_activePanel > tutorialPanels.Count - 1))
              {
                     tutorialPanels[_activePanel].SetActive(true);
                     return;
              }
              
              gameObject.SetActive(false);
              
              if(levelGui) levelGui.SetActive(true);
       }

}
