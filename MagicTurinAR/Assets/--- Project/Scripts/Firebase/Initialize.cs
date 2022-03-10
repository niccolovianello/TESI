using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Analytics;
using UnityEngine;

public class Initialize : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        }
        );
    }
}
