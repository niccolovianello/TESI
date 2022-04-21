using UnityEngine;

public class GyroManager : MonoBehaviour
{
    #region Instance

    private static GyroManager instance;
    public static GyroManager Instance
    {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<GyroManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned GyroManager", typeof(GyroManager)).GetComponent<GyroManager>();
                }

            }
            return instance;
        }
        set {

            instance = value;
        }
    }
    #endregion

    [Header("Logic")]
    private Gyroscope gyro;
    private Quaternion rotation;
    private bool gyroActive;

    public Quaternion GyroRotation
    {
        get {
            return rotation;
        }
    }
    public void EnableGyro()
    {
        if (gyroActive)
            return;
        
        if(SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            gyro.enabled = true;
            gyroActive = gyro.enabled;
        }

        
    }

    private void Update()
    {
        if (gyroActive)
        {
            rotation = gyro.attitude;
            
        }
    }

}
