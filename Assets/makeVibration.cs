using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeVibration : MonoBehaviour
{

    private AndroidJavaObject vibrator;
    private AndroidJavaObject currentActivity;
    private AndroidJavaObject unityPlayer;
    private bool isVibrating = true;

    void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if (isVibrating)
        Handheld.Vibrate();
    }

    private void Initialize()
    {
        if (isAndroid())
        {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }

    public void StartVibration()
    {
        isVibrating = true;
        if (isAndroid())
        {
            // Start the vibration coroutine
            vibrator.Call("vibrate", 10000);
        }
        else
        {
            // If not on Android, use default Unity vibration
            Handheld.Vibrate();
        }
    }

    public void st()
    {
        isVibrating=true;
            //Handheld.Vibrate();
    }


    public void repeatVibration()
    {
        while (isVibrating)
        {
            Handheld.Vibrate();
        }        
    }



    public void StopVibration()
    {
        isVibrating = false;
        if (isAndroid())
        {
            vibrator.Call("cancel");
        }
        else
        {
            // If not on Android, or if vibration coroutine is not running, stop the default Unity vibration
            Handheld.Vibrate();
        }
    }


    public bool HasVibrator()
    {
        return isAndroid();
    }

    private bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }
}
