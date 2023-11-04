using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class vibration : MonoBehaviour
{
    // Start is called before the first frame update
    public Button vibrateToggleButton;
    private bool isVibrationOn = true;
    void Start()
    {
        vibrateToggleButton.onClick.AddListener(ToggleVibration);
    }

    void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        if (isVibrationOn )
        {
            Handheld.Vibrate();
        }
        else
        {
            Handheld.Vibrate();
        }
    }
}
