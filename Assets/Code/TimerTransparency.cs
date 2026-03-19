using UnityEngine;
using UnityEngine.UI;

public class TimerTransparency : MonoBehaviour
{
    public float timerDuration = 5f; // Duration of the timer in seconds
    public Image image; // Reference to the UI Image component

    private float currentTime;

    void Start()
    {
        currentTime = timerDuration;
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            float alpha = 1 - (currentTime / timerDuration); // Alpha increases from 0 to 1 over time
            image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        }
        else
        {
            // Ensure alpha is fully opaque when timer ends
            image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
            enabled = false; // Disable the script to stop updating
        }
    }
}