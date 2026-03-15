using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField]
    private GameObject pausePanel;

    [Header("Volume Controls")]
    [SerializeField]
    private Slider masterSlider;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;

    [Header("Audio Mixer & Groups")]
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioMixerGroup masterGroup;
    [SerializeField]
    private AudioMixerGroup musicGroup;
    [SerializeField]
    private AudioMixerGroup sfxGroup;

    public static Menu instance;
    public bool isEnabled;

    private const float MinDb = -80f;
    private const float MaxDb = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;

        // Ensure the pause panel is hidden at game start
        if (pausePanel != null)
            pausePanel.SetActive(false);

        isEnabled = pausePanel != null && pausePanel.activeSelf;

        // Initialize slider values from mixer groups
        TryInitSlider(masterSlider, masterGroup, "MasterVolume");
        TryInitSlider(musicSlider, musicGroup, "MusicVolume");
        TryInitSlider(sfxSlider, sfxGroup, "SfxVolume");

        // Wire up slider change callbacks
        if (masterSlider != null)
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
        if (musicSlider != null)
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        if (sfxSlider != null)
            sfxSlider.onValueChanged.AddListener(SetSfxVolume);
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame && pausePanel != null)
        {
            pausePanel.SetActive(!pausePanel.activeSelf);
            isEnabled = pausePanel.activeSelf;
        }
    }

    private void TryInitSlider(Slider slider, AudioMixerGroup group, string parameterName)
    {
        if (slider == null || audioMixer == null || group == null)
            return;

        if (audioMixer.GetFloat(parameterName, out var dB))
        {
            // Convert dB to logarithmic slider value (0-1)
            float sliderValue = (dB > MinDb) ? Mathf.Pow(10, dB / 20) : 0f;
            slider.SetValueWithoutNotify(sliderValue);
        }
        else
        {
            // Default to full volume if the parameter is missing
            slider.SetValueWithoutNotify(1f);
        }
    }

    private void SetMasterVolume(float value)
    {
        SetVolume(masterGroup, "masterVolume", value);
    }

    private void SetMusicVolume(float value)
    {
        SetVolume(musicGroup, "musicVolume", value);
    }

    private void SetSfxVolume(float value)
    {
        SetVolume(sfxGroup, "fxVolume", value);
    }

    private void SetVolume(AudioMixerGroup group, string parameterName, float sliderValue)
    {
        if (audioMixer == null || group == null)
            return;

        // Convert logarithmic slider value (0-1) to dB
        float dB = (sliderValue > 0) ? Mathf.Log10(sliderValue) * 20 : MinDb;
        dB = Mathf.Clamp(dB, MinDb, MaxDb);
        audioMixer.SetFloat(parameterName, dB);
    }
}
