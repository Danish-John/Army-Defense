using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioMixer BGMusicMixer;
    [SerializeField] Slider BGMusicSlider;

    [SerializeField] AudioMixer SFXMixer;
    [SerializeField] Slider SFXSlider;



    private void Start()
    {

        load();
    }



    private void Update()
    {

        if (LevelManager.Instance)
        {
            if (!GameObject.FindGameObjectWithTag("Police") || !GameObject.FindGameObjectWithTag("Enemy"))
            {
                LevelManager.Instance.FightSound.Stop();
            }
        }

       
        
    }



    public void load()
    {
        if (!PlayerPrefs.HasKey("MusicVol"))
        {
            BGMusicSlider.value = 1;
        }
        else
        {
            BGMusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        }


        if (!PlayerPrefs.HasKey("SFXVol"))
        {
            SFXSlider.value = 1;
        }
        else
        {
            SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        }




    }


    public void SaveSFX(float val)
    {
        PlayerPrefs.SetFloat("SFXVol", val);
    }


    public void SaveMusic(float val)
    {
        PlayerPrefs.SetFloat("MusicVol", val);
    }




    public void SetMusicVolume()
    {
        float volume = BGMusicSlider.value;
        BGMusicMixer.SetFloat("music", Mathf.Log10(volume)*20);

        SaveMusic(volume);


    }


    public void SetSFXvolume()
    {
        float volume = SFXSlider.value;
        SFXMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        SaveSFX(volume);

    }


}
