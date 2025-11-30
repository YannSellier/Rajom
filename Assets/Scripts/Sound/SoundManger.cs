using System.Collections;
using UnityEngine;

public class SoundManger : MonoBehaviour
{
    // SoundManger _audioManager = GameObject.FindObjectOfType<SoundManger>()
    // _audioManager.PlaySFX(_audioManager.)
    
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxsSource;
    
    [SerializeField] public AudioClip ambientSound;
    [SerializeField] public AudioClip armsMoving;
    [SerializeField] public AudioClip changeHead;
    [SerializeField] public AudioClip craft;
    [SerializeField] public AudioClip destroyPart;
    [SerializeField] public AudioClip dropPart;
    [SerializeField] public AudioClip takePart;
    [SerializeField] public AudioClip validPart;
    
    private void Start()
    {
        musicSource.clip = ambientSound; 
        musicSource.Play();
    }
    
    public void PlaySfx(AudioClip clip, float duration, bool loop)
    {
        if (clip == null)
            return;

        AudioSource source = GetComponent<AudioSource>();
        if (source == null)
            source = gameObject.AddComponent<AudioSource>();

        source.clip = clip;
        source.loop = loop;
        source.Play();

        if (!loop)
            StartCoroutine(StopAfterDelay(source, duration));
    }

    private IEnumerator StopAfterDelay(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
    }
}
