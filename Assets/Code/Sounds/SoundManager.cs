using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioClip[] mainMusic;
    public GameObject audioPrefab;

    public AudioMixerGroup master;
    public AudioMixerGroup music;
    public AudioMixerGroup FX;
    void Start()
    {
        instance = this;
        
    }

    

    
}
