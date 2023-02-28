using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplovinInit : MonoBehaviour
{
    public static ApplovinInit I { get; private set; }

    private void Awake()
    {
        if (I != null)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);
        
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
        };

        MaxSdk.SetSdkKey("fGPX53TyPO5ph4by8VQs_R943x6LAKycLO_4TSeApzT9VpgH3BQApmTF0yfm10a6n0XKq1kx92TGkWTTemvo01");
        MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
    }
}
