using UnityEngine;
using UnityEngine.UI;

public class FrameRateControl : MonoBehaviour
{
    public Slider frameRateSlider;

    void Start()
    {
        frameRateSlider.onValueChanged.AddListener(ChangeFrameRate);
    }

    private void ChangeFrameRate(float value)
    {
        int desiredFrameRate = Mathf.RoundToInt(value);
        Application.targetFrameRate = desiredFrameRate;
        Debug.Log("Target Frame Rate: " + Application.targetFrameRate);
    }

    void Update()
    {
        
    }
}
