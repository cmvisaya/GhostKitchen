using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] clips;
    public AudioClip[] bgms;
    public AudioSource source;
    private bool muted = false;

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
        if(source == null) {
            source = GetComponent<AudioSource>();
        }
    }

    void Update() {
        if(Input.GetButtonDown("Mute")) {
            if(muted) {
                source.volume = 1f;
            } else {
                source.volume = 0f;
            }
            muted = !muted;
        }
    }

    public void Play(int id, float volume) {
        if(source != null)
        source.PlayOneShot(clips[id], volume);
    }

    public void StopBGM() {
        if(source != null)
        source.Stop();
    }

    public void PlayBGM(int id) {
        if(source != null)
        source.clip = bgms[id];
        PlayBGM();
    }

    public void PlayBGM() {
        if(source != null)
        source.Play();
    }

    //Add SetClip method or just do that in playbgm
}
