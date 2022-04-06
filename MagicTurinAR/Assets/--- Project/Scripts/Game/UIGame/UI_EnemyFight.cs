using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Mirror;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class UI_EnemyFight : MonoBehaviour
{
    public Canvas canvasWindowPlayerDefeat;
    public Text cameraPosition;
    public Camera camera3;

    public void PlayerDefeatedByDemon()
    {
        canvasWindowPlayerDefeat.enabled = true;
        //camera3 = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        cameraPosition.text = "x: " + camera3.transform.position.x + "\nz: " + camera3.transform.position.z;
    }

    public void RestartFight()
    {
        canvasWindowPlayerDefeat.enabled = false;
        MissionsManager mm = FindObjectOfType<MissionsManager>();
        
        if (mm.currentMission.playerType == MissionSO.PlayerType.Hunter)
        {
            string currentSceneName = mm.currentMission.sceneName;
            SceneManager.UnloadSceneAsync(currentSceneName);
            SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
        }       
        else
        {
            SceneManager.UnloadSceneAsync("AR_EnemyFight");
            SceneManager.LoadSceneAsync("AR_EnemyFight", LoadSceneMode.Additive);
        }

    }

    public void ExitFromFightScene()
    {
        MissionsManager mm = FindObjectOfType<MissionsManager>();
        if (mm.currentMission.playerType == MissionSO.PlayerType.Hunter)
        {
            mm.FinishMission();
        }
        else

        {

            foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
            {
                if (nt.isLocalPlayer)
                    nt.RenderPlayerBody();


            }


            foreach (Spell spell in FindObjectsOfType<Spell>())
            {
                spell.Destroy();
            }

            SceneManager.UnloadSceneAsync("AR_EnemyFight");
            GameManager gm = FindObjectOfType<GameManager>();
            gm.PlayerCameraObject.SetActive(true);
            gm.EnableMainGame();
        }
    }
}
