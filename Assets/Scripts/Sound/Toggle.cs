using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toggle : MonoBehaviour
{
    public Sprite[] sprites;
    public void ToggleMusic()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (AudioManager.Instance.MusicSource.mute)
        {
            AudioManager.Instance.MusicSource.mute = false;
            GameObject.FindGameObjectWithTag("Music").GetComponent<Image>().sprite = sprites[0];
        }
        else
        {
            AudioManager.Instance.MusicSource.mute = true;
            GameObject.FindGameObjectWithTag("Music").GetComponent<Image>().sprite = sprites[1];
        }
    }
    public void ToggleSfx()
    {
        AudioManager.Instance.PlaySFX("Button");
        if (AudioManager.Instance.SFXSource.mute)
        {
            AudioManager.Instance.SFXSource.mute = false;
            GameObject.FindGameObjectWithTag("SFX").GetComponent<Image>().sprite = sprites[2];
        }
        else
        {
            AudioManager.Instance.SFXSource.mute = true;
            GameObject.FindGameObjectWithTag("SFX").GetComponent<Image>().sprite = sprites[3];
        }

    }
}
