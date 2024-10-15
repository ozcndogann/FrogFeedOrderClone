using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource MusicSource, SFXSource;
    public Sound[] MusicSounds, SFXSounds;
    private GameObject music, sfx;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        PlayMusic("Music");
        music = GameObject.FindGameObjectWithTag("Music");
        sfx = GameObject.FindGameObjectWithTag("SFX");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(MusicSounds, x => x.name == name);

        if (s == null)
        {

            Debug.Log("Couldn't find music.");

        }
        else
        {
            MusicSource.clip = s.clip;
            MusicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);

        if (s == null)
        {

            Debug.Log("Couldn't find SFX.");

        }
        else
        {
            SFXSource.PlayOneShot(s.clip);
        }

    }

}
