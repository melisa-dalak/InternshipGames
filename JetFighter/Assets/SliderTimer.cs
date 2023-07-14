using UnityEngine;
using UnityEngine.UI;

public class SliderTimer : MonoBehaviour
{
    public Gamer gamer;
    public Slider slider;
    private void Update()
    {
        UpdateSliderValue();
    }

    public void UpdateSliderValue()
    {
        float remainingTime = gamer.inputTimer.GetRemainingTime();
        float totalTime = gamer.inputTimer.duration;
        float progress = 1f - (remainingTime / totalTime);

        slider.value = progress;
    }
}
