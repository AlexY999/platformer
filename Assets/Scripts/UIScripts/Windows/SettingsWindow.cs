using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : MonoBehaviour
{
    [SerializeField] private Toggle SFXToggle;
    [SerializeField] private Toggle MusicToggle;

    private void Start()
    {
        SFXToggle.isOn = AudioManager.Instance().GetSFXSoudsPointer();
        MusicToggle.isOn = AudioManager.Instance().GetMusicSoudsPointer();
    }

    public void OnSFXButtonClick(bool pointer)
    {
        UIManager.Instance().OnButtonClisk();
        AudioManager.Instance().ChangeSFXSoudsPointer(pointer);
    }
    
    public void OnMusicButtonClick(bool pointer)
    {
        UIManager.Instance().OnButtonClisk();
        AudioManager.Instance().ChangeMusicSoudsPointer(pointer);
    }
    
    public void OnBackButtonClick()
    {
        UIManager.Instance().PreviousPage();
        UIManager.Instance().OnButtonClisk();
    }
}
