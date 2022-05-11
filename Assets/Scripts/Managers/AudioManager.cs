using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioSource MusicAudioSource;
    [Space]
    [SerializeField] private AudioClip buttonClickAudioClip;
    [SerializeField] private List<AudioClip> musicAudioClip;

    private bool SFXPointer = true;
    private bool musicPointer = true;
    private int currentMusicAudioClip = 0;
    
    private static AudioManager instance;

    public static AudioManager Instance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance == this)
        {
            Destroy(gameObject);
        }
        else if (instance == AudioManager.instance)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeSFXSoudsPointer(bool pointer)
    {
        if(pointer)
            SFXPointer = true;
        else
            SFXPointer = false;

        TurnMusicSFX();
    }

    public void ChangeMusicSoudsPointer(bool pointer)
    {
        if(pointer)
            musicPointer = true;
        else
            musicPointer = false;
        
        TurnMusicSFX();
    }

    public bool GetSFXSoudsPointer()
    {
        return SFXPointer;
    }

    public bool GetMusicSoudsPointer()
    {
        return musicPointer;
    }

    public void OnButtonClickPlayAudioClip()
    {
        SFXAudioSource.PlayOneShot(buttonClickAudioClip);
    }

    public void PlayMusic()
    {
        MusicAudioSource.clip = musicAudioClip[currentMusicAudioClip];
        MusicAudioSource.Play();
        
        if (currentMusicAudioClip + 1 != musicAudioClip.Count)
            currentMusicAudioClip++;
        else
            currentMusicAudioClip = 0;
    }

    private void TurnMusicSFX()
    {
        SFXAudioSource.volume = SFXPointer ? 1 : 0;
        MusicAudioSource.volume = musicPointer ? 1 : 0;
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusic();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
