using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class SoundHolder : MonoBehaviour
{
    public AudioClip[] holder;

    public void PlayFX(int num, float volume)
    {
        var sound = Instantiate(SoundManager.instance.audioPrefab, transform.position, Quaternion.identity);
        sound.GetComponent<AudioSource>().clip = holder[num];
        sound.GetComponent<AudioSource>().volume = volume;
        sound.GetComponent<AudioSource>().Play();
        Destroy(sound, holder[num].length);
    }
}
