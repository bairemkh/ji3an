using System.Collections;
using UnityEngine;

public class VibrationController : MonoBehaviour
{
    private AndroidJavaObject androidVibrator;
    private Coroutine vibrationCoroutine;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        // Check if the device is Android
        if (isAndroid())
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            androidVibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
        }
    }

    public void StartVibration()
    {
        if (isAndroid())
        {
            // Start the vibration coroutine
            vibrationCoroutine = StartCoroutine(LongVibrationCoroutine());
        }
        else
        {
            // If not on Android, use default Unity vibration
            Handheld.Vibrate();
        }
    }

    public void StopVibration()
    {
        if (isAndroid() && vibrationCoroutine != null)
        {
            // Stop the vibration coroutine
            StopCoroutine(vibrationCoroutine);
            // Stop the vibration
            androidVibrator.Call("cancel");
        }
        else
        {
            // If not on Android, or if vibration coroutine is not running, stop the default Unity vibration
            Handheld.Vibrate();
        }
    }

    private IEnumerator LongVibrationCoroutine()
    {
        // Vibrate for 10 seconds (adjust the duration as needed)
        androidVibrator.Call("vibrate", 10000);

        // Wait for the vibration duration to complete
        yield return new WaitForSeconds(10.0f);

        // Stop the vibration
        androidVibrator.Call("cancel");
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
