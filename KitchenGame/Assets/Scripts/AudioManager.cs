using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] clips;
    public AudioClip[] bgms;
    private AudioSource source;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        source = GetComponent<AudioSource>();
    }

    void Start() {
    }

    public void Play(int id, float volume) {
        source.PlayOneShot(clips[id], volume);
    }

    public void StopBGM() {
        source.Stop();
    }

    public void PlayBGM(int id) {
        source.clip = bgms[id];
        PlayBGM();
    }

    public void PlayBGM() {
        source.Play();
    }

    //Add SetClip method or just do that in playbgm
}
