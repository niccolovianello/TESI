using UnityEngine;

public class Chest : MonoBehaviour
{
    internal SharedARManagerScript Controller = null;

    private Vector3 _pos;
    private Vector3 _initialPosition;

    private bool _isGameStarted;
    private bool _isHost;

    private MessagingManagerChestQuest _messagingManager;

    [SerializeField] 
    private LockControl lockControl;
    [SerializeField] 
    private Rotate[] rounds;

    private void Start()
    {
        _initialPosition = transform.position;
    }

    internal void GameStart(bool isHost, MessagingManagerChestQuest messagingManager)
    {
        _isHost = isHost;
        _isGameStarted = true;
        _initialPosition = transform.position;

        if (!_isHost)
            return;

        _messagingManager = messagingManager;
        
    }


        public void RotateRound(int roundIndex)
    {
        rounds[roundIndex].RotateWheel();
        _messagingManager.BroadCastRoundRotation(roundIndex);
    }
    private void OnEnable()
    {
        LockControl.Unlocked += TriggerAnimation;
    }
    
    private void TriggerAnimation()
    {
        GetComponent<Animator>().SetTrigger("Unlock");
    }
}
